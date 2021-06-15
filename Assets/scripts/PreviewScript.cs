using System.Collections;
using System.IO;
using UnityEngine;
using Dummiesman;
using UnityEditor;
using UnityEngine.UI;

public class PreviewScript : MonoBehaviour
{
    public Camera cam; 
    private int previewIndex = 0;
    private GameObject obj, marker;
    public Material markerMaterial;
    private string path;
    // Start is called before the first frame update
    void Start()
    {
        path = Application.dataPath + "/MarkerData.txt";
        previewIndex = this.GetComponent<SceneParams>().getPreviewIndex();
        print("pi=>"+previewIndex);
        string markerPath = Application.dataPath + "\\StreamingAssets\\Vuforia\\single\\markers\\" + previewIndex + ".jpg";
        File.Copy(markerPath,Application.dataPath + "\\Resources\\Textures\\" + previewIndex + ".jpg", true);
        AssetDatabase.Refresh();
        string objectPath = Application.dataPath + "\\StreamingAssets\\Vuforia\\single\\models\\" + previewIndex + ".obj";
        marker = createTexture();
        obj = new OBJLoader().Load(objectPath);
        foreach (Transform t in obj.transform)
        {
            var mm = new MarkerManipulator();
            var mn = new MeshCollider();
            t.gameObject.tag = "Model";
            if (!t.gameObject.TryGetComponent<MarkerManipulator>(out mm))
            {
                t.gameObject.AddComponent<MarkerManipulator>();
            }
            if (!t.gameObject.TryGetComponent<MeshCollider>(out mn))
            {
                t.gameObject.AddComponent<MeshCollider>();
            }

        }
        //Bounds b = GetRealSize(obj);
        //MeshCollider col = obj.AddComponent<MeshCollider>();
        //col.size = new Vector3(b.size.x, b.size.y, b.size.z);
        obj.transform.position = marker.transform.position + new Vector3(0,0.1f,0);
        //obj.transform.localScale = new Vector3(50 / b.size.x, 100 / b.size.y, 100 / b.size.z);
        //obj.AddComponent<ObjectRotate>();
    }


    void loadLocalImage(string path)
    {
        WWW www = new WWW(path);
        GameObject image = GameObject.Find("Image");
        image.GetComponent<RawImage>().texture = www.texture;
    }

    public static Bounds GetRealSize(GameObject parent)
    {
        MeshFilter[] childrens = parent.GetComponentsInChildren<MeshFilter>();


        Vector3 minV = childrens[0].transform.position - MultVect(childrens[0].mesh.bounds.size, childrens[0].transform.localScale) / 2;
        Vector3 maxV = childrens[0].transform.position + MultVect(childrens[0].mesh.bounds.size, childrens[0].transform.localScale) / 2;

        for (int i = 1; i < childrens.Length; i++)
        {
            maxV = Vector3.Max(maxV, childrens[i].transform.position + MultVect(childrens[i].mesh.bounds.size, childrens[i].transform.localScale) / 2);
            minV = Vector3.Min(minV, childrens[i].transform.position - MultVect(childrens[i].mesh.bounds.size, childrens[i].transform.localScale) / 2);

        }
        Vector3 v3 = maxV - minV;
        return new Bounds(minV + v3 / 2, v3);
    }

    private static Vector3 MultVect(Vector3 a, Vector3 b)
    {
        a.x *= b.x;
        a.y *= b.y;
        a.z *= b.z;
        return a;
    }


    public void SavePreview()
    {
        print("Saving");
        Transform t = obj.transform;
        Vector3 offset = marker.transform.position - t.position;
        string newText = "";
        newText += previewIndex + " " + offset.x + "_" + offset.y + "_" + offset.z + " " + t.localScale.x + "_" + t.localScale.y + "_" + t.localScale.z + " " + t.rotation.eulerAngles.x + "_" + t.rotation.eulerAngles.y + "_" + t.rotation.eulerAngles.z + "\n";

        if (File.Exists(path))
        {
            string[] text = File.ReadAllLines(path);
            for (int j = 0; j < text.Length; j++)
            {
                string[] line = text[j].Split(' ');
                if (previewIndex != int.Parse(line[0]))
                    newText += text[j] + "\n";
            }
        }
        File.WriteAllText(path, newText);
    }


    private GameObject createTexture()
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);
        Texture t = Resources.Load<Texture>("Textures/"+previewIndex +"");
        go.GetComponent<MeshRenderer>().material = markerMaterial;
        go.GetComponent<MeshRenderer>().material.mainTexture = t;
        //go.AddComponent<MarkerManipulator>();
        //go.tag = "Model";
        // to be renderered onto
        // make the object draggable

        // set up transform
        go.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 45f + Camera.main.transform.right * -10;
        //go.transform.localScale = Vector3.one;

        return go;

    }

}
