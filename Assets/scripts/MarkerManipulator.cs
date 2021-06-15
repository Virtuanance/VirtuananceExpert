using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerManipulator : MonoBehaviour
{
    private Material mat;
    private Color originalColor;
    int i = 0;
    GameObject moveIcon;
    GameObject instantiatedIcon;
    public bool selected = false;
    private List<GameObject> ui_degree_inputs;
    CameraMovementPreview cm;
    // Start is called before the first frame update
    void Start()
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

        if (this.TryGetComponent<MeshRenderer>(out var renderer))
        {
            mat = renderer.material;
            originalColor = mat.color;
                }

        cm = Camera.main.gameObject.GetComponent<CameraMovementPreview>();

    }

    // Update is called once per frame
    void Update()
    {
        if (selected && Input.GetKeyDown(KeyCode.Escape))
            Deselect();

        if (selected)
        {
            transform.parent.transform.position = transform.position;
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                transform.parent.transform.localScale = new Vector3(transform.localScale.x + 0.05f, transform.localScale.y + 0.05f, transform.localScale.z + 0.05f);
                i++;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                transform.parent.transform.localScale = new Vector3(transform.localScale.x - 0.05f, transform.localScale.y - 0.05f, transform.localScale.z - 0.05f);
                i--;
            }

        }
    }

    private void OnMouseDown()
    {
        if(cm.smthSelected)
        {
            cm.deselectAll();
            print("smth was selected");
        }
        selected = true;
        cm.smthSelected = true;
        if (mat != null)
        {
            originalColor = mat.color;
            mat.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.45f);
        }
        if (instantiatedIcon == null)
        {
            instantiatedIcon = Instantiate<GameObject>(moveIcon);
            instantiatedIcon.transform.parent = this.gameObject.transform;
            instantiatedIcon.transform.position = this.gameObject.transform.position;
            instantiatedIcon.transform.localScale *= 0.2f;
        }
        GetComponent<MeshCollider>().enabled = false;
        Vector3 initAngles = transform.eulerAngles;

        foreach (GameObject item in ui_degree_inputs)
        {
            item.SetActive(true);
        }

        foreach (GameObject item in ui_degree_inputs)
        {
            if (item.name.Contains("X"))
            {
                item.GetComponent<Slider>().value = initAngles.x;
                //item.transform.Find("SliderValue").GetComponent<Text>().text = item.GetComponent<Slider>().value.ToString();
            }
            else if (item.name.Contains("Y"))
            {
                item.GetComponent<Slider>().value = initAngles.y;
                // item.transform.Find("SliderValue").GetComponent<Text>().text = item.GetComponent<Slider>().value.ToString();

            }
            else if (item.name.Contains("Z"))
            {
                item.GetComponent<Slider>().value = initAngles.z;
                //item.transform.Find("SliderValue").GetComponent<Text>().text = item.GetComponent<Slider>().value.ToString();

            }
        }

    }
    public void RotationFieldChanged()
    {
        if (selected)
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

    public void Deselect()
    {
        if (mat != null)
        {
            mat.color = originalColor;
        }
        if (selected)
        {
            print("deselecting "+this.name);
             if (instantiatedIcon != null)
            {
                Destroy(instantiatedIcon);
            }
            GetComponent<MeshCollider>().enabled = true;
            selected = false;
        }

    }

}
