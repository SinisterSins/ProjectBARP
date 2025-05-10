using UnityEngine;

public class BackgroundSwitch : MonoBehaviour
{
    public BackgroundSetManager manager;
    public GameObject targetSet;
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
        if (other.CompareTag("Player") && manager !=null && targetSet != null)
        {
            manager.SetActiveSet(targetSet);
        }
    }
}
