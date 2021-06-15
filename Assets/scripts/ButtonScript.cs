using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ButtonScript : MonoBehaviour
{

	public Button mainButton;
	public Button b1;
	public Button b2;
	public Button b3;

	public Sprite bIconMain;
	public Sprite bIconB1;
	public Sprite bIconB1Toggle;
	public Sprite bIconB2;
	public Sprite bIconB3;

	public Button quitButton;
	public Button yes;
	public Button no;

	private int iteration = 0;
	private int totalRotation = 0;
	private bool toggle = false;
	private bool muted = false;
	private bool quitToggle = false;
	void Start()
	{
		
		mainButton.GetComponent<Button>().onClick.AddListener(mainButtonClick);
		b1.GetComponent<Button>().onClick.AddListener(muteButton);
		b2.GetComponent<Button>().onClick.AddListener(advancedSettingsButton);
		b3.GetComponent<Button>().onClick.AddListener(exitButton);
		yes.GetComponent<Button>().onClick.AddListener(yesButton);
		no.GetComponent<Button>().onClick.AddListener(noButton);

		mainButton.GetComponent<Image>().sprite = bIconMain;
		b1.GetComponent<Image>().sprite = bIconB1;
		b2.GetComponent<Image>().sprite = bIconB2;
		b3.GetComponent<Image>().sprite = bIconB3;

		quitButton.gameObject.SetActive(false);
		quitButton.interactable = false;
		yes.gameObject.SetActive(false);
		no.gameObject.SetActive(false);

		b1.transform.localScale = new Vector3(0, 0, 0);
		b2.transform.localScale = new Vector3(0, 0, 0);
		b3.transform.localScale = new Vector3(0, 0, 0);
		

	}

	void mainButtonClick()
	{
		if ((iteration != 0 && iteration != 31) || (totalRotation != 0 && totalRotation != 10)) return;
        else { 
			toggle = !toggle;
			if (toggle)
			{
				b1.transform.localScale = new Vector3(0, 0, 0);
				b2.transform.localScale = new Vector3(0, 0, 0);
				b3.transform.localScale = new Vector3(0, 0, 0);
			}
        
			StartCoroutine(buttonRotater());
		}
	}

	private IEnumerator buttonRotater()
    {
		yield return new WaitForSeconds(0.015f);
		if (toggle)
        {
			rotateRight();
        }
        else
        {
			rotateLeft();
        }

    }
	
	private void rotateRight()
    {
		totalRotation++;
		if (totalRotation < 10)
        {
			mainButton.transform.eulerAngles = new Vector3(0, 0, -totalRotation * 10);
			StartCoroutine(buttonRotater());
		}
			
        if (totalRotation == 2)
        {
			StartCoroutine(codeWaiter());
		}
		
    }

	private void rotateLeft()
    {
		totalRotation--;
		if (totalRotation > 0)
        {
			mainButton.transform.eulerAngles = new Vector3(0, 0, -totalRotation * 10);
			StartCoroutine(buttonRotater());
		}

		if (totalRotation == 8)
		{
			StartCoroutine(codeWaiter());
		}
		
	}


	private IEnumerator codeWaiter()
    {
		yield return new WaitForSeconds(0.01f);
		if (toggle)
			increaseSize();
        else  
			decreaseSize();
		
	}

	private void increaseSize()
    {
		iteration++;
		if ( iteration > 0 && iteration <= 10)
        {
			if (iteration == 1)
				b1.transform.localScale = new Vector3(0.1f, 0.1f, 0);
			else if (iteration == 2)
				b1.transform.localScale = new Vector3(0.2f, 0.2f, 0);
			else if (iteration == 3)
				b1.transform.localScale = new Vector3(0.3f, 0.3f, 0);
			else if (iteration == 4)
				b1.transform.localScale = new Vector3(0.4f, 0.4f, 0);
			else if (iteration == 5)
				b1.transform.localScale = new Vector3(0.5f, 0.5f, 0);
			else if (iteration == 6)
				b1.transform.localScale = new Vector3(0.6f, 0.6f, 0);
			else if (iteration == 7)
				b1.transform.localScale = new Vector3(0.7f, 0.7f, 0);
			else if (iteration == 8)
				b1.transform.localScale = new Vector3(0.8f, 0.8f, 0);
			else if (iteration == 9)
				b1.transform.localScale = new Vector3(0.9f, 0.9f, 0);
			else if (iteration == 10)
				b1.transform.localScale = new Vector3(1f, 1f, 0);
			StartCoroutine(codeWaiter());
		}
		else if ( iteration > 10 &&iteration <= 20)
		{
			if (iteration == 11)
				b2.transform.localScale = new Vector3(0.1f, 0.1f, 0);
			else if (iteration == 12)
				b2.transform.localScale = new Vector3(0.2f, 0.2f, 0);
			else if (iteration == 13)
				b2.transform.localScale = new Vector3(0.3f, 0.3f, 0);
			else if (iteration == 14)
				b2.transform.localScale = new Vector3(0.4f, 0.4f, 0);
			else if (iteration == 15)
				b2.transform.localScale = new Vector3(0.5f, 0.5f, 0);
			else if (iteration == 16)
				b2.transform.localScale = new Vector3(0.6f, 0.6f, 0);
			else if (iteration == 17)
				b2.transform.localScale = new Vector3(0.7f, 0.7f, 0);
			else if (iteration == 18)
				b2.transform.localScale = new Vector3(0.8f, 0.8f, 0);
			else if (iteration == 19)
				b2.transform.localScale = new Vector3(0.9f, 0.9f, 0);
			else if (iteration == 20)
				b2.transform.localScale = new Vector3(1f, 1f, 0);
			StartCoroutine(codeWaiter());
		}
		else if (iteration > 20 && iteration <= 30)
		{
			if (iteration == 21)
				b3.transform.localScale = new Vector3(0.1f, 0.1f, 0);
			else if (iteration == 22)
				b3.transform.localScale = new Vector3(0.2f, 0.2f, 0);
			else if (iteration == 23)
				b3.transform.localScale = new Vector3(0.3f, 0.3f, 0);
			else if (iteration == 24)
				b3.transform.localScale = new Vector3(0.4f, 0.4f, 0);
			else if (iteration == 25)
				b3.transform.localScale = new Vector3(0.5f, 0.5f, 0);
			else if (iteration == 26)
				b3.transform.localScale = new Vector3(0.6f, 0.6f, 0);
			else if (iteration == 27)
				b3.transform.localScale = new Vector3(0.7f, 0.7f, 0);
			else if (iteration == 28)
				b3.transform.localScale = new Vector3(0.8f, 0.8f, 0);
			else if (iteration == 29)
				b3.transform.localScale = new Vector3(0.9f, 0.9f, 0);
			else if (iteration == 30)
				b3.transform.localScale = new Vector3(1f, 1f, 0);
			StartCoroutine(codeWaiter());
		}

		
	}

	private void decreaseSize()
	{
		iteration--;
		if (iteration > 0 && iteration <= 10)
		{
			if (iteration == 1)
				b1.transform.localScale = new Vector3(0.1f, 0.1f, 0);
			else if (iteration == 2)
				b1.transform.localScale = new Vector3(0.2f, 0.2f, 0);
			else if (iteration == 3)
				b1.transform.localScale = new Vector3(0.3f, 0.3f, 0);
			else if (iteration == 4)
				b1.transform.localScale = new Vector3(0.4f, 0.4f, 0);
			else if (iteration == 5)
				b1.transform.localScale = new Vector3(0.5f, 0.5f, 0);
			else if (iteration == 6)
				b1.transform.localScale = new Vector3(0.6f, 0.6f, 0);
			else if (iteration == 7)
				b1.transform.localScale = new Vector3(0.7f, 0.7f, 0);
			else if (iteration == 8)
				b1.transform.localScale = new Vector3(0.8f, 0.8f, 0);
			else if (iteration == 9)
				b1.transform.localScale = new Vector3(0.9f, 0.9f, 0);
			else if (iteration == 10)
				b1.transform.localScale = new Vector3(1f, 1f, 0);
			StartCoroutine(codeWaiter());
		}
		else if (iteration > 10 && iteration <= 20)
		{
			if (iteration == 11)
				b2.transform.localScale = new Vector3(0.1f, 0.1f, 0);
			else if (iteration == 12)
				b2.transform.localScale = new Vector3(0.2f, 0.2f, 0);
			else if (iteration == 13)
				b2.transform.localScale = new Vector3(0.3f, 0.3f, 0);
			else if (iteration == 14)
				b2.transform.localScale = new Vector3(0.4f, 0.4f, 0);
			else if (iteration == 15)
				b2.transform.localScale = new Vector3(0.5f, 0.5f, 0);
			else if (iteration == 16)
				b2.transform.localScale = new Vector3(0.6f, 0.6f, 0);
			else if (iteration == 17)
				b2.transform.localScale = new Vector3(0.7f, 0.7f, 0);
			else if (iteration == 18)
				b2.transform.localScale = new Vector3(0.8f, 0.8f, 0);
			else if (iteration == 19)
				b2.transform.localScale = new Vector3(0.9f, 0.9f, 0);
			else if (iteration == 20)
				b2.transform.localScale = new Vector3(1f, 1f, 0);
			StartCoroutine(codeWaiter());
		}
		else if (iteration > 20 && iteration <= 30)
		{
			if (iteration == 21)
				b3.transform.localScale = new Vector3(0.1f, 0.1f, 0);
			else if (iteration == 22)
				b3.transform.localScale = new Vector3(0.2f, 0.2f, 0);
			else if (iteration == 23)
				b3.transform.localScale = new Vector3(0.3f, 0.3f, 0);
			else if (iteration == 24)
				b3.transform.localScale = new Vector3(0.4f, 0.4f, 0);
			else if (iteration == 25)
				b3.transform.localScale = new Vector3(0.5f, 0.5f, 0);
			else if (iteration == 26)
				b3.transform.localScale = new Vector3(0.6f, 0.6f, 0);
			else if (iteration == 27)
				b3.transform.localScale = new Vector3(0.7f, 0.7f, 0);
			else if (iteration == 28)
				b3.transform.localScale = new Vector3(0.8f, 0.8f, 0);
			else if (iteration == 29)
				b3.transform.localScale = new Vector3(0.9f, 0.9f, 0);
			else if (iteration == 30)
				b3.transform.localScale = new Vector3(1f, 1f, 0);
			StartCoroutine(codeWaiter());
		}

		if (iteration == 20)
        {
			b3.transform.localScale = new Vector3(0, 0, 0);
        }
		if (iteration == 10)
		{
			b2.transform.localScale = new Vector3(0, 0, 0);
		}
		if (iteration == 0)
		{
			b1.transform.localScale = new Vector3(0, 0, 0);
		}


	}

	void muteButton()
	{
		muted = !muted;
		if (muted)
        {
			b1.GetComponent<Image>().sprite = bIconB1Toggle;
		}
        else
        {
			b1.GetComponent<Image>().sprite = bIconB1;
		}
	}

	void exitButton()
	{
		quitToggle = !quitToggle;
		yes.gameObject.SetActive(quitToggle);
		no.gameObject.SetActive(quitToggle);
		quitButton.gameObject.SetActive(quitToggle);
		mainButton.interactable = false;
		b1.interactable = false;
		b2.interactable = false;
		b3.interactable = false;
		Color col = quitButton.GetComponent<Image>().color;
		col.a = 0;
		quitButton.GetComponent<Image>().color = col;
		yes.GetComponent<Image>().color = col;
		no.GetComponent<Image>().color = col;
		StartCoroutine(fadeIn());
	}

	void yesButton()
    {
		//return to main menu
    }

	void noButton()
    {
		quitToggle = !quitToggle;
		
		StartCoroutine(fadeOut());
	}

	void advancedSettingsButton()
	{

	}

	private IEnumerator fadeOut()
	{
		Color col = quitButton.GetComponent<Image>().color;
		while (col.a > 0)
		{                  
			col.a -= Time.deltaTime / 1;
			quitButton.GetComponent<Image>().color = col;
			yes.GetComponent<Image>().color = col;
			no.GetComponent<Image>().color = col;
			yield return null;
		}
		if (col.a <= 0)
        {
			yes.gameObject.SetActive(quitToggle);
			no.gameObject.SetActive(quitToggle);
			quitButton.gameObject.SetActive(quitToggle);
			mainButton.interactable = true;
			b1.interactable = true;
			b2.interactable = true;
			b3.interactable = true;
		}
	}

	private IEnumerator fadeIn()
	{
		Color col = quitButton.GetComponent<Image>().color;
		while (col.a < 1)
		{                   
			col.a += Time.deltaTime / 1;
			quitButton.GetComponent<Image>().color = col;
			yes.GetComponent<Image>().color = col;
			no.GetComponent<Image>().color = col;
			yield return null;
		}
		
		
	}
}