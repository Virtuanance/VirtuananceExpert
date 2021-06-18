using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public enum AnnotationsTools
{
    None,
    Pencil,
    Dropper,
    Hand
}
public class Annotations : MonoBehaviour
{
    private Camera camera;
    public Material lineMaterial;
    public GameObject model;
    public float lineWidth = 1.0f;
    public float depth = 2;
    public Texture pencilIcon;
    public Texture dropperIcon;
    public Texture handIcon;
    public Texture noneIcon;
    public Canvas canvas;
    public AnnotationsTools currentTool;
    private List<GameObject> ui_degree_inputs;
    private List<GameObject> ui_color_sliders;
    private Slider ui_slider;
    private string path;
    private List<Vector3> drawPoints;
    public List<GameObject> drawings;
    private RaycastHit hit;
    private List<string> fileContent;
    private Vector3 pivotPointScreen;
    public Vector3 pivotPointWorld;
    private bool lockDepth = false,sliding = false;
    private int lineID = 0;

    //private Vector3? lineSP = null; //"?" makes it nullable
    // Start is called before the first frame update
    void Start()
    {
        path = Application.dataPath +"/paths.txt";
        File.WriteAllLines(path, new List<String>());
        fileContent = new List<string>();
        ui_degree_inputs = new List<GameObject>();
        ui_degree_inputs.Add(GameObject.Find("X_Slider"));
        ui_degree_inputs.Add(GameObject.Find("Y_Slider"));
        ui_degree_inputs.Add(GameObject.Find("Z_Slider"));

        ui_color_sliders = new List<GameObject>();
        ui_color_sliders.Add(GameObject.Find("R_Slider"));
        ui_color_sliders.Add(GameObject.Find("G_Slider"));
        ui_color_sliders.Add(GameObject.Find("B_Slider"));



        currentTool = AnnotationsTools.None;
        canvas = FindObjectOfType<Canvas>();
        ui_slider = GameObject.Find("WidthSlider").GetComponent<Slider>();
        drawPoints = new List<Vector3>();
        drawings = new List<GameObject>();
        camera = GetComponent<Camera>();
        ui_slider.GetComponentInChildren<Text>().text = ui_slider.value + "";
        lineWidth = ui_slider.value;
        ColorChanged();
        ui_slider.gameObject.SetActive(false);
        DeactivateCSs();
        DeactivateRSs();
        ui_slider.gameObject.SetActive(true);
        currentTool = AnnotationsTools.None;
        canvas.GetComponentInChildren<RawImage>().texture = noneIcon;
        GameObject.Find("ExpertHandler").GetComponent<ExpertConnectHandler>().HandleLeaving();
    }

