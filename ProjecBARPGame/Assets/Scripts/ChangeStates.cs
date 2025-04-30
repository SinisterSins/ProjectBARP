using UnityEngine;

public class ChangeStates : MonoBehaviour
{
    private Animator myAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Trigger for Player Jump animation
        if (Input.GetKeyDown(KeyCode.W))
        {
            myAnimator.SetTrigger(name: "DoJump");

            Debug.Log("player jumped");
        }

        //Trigger for Player Slide animation
        if (Input.GetKeyDown(KeyCode.S))
        {
            myAnimator.SetTrigger(name: "DoSlide");

            Debug.Log("player slide");
        }

        //Trigger for Player Punch animation
        if (Input.GetKeyDown(KeyCode.D))
        {
            myAnimator.SetTrigger(name: "DoPunch");

            Debug.Log("player punched");
        }

        //Trigger for Player Block animation
        if (Input.GetKeyDown(KeyCode.A))
        {
            myAnimator.SetTrigger(name: "DoBlock");

            Debug.Log("player blocked");
        }
    }
}
