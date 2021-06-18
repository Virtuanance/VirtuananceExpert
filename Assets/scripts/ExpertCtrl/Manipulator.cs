using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Manipulator : MonoBehaviour
{
    public Shader outline;
    public Annotations annotations;
    private Material mat;
    private Color originalColor;
    private RaycastHit hit;
    [SerializeField]
    GameObject moveIcon;
    GameObject instantiatedIcon;
    CameraMovement cm;
    private List<GameObject> ui_degree_inputs;
    int i = 0;
    private Vector3 mOffset;
    public bool selected = false;

    private float mZCoord;



    // Start is called before the first frame update
    private void Start()
    {
        init();
    }

    public void init()
    {
        moveIcon = (GameObject)Resources.Load("MoveTool");
        ui_degree_inputs = new List<GameObject>();
        var fooGroup = Resources.FindObjectsOfTypeAll<Slider>();
        if (fooGroup.Length > 0)
        {
            foreach (var item in fooGroup)
            {
                if (item.gameObject.name.Equals("X_Slider"))
                {
                    ui_degree_inputs.Add(item.gameObject);
                }
                else if (item.gameObject.name.Equals("Y_Slider"))
                {
                    ui_degree_inputs.Add(item.gameObject);
                }
                else if (item.gameObject.name.Equals("Z_Slider"))
                {
                    ui_degree_inputs.Add(item.gameObject);
                }
            }
        }

        cm = FindObjectOfType<CameraMovement>();
        annotations = FindObjectOfType<Annotations>();
        if (this.TryGetComponent<MeshRenderer>(out var renderer))
            mat = renderer.material;
        else if (this.TryGetComponent<LineRenderer>(out var lrenderer))
            mat = lrenderer.material;

        mat.SetColor("_EmissionColor", new Color(0, 0.7f, 0, 1));

    }

    void OnMouseDown()
    {
        if (annotations.currentTool == AnnotationsTools.Hand)
        {
            annotations.deselectAll();
            selected = true;
            cm.smthSelected = true;
            if (mat != null)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                originalColor = mat.color;
                mat.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.15f);
            }
            if (instantiatedIcon == null)
            {
                instantiatedIcon = Instantiate<GameObject>(moveIcon);
                instantiatedIcon.transform.parent = this.gameObject.transform;
                instantiatedIcon.transform.position = this.gameObject.transform.position;
                instantiatedIcon.transform.localScale *= 0.07f;
            }
            GetComponent<MeshCollider>().enabled = false;
            Vector3 initAngles = transform.eulerAngles;
            foreach (GameObject item in ui_degree_inputs)
            {
                if (item.name.Contains("X"))
                {
                    print("X " + initAngles.x);
                    item.GetComponent<Slider>().value = initAngles.x;
                    //item.transform.Find("SliderValue").GetComponent<Text>().text = item.GetComponent<Slider>().value.ToString();
                }  
                else if (item.name.Contains("Y"))
                {
                    print("Y " + initAngles.y);
                    item.GetComponent<Slider>().value = initAngles.y;
                   // item.transform.Find("SliderValue").GetComponent<Text>().text = item.GetComponent<Slider>().value.ToString();

                }  
                else if (item.name.Contains("Z"))
                {
                    print("Z " + initAngles.z);
                    item.GetComponent<Slider>().value = initAngles.z;
                    //item.transform.Find("SliderValue").GetComponent<Text>().text = item.GetComponent<Slider>().value.ToString();

                }

            }
        }


    }

    public void Deselect()
    {
        if (selected)
        {
            if (mat != null)
            {
                mat.SetOverrideTag("RenderType", "");
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                mat.SetInt("_ZWrite", 1);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.DisableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = -1;
                mat.DisableKeyword("_EMISSION");
                mat.color = originalColor;
            }
            if (instantiatedIcon != null)
            {
                Destroy(instantiatedIcon);
            }
            GetComponent<MeshCollider>().enabled = true;
            selected = false;
        }
       
    }
  


    // Update is called once per frame
    void Update()
    {
        if (selected && Input.GetKeyDown(KeyCode.Backspace))
            Deselect();

        if (selected)
        {
            
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && i < 10) // forward
            {
                transform.localScale = new Vector3(transform.localScale.x + 0.05f, transform.localScale.y + 0.05f, transform.localScale.z + 0.05f);
                i++;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && i > -10) // backwards
            {
                transform.localScale = new Vector3(transform.localScale.x - 0.05f, transform.localScale.y - 0.05f, transform.localScale.z - 0.05f);
                i--;
            }
            /*if(Input.GetMouseButton(1))
            {
                transform.Rotate((Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime), (Input.GetAxis("Mouse Y") * -rotSpeed * Time.deltaTime), 0, Space.World);
                ui_degree_inputs[0].value = transform.rotation.eulerAngles.x;
                ui_degree_inputs[1].value = transform.rotation..y;
                ui_degree_inputs[2].value = transform.rotation.z;
            }*/
            
        }


    }

    public void RotationFieldChanged()
    {
        if(selected)
        {
            Vector3 setToEA = Vector3.zero;
            foreach (GameObject item in ui_degree_inputs)
            {
                if (item.name.Contains("X"))
                {
                    setToEA.x = item.GetComponent<Slider>().value;
                }
                else if (item.name.Contains("Y"))
                {
                    setToEA.y = item.GetComponent<Slider>().value;
                }
                else if (item.name.Contains("Z"))
                {
                    setToEA.z = item.GetComponent<Slider>().value;
                }
            }
            transform.parent.transform.eulerAngles = setToEA;
        }
    }
    private Vector3 GetMouseAsWorldPoint()

    {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;



        // z coordinate of game object on screen

        mousePoint.z = mZCoord;



        Vector3 ret = Camera.main.ScreenToWorldPoint(mousePoint);

        // Convert it to world points

        // Convert it to world points

        return ret;

    }



}
