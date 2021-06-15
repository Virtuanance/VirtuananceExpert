using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTool : MonoBehaviour
{
    public Transform parent;
    public Vector3 initialPos;
    public Vector3 mOffset;
    Quaternion rotation;
    void Awake()
    {
        rotation = transform.rotation;
    }
    private void Start()
    {
        parent = transform.parent.parent;
    }

    void LateUpdate()
    {
        transform.rotation = rotation;
    }

    public void MoveOnY()
    {
        parent.position = new Vector3(initialPos.x, GetMouseAsWorldPoint().y + mOffset.y, initialPos.z);
    }
    public void MoveOnX()
    {
        parent.position = new Vector3(GetMouseAsWorldPoint().x + mOffset.x, initialPos.y, initialPos.z);
    }
    public void MoveOnZ()
    {
        parent.position = new Vector3(initialPos.x, initialPos.y, GetMouseAsWorldPoint().z + mOffset.z);
    }


    public void MoveOnXY()
    {
        parent.position = new Vector3(GetMouseAsWorldPoint().x + mOffset.x, GetMouseAsWorldPoint().y + mOffset.y, initialPos.z);
    }
    public void MoveOnXZ()
    {
        parent.position = new Vector3(GetMouseAsWorldPoint().x + mOffset.x, initialPos.y, GetMouseAsWorldPoint().z + mOffset.z);
    }  
    public void MoveOnYZ()
    {
        parent.position = new Vector3(initialPos.x, GetMouseAsWorldPoint().y + mOffset.y, GetMouseAsWorldPoint().z + mOffset.z);
    }
    public Vector3 GetMouseAsWorldPoint()

    {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;



        // z coordinate of game object on screen

        mousePoint.z = Camera.main.WorldToScreenPoint(

                 parent.position).z;

        Vector3 ret = Camera.main.ScreenToWorldPoint(mousePoint);

        // Convert it to world points

        return ret;
    }
}
