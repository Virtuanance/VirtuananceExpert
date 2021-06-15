using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
 
    public float mainSpeed = 30.0f;
    public float shiftAdd = 50.0f;
    public float maxShift = 100f;
    public float degrees = 10f;
    public float angularSpeed = 1.0f;
    public float rotationSpeed = 2.0f;

    public bool smthSelected = false;

    float pitch;
    float yaw;

    public Camera targetCam;
    private Vector3 pivotPointScreen;
    private Vector3 pivotPointWorld;
    private Ray ray;
    private Annotations annotations;
    private RaycastHit hit;
    private Quaternion initialRot;

    // Use this for initialization
    void Start()
    {
        //Changed
        annotations = GetComponent<Annotations>();
        //Set the targetCam here if not assigned from inspector
        if (targetCam == null)
            targetCam = Camera.main;

        initialRot = targetCam.transform.localRotation;
    }
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.R))
        {
            targetCam.transform.rotation = initialRot;
        }
        if (annotations.currentTool == AnnotationsTools.Hand || annotations.currentTool == AnnotationsTools.None)
        {
            
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

                //Take the mouse position
                pivotPointScreen = Input.mousePosition;
                //Convert mouse position to ray
                // Then raycast using this ray to check if it hits something in the scene
                //**Converting the mousepositin to world position directly will
                //**return constant value of the camera position as the Z value is always 0
                ray = targetCam.ScreenPointToRay(pivotPointScreen);
                if (Physics.Raycast(ray, out hit))
                {
                    //If it hits something, then we will be using this as the pivot point
                    pivotPointWorld = hit.point;
                }
                //targetCam.transform.LookAt (pivotPointWorld);
                Cursor.visible = false;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                Cursor.visible = true;
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
                p = p * mainSpeed ;
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
}
