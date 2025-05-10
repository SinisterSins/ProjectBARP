using System;
using UnityEngine;
using TMPro;
public class TutorialScript : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    public string[] tutorialText;

    private int currentIndex = 0; //what one we are currently 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TextObject"))
        {
            //change the current text
            textMeshPro.text = tutorialText[currentIndex];

            //increment the indext
            currentIndex = (currentIndex + 1);
        }
    }
}