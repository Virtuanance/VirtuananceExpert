using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using System.IO;

public class SpawnObjects : MonoBehaviour
{

    Vector3 camPos;
    // Start is called before the first frame update
    void Start()
    {
        camPos = Camera.main.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick()
    {
        StartCoroutine(WhenClicked());
    }

    private IEnumerator WhenClicked()
    {
        string channelName = PlayerPrefs.GetString("ChannelName");
        print("cname = " + channelName);
        WWW www = new WWW("http://172.105.247.10:8080/downloadFile/"+ channelName + ".obj");

        yield return www;
        bool success = true;
        if (www.text.Substring(2, 4) == "time")
        {
            success = false;
        }

        if (success)
        {
            File.WriteAllBytes(channelName + ".obj", www.bytes);
            GameObject obj = new OBJLoader().Load(channelName + ".obj");
            obj.tag = "Model";
            obj.transform.localScale = obj.transform.localScale / 10;
            obj.transform.GetChild(0).gameObject.AddComponent<Manipulator>();
            obj.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
            obj.transform.position = new Vector3(camPos.x - 4,camPos.y , camPos.z + 5) ;
            GetComponent<Annotations>().model = obj;
            obj.transform.GetChild(0).gameObject.GetComponent<Manipulator>().init();
        }

        else
        {
            Debug.Log("No such object found!");
        }
    }
}
