using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    float rotSpeed = 5;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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

    }

    private void OnMouseDrag()
    {
        float rotx = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        float roty = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;
        transform.RotateAround(Vector3.up, -rotx);
        transform.RotateAround(Vector3.right, roty);
    }



}
