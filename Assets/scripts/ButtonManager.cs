using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private string level;
    public void MoveScene(string level)
    {
        if(level.Equals("Exit"))
        {
            #if UNITY_EDITOR
                     UnityEditor.EditorApplication.isPlaying = false;
            #else
                     Application.Quit();
            #endif
        }
        else {
            SceneManager.LoadScene(level);
        }
    }
}
