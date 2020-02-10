using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class story : MonoBehaviour
{
    public Text text;
    public string[] sentences;
    public int index = 0;
    public float typingSpeed;
    public GameObject continueButton;
    public GameObject textField;

    private void Start()
    {
        StartCoroutine(Type());
        continueButton.SetActive(false);
    }

    private void Update()
    {
        if(text.text == sentences[index])
        {
            continueButton.SetActive(true);
        }
    }

    IEnumerator Type()
    {
        foreach(char letter in sentences[index].ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
       
    }

    public void nextSentence()
    {
        continueButton.SetActive(false);
        if (index < sentences.Length - 1)
        {
            index++;
            text.text = "";
            StartCoroutine(Type());
        }
        else
        {
            text.text = "";
            continueButton.SetActive(false);
            textField.SetActive(false);
        }
    }
}