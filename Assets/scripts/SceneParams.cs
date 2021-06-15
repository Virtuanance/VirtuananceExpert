using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneParams : MonoBehaviour
{
    // Start is called before the first frame update
    public static int previewIndex = 0;
    public static string id = "";
    public void setPreviewIndex(int newIndex)
    {
        previewIndex = newIndex;
    }

    public int getPreviewIndex()
    {
        return previewIndex;
    }

    public void setId(string newId)
    {
        id = newId;
    }

    public string getId()
    {
        return id;
    }


}
