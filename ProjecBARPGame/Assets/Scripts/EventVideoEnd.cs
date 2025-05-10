using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EventVideoEnd : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string sceneToLoad;

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
        SceneManager.LoadScene(sceneToLoad);
        Debug.Log("Video ended");
    }
}
