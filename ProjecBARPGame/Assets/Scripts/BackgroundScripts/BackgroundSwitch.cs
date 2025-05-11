using UnityEngine;

public class BackgroundSwitch : MonoBehaviour
{
    public BackgroundSetManager manager;
    public GameObject targetSet;
    // Add variable for audio source
    public AudioSource currentBackgroundMusic;
    public AudioSource nextBackgroundMusic;
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
        if (other.CompareTag("Player") && manager != null && targetSet != null)
        {
            //Trigger audio
            currentBackgroundMusic.Stop();
            nextBackgroundMusic.Play();
            manager.SetActiveSet(targetSet);
        }

    }
}