    public void DeactivateRSs()
    {

        foreach (GameObject item in ui_degree_inputs)
        {
            item.SetActive(false);
        }
    }
    public void DeactivateCSs()
    {
        foreach (GameObject item in ui_color_sliders)
        {
            item.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            lockDepth = !lockDepth;
            print("locked " + lockDepth);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            deselectAll();
            DeactivateRSs();
            DeactivateCSs();

            ui_slider.gameObject.SetActive(true);
            currentTool = AnnotationsTools.None;
            canvas.GetComponentInChildren<RawImage>().texture = noneIcon;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            deselectAll();
            sliding = false;
            currentTool = AnnotationsTools.Pencil;
            DeactivateRSs();
            ui_slider.gameObject.SetActive(true);
            foreach (GameObject item in ui_color_sliders)
            {
                item.SetActive(true);
            }
            canvas.GetComponentInChildren<RawImage>().texture = pencilIcon;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            deselectAll();
            ui_slider.gameObject.SetActive(false);
            DeactivateRSs();
            DeactivateCSs();
            currentTool = AnnotationsTools.Dropper;
            canvas.GetComponentInChildren<RawImage>().texture = dropperIcon;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            ui_slider.gameObject.SetActive(false);
            currentTool = AnnotationsTools.Hand;
            DeactivateCSs();
            foreach (GameObject item in ui_degree_inputs)
            {
                item.SetActive(true);
            }
            canvas.GetComponentInChildren<RawImage>().texture = handIcon;
        }

        if(currentTool == AnnotationsTools.Dropper)
        {
            if(Input.GetMouseButtonDown(0))
            {
                pivotPointScreen = Input.mousePosition;
                var ray = camera.ScreenPointToRay(pivotPointScreen);
                if (Physics.Raycast(ray, out hit))
                {
                    //If it hits something, then we will be using this as the pivot point
                    pivotPointWorld = hit.point;
                    print(pivotPointWorld + " is new pivot");
                }
                else
                {
                    print("old pivot is " + pivotPointWorld);
                }
            }
           
        }else if(currentTool == AnnotationsTools.Pencil && !sliding)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!lockDepth)
                {
                    pivotPointScreen = Input.mousePosition;
                    var ray = camera.ScreenPointToRay(pivotPointScreen);
                    if (Physics.Raycast(ray, out hit))
                    {
                        pivotPointWorld = hit.point;
                        print(hit.collider.name + " " + hit.point);
                        CreateLine(hit.point);
                    }
                    else
                    {
                        CreateLine(GetMouseCameraPos());
                    }
                }else
                {
                    CreateLine(GetMouseCameraPos());
                }
            }
            else if(Input.GetMouseButtonUp(0))
            {
                var mesh = new Mesh();
                drawings[drawings.Count - 1].GetComponent<LineRenderer>().BakeMesh(mesh, true);
                drawings[drawings.Count - 1].GetComponent<MeshCollider>().sharedMesh = mesh;
                drawings[drawings.Count - 1].AddComponent<Manipulator>();
                var outpos = new Vector3[drawings[drawings.Count - 1].GetComponent<LineRenderer>().positionCount];
                var cam = Camera.main.transform.position;
                //string topLine = (cam.x - model.transform.position.x) + "_" + (cam.y - model.transform.position.y) + "_" + (cam.z - model.transform.position.z) + " " + model.transform.parent.rotation.eulerAngles.x+"_" + model.transform.parent.rotation.eulerAngles.y + "_" + model.transform.parent.rotation.eulerAngles.z;
                string topLine = (drawings[drawings.Count - 1].transform.localPosition.x) + "_" + (drawings[drawings.Count - 1].transform.localPosition.y) + "_" + (drawings[drawings.Count - 1].transform.localPosition.z) + " " + model.transform.rotation.eulerAngles.x+"_" + model.transform.rotation.eulerAngles.y + "_" + model.transform.rotation.eulerAngles.z;
                if (fileContent.Count > 0)
                {
                    fileContent.RemoveAt(0);
                    fileContent.Insert(0, topLine);
                }
                else
                {
                    fileContent.Insert(0, topLine);
                }
                string line = lineMaterial.color.r + "_" + lineMaterial.color.g + "_" + lineMaterial.color.b + "_" + lineWidth+ " ";
                line += drawings[drawings.Count - 1].name;
                drawings[drawings.Count - 1].GetComponent<LineRenderer>().GetPositions(outpos);
                foreach(Vector3 v in outpos)
                {
                   //line += (v.x - drawings[drawings.Count - 1].transform.localPosition.x) + "_" + (v.y-drawings[drawings.Count - 1].transform.localPosition.y) + "_" + ( v.z- drawings[drawings.Count - 1].transform.localPosition.z) + " ";
                   line += ( v.x) + "_" + (v.y) + "_" + ( v.z) + " ";
                }
                fileContent.Add(line);
                UpdateTextFile();
            }
            if (Input.GetMouseButton(0))
            {
                var ray = GetMouseCameraPos();
                if (Vector3.Distance(ray, drawPoints[drawPoints.Count - 1]) >= .1f)
                {
                    UpdateLine(ray);
                }
            }            
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Z) && drawings.Count > 0)
        {
            var toDestroy = drawings[drawings.Count - 1];
            fileContent.RemoveAt(fileContent.Count - 1);
            drawings.RemoveAt(drawings.Count - 1);
            Destroy(toDestroy);
        }
    }

    public void rotateChanged()
    {
        foreach (var item in ui_degree_inputs)
        {
            Text val = item.transform.Find("SliderValue").GetComponent<Text>();
            val.text = item.GetComponent<Slider>().value.ToString();
        }
        Manipulator[] ms = FindObjectsOfType<Manipulator>();
        for (int i = 0; i < ms.Length; i++)
        {
            ms[i].RotationFieldChanged();
        }
    }

    public void ColorChanged()
    {
        foreach (var item in ui_color_sliders)
        {
            Text val = item.transform.Find("SliderValue").GetComponent<Text>();
            val.text = item.GetComponent<Slider>().value.ToString();
        }

        lineMaterial.color = new Color(ui_color_sliders[0].GetComponent<Slider>().value / 255f, ui_color_sliders[1].GetComponent<Slider>().value / 255f, ui_color_sliders[2].GetComponent<Slider>().value / 255f);
    }
    public void deselectAll()
    {
        Manipulator[] manipulators = GameObject.FindObjectsOfType<Manipulator>();
        foreach (var item in manipulators)
        {
            item.Deselect();
        }
    }
    public void sliderChanged()
    {
        if(ui_slider == null)
            ui_slider = FindObjectOfType<Slider>();

        float val = ui_slider.value;
        ui_slider.GetComponentInChildren<Text>().text = val.ToString("0.00");
        lineWidth = val;
    }

    public void startSliding()
    {
        sliding = true;
    }
    public void stopSliding()
    {
        sliding = false;
    }


    private void CreateLine(Vector3 lineSP)
    {
        print(lineSP + " is SP");
        drawPoints.Clear();
        drawPoints.Add(lineSP);
        drawPoints.Add(lineSP);
        var lineGO = new GameObject();
        lineGO.transform.localPosition = Vector3.zero;
        lineGO.transform.SetParent(model.transform);
        lineGO.name = "Line" + drawings.Count + " ";
        var lineRenderer = lineGO.AddComponent<LineRenderer>();
        var meshCollider = lineGO.AddComponent<MeshCollider>();
        lineRenderer.material = lineMaterial;
        lineRenderer.useWorldSpace = false;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.SetPosition(0,drawPoints[0]);
        lineRenderer.SetPosition(1,drawPoints[1]);
        drawings.Add(lineGO);
    }

    private void UpdateTextFile()
    {
        print("Updating file!");
        File.WriteAllLines(path, fileContent.ToArray());
        StartCoroutine(SendTextFile());  
    }

    private IEnumerator SendTextFile()
    {
        WWWForm form = new WWWForm();
        string channelName = PlayerPrefs.GetString("ChannelName");
        form.AddBinaryData("file", File.ReadAllBytes(path));
        form.AddField("fileName", channelName + "-lines.txt");
        UnityWebRequest request = UnityWebRequest.Post("http://172.105.247.10:8080/uploadFile", form);
        yield return request.SendWebRequest();
        setModifications();
    }

    void setModifications()
    {
        StartCoroutine(setStatusForServer());
    }

    IEnumerator setStatusForServer()
    {
        WWWForm form = new WWWForm();
        form.AddField("channelName", PlayerPrefs.GetString("ChannelName"));
        UnityWebRequest request = UnityWebRequest.Post("http://172.105.247.10:8080/setStatus", form);
        yield return request.SendWebRequest();
    }

    private void UpdateLine(Vector3 pos)
    {
        drawings[drawings.Count - 1].GetComponent<LineRenderer>().positionCount++;
        drawings[drawings.Count - 1].GetComponent<LineRenderer>().SetPosition(drawings[drawings.Count - 1].GetComponent<LineRenderer>().positionCount - 1 ,pos);
    }
    private Vector3 GetMouseCameraPos()
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                return hit.point;
            }
        return ray.origin + ray.direction * Vector3.Distance(pivotPointWorld ,  camera.transform.position);
    }

}
