using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdText : MonoBehaviour
{

    public Text txt;
    // Start is called before the first frame update
    void Start()
    {
        txt.text =  txt.GetComponent<SceneParams>().getId();
    }


}
