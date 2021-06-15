using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToolXY : MonoBehaviour
{
    private MoveTool parent;

    private void Start()
    {
        parent = transform.parent.GetComponent<MoveTool>();
    }

    private void OnMouseDown()
    {
        parent.initialPos = parent.parent.position;
        parent.mOffset = transform.parent.parent.position - parent.GetMouseAsWorldPoint();
    }

    // Start is called before the first frame update
    private void OnMouseDrag()
    {
        parent.MoveOnXY();
    }
}
