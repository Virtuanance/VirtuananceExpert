using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AnnotationsTechnician : MonoBehaviour
{
    // Start is called before the first frame update
    private string[] lineList;
    public Material lineMaterial;
    List<GameObject> drawings;
    public string channelName;
    void Start()
    {
        drawings = new List<GameObject>();
        InvokeRepeating("CallEverySecond", 1f, 1f);
    }

    private IEnumerator getAnnotations()
    {

        WWW www = new WWW("http://172.105.247.10:8080/downloadFile/" + channelName + "-lines.txt");

        yield return www;
        if (www.text.Substring(2, 4) == "time")
        {
            Debug.LogWarning("No line file uploaded for this channel!");
        }
        else
        {
            lineList = www.text.Split('\n');
            drawings.Clear();
            foreach (string line in lineList)
            {
                if (line != "")
                {
                    float r, g, b, w;
                    string[] elements = line.Split(' ');
                    Vector3[] pointList = new Vector3[elements.Length - 1];
                    for (int i = 0; i < elements.Length-1; i++)
                    {
                        string[] element = elements[i].Split('_');
                        if (element.Length == 3)
                        {
                            element[0] = element[0].Replace(',', '.');
                            element[1] = element[1].Replace(',', '.');
                            element[2] = element[2].Replace(',', '.');
                        }
                        else if (element.Length == 4)
                        {
                            element[0] = element[0].Replace(',', '.');
                            element[1] = element[1].Replace(',', '.');
                            element[2] = element[2].Replace(',', '.');
                            element[3] = element[3].Replace(',', '.');
                        }
                        if (i == 0)
                        {
                            r = float.Parse(element[0]);
                            g = float.Parse(element[1]);
                            b = float.Parse(element[2]);
                            w = float.Parse(element[3]);
                            print(r);
                            print(g);
                            print(b);
                            print(w);
                            var lineGO = new GameObject();
                            lineGO.name = "Line " + drawings.Count;
                            var lineRenderer = lineGO.AddComponent<LineRenderer>();
                            lineMaterial.color = new Color(r, g, b, 1);
                            lineRenderer.material = lineMaterial;
                            lineRenderer.useWorldSpace = false;
                            lineRenderer.endWidth = w;
                            lineRenderer.startWidth = w;
                            drawings.Add(lineGO);
                        }
                        else if (element.Length > 1)
                        {
                            print(element[0]);
                            print(element[1]);
                            print(element[2]);
                            
                            pointList[i - 1] = new Vector3(float.Parse(element[0]), float.Parse(element[1]), float.Parse(element[2]));
                            print("POINT LIST! ");
                            print(pointList[i - 1]);
                        }
                        
                    }
                    drawings[drawings.Count - 1].GetComponent<LineRenderer>().positionCount = pointList.Length;
                    drawings[drawings.Count - 1].GetComponent<LineRenderer>().SetPositions(pointList);
                    GameObject parent = GameObject.Find(elements[elements.Length - 1]);
                    drawings[drawings.Count - 1].transform.SetParent(parent.transform);

                }
            }

         


            

        }

    }

    // Update is called once per frame
    void CallEverySecond()
    {
       StartCoroutine(checkForUpdate());
    }


    IEnumerator checkForUpdate()
    {
        WWWForm form = new WWWForm();
        form.AddField("channelName", channelName);
        UnityWebRequest request = UnityWebRequest.Post("http://172.105.247.10:8080/getStatus", form);
        yield return request.SendWebRequest();
        var response = request.downloadHandler.text;
        if (response.CompareTo("Modification Available!") == 0)
        {
            StartCoroutine(getAnnotations());
        }   
    }
}
