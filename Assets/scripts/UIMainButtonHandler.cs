using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainButtonHandler : MonoBehaviour
{

    public Button mainButton1;
    public Button mainButton2;
    public Button mainButton3;

    public void button1click()
    {
        StartCoroutine(fadeButton(mainButton1, false, 1f, ""));
        StartCoroutine(fadeButton(mainButton2, false, 1f, ""));
        StartCoroutine(fadeButton(mainButton3, false, 1f, "UIImporter"));

    }
    public void button2click()
    {
        StartCoroutine(fadeButton(mainButton1, false, 1f, ""));
        StartCoroutine(fadeButton(mainButton2, false, 1f, ""));
        StartCoroutine(fadeButton(mainButton3, false, 1f, "UIImported"));
    }
    public void button3click()
    {
        StartCoroutine(fadeButton(mainButton1, false, 1f, ""));
        StartCoroutine(fadeButton(mainButton2, false, 1f, ""));
        StartCoroutine(fadeButton(mainButton3, false, 1f, "MainMenu"));
    }

    IEnumerator fadeButton(Button button, bool fadeIn, float duration, string targetScene)
    {

        float counter = 0f;

        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0;
            b = 1;
        }
        else
        {
            a = 1;
            b = 0;
        }

        Image buttonImage = button.GetComponent<Image>();
        Text buttonText = button.GetComponentInChildren<Text>();

        //Enable both Button, Image and Text components
        if (!button.enabled)
            button.enabled = true;

        if (!buttonImage.enabled)
            buttonImage.enabled = true;

        if (!buttonText.enabled)
            buttonText.enabled = true;

        //For Button None or ColorTint mode
        Color buttonColor = buttonImage.color;
        Color textColor = buttonText.color;

        //For Button SpriteSwap mode
        ColorBlock colorBlock = button.colors;


        //Do the actual fading
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);
            //Debug.Log(alpha);

            if (button.transition == Selectable.Transition.None || button.transition == Selectable.Transition.ColorTint)
            {
                buttonImage.color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, alpha);//Fade Traget Image
                buttonText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);//Fade Text
            }
            else if (button.transition == Selectable.Transition.SpriteSwap)
            {
                ////Fade All Transition Images
                colorBlock.normalColor = new Color(colorBlock.normalColor.r, colorBlock.normalColor.g, colorBlock.normalColor.b, alpha);
                colorBlock.pressedColor = new Color(colorBlock.pressedColor.r, colorBlock.pressedColor.g, colorBlock.pressedColor.b, alpha);
                colorBlock.highlightedColor = new Color(colorBlock.highlightedColor.r, colorBlock.highlightedColor.g, colorBlock.highlightedColor.b, alpha);
                colorBlock.disabledColor = new Color(colorBlock.disabledColor.r, colorBlock.disabledColor.g, colorBlock.disabledColor.b, alpha);

                button.colors = colorBlock; //Assign the colors back to the Button
                buttonImage.color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, alpha);//Fade Traget Image
                buttonText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);//Fade Text
            }
            else
            {
                Debug.LogError("Button Transition Type not Supported");
            }

            yield return null;
        }

        if (!fadeIn)
        {
            //Disable both Button, Image and Text components
            buttonImage.enabled = false;
            buttonText.enabled = false;
            button.enabled = false;
        }

        if (targetScene.CompareTo("") != 0)
        {
            if (targetScene.CompareTo("exit") == 0) Application.Quit();
            else SceneManager.LoadScene(targetScene);
        }
    }

}
