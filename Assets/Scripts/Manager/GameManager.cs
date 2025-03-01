using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float loadDelay = 0.5f;
    
    void Awake()
    {
        // Initiate a game session
        int numGameManagers = FindObjectsOfType<GameManager>().Length;
        if (numGameManagers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Either reset the current level or the whole game session
    public void ProcessPlayerCrash()
    {
        Invoke("ResetGame", loadDelay);
    }

    // Reset game session to the first level
    void ResetGame()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
