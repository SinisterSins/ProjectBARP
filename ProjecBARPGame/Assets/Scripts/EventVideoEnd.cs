using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EventVideoEnd : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        videoPlayer.loopPointReached += OnLoopPointReached;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLoopPointReached(VideoPlayer vp)
    {
        SceneManager.LoadScene("Initializer");
        Debug.Log("Video ended");
    }
}
