using UnityEngine;
using UnityEngine.SceneManagement;

// GameMaster is a persistent script that manages the game as a whole
public class GameMaster : MonoBehaviour
{
    public static GameMaster gm; 

    // runs on initialization of game, before frames are loaded
    void Awake()
    {
        // keeps GameMaster GameObject alive across scenes
        if (gm == null)
        {
            gm = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // move to the first level. this is a WIP until we have a menu screen/scene etc
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            LoadNextLevel();
        }
    }

    // go to the next level
    public void LoadNextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int nextLevel = currentLevel + 1;

        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            Debug.Log("last level reached");
        }
    }

    // restart the current level
    public void RestartLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
    }

    // loads a level by its scene name
    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
