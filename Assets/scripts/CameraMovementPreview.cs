using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.XR;
public class CameraMovementPreview : MonoBehaviour
{

    public float mainSpeed = 30.0f;
    public float shiftAdd = 50.0f;
    public float maxShift = 100f;
    public float degrees = 10f;
    public float angularSpeed = 1.0f;
    public float rotationSpeed = 2.0f;
    private List<GameObject> ui_degree_inputs;

    public bool sliding = false;
    public bool smthSelected = false;

    float pitch = -90f;
    float yaw;

    public Camera targetCam;
    private Quaternion initialRot;
    // Start is called before the first frame update
    void Start()
    {
        DisableVR();
        ui_degree_inputs = new List<GameObject>();
        ui_degree_inputs.Add(GameObject.Find("X_Slider"));
        ui_degree_inputs.Add(GameObject.Find("Y_Slider"));
        ui_degree_inputs.Add(GameObject.Find("Z_Slider"));
        if (targetCam == null)
            targetCam = Camera.main;

        DeactivateRSs();

        initialRot = targetCam.transform.localRotation;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            targetCam.transform.rotation = initialRot;
        }

        if (Input.GetMouseButton(1))
        {
            pitch += rotationSpeed * Input.GetAxis("Mouse Y");
            yaw += rotationSpeed * Input.GetAxis("Mouse X");

            // Clamp pitch:
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            // Wrap yaw:
            while (yaw < 0f)
            {
                yaw += 360f;
            }
            while (yaw >= 360f)
            {
                yaw -= 360f;
            }
            float z = targetCam.transform.eulerAngles.z;
            targetCam.transform.eulerAngles = new Vector3(-pitch, yaw, z);
        }

        if (Input.GetMouseButtonDown(1))
        {
            //targetCam.transform.LookAt (pivotPointWorld);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        //Keyboard commands
        float f = 0.0f;
        Vector3 p = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            p = p * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            p = p * mainSpeed;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            targetCam.transform.RotateAround(Vector3.forward, rotationSpeed * 0.01f);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            targetCam.transform.RotateAround(Vector3.forward, rotationSpeed * -0.01f);
        }


        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.Space))
        { //If player wants to move on X and Z axis only
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            transform.Translate(p);
        }

    }
    IEnumerator LoadDevice(string newDevice, bool enable)
    {
        XRSettings.LoadDeviceByName(newDevice);
        yield return null;
        XRSettings.enabled = enable;
    }

    private void DisableVR()
    {
        StartCoroutine(LoadDevice("", false));
    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }


    public void rotateChanged()
    {
        foreach (var item in ui_degree_inputs)
        {
            Text val = item.transform.Find("SliderValue").GetComponent<Text>();
            val.text = item.GetComponent<Slider>().value.ToString();
        }
        MarkerManipulator[] ms = FindObjectsOfType<MarkerManipulator>();
        for (int i = 0; i < ms.Length; i++)
        {
            ms[i].RotationFieldChanged();
        }
    }

    public void startSliding()
    {
        sliding = true;
    }
    public void stopSliding()
    {
        sliding = false;
    }


    public void deselectAll()
    {
        GameObject[] models = GameObject.FindGameObjectsWithTag("Model");
        for (int i = 0; i < models.Length; i++)
        {
            MarkerManipulator a = models[i].GetComponentInChildren<MarkerManipulator>();
            a.Deselect();
        }
    }


    public void DeactivateRSs()
    {

        foreach (GameObject item in ui_degree_inputs)
        {
            item.SetActive(false);
        }
    }
}
