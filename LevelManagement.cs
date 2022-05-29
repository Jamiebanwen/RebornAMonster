using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagement : MonoBehaviour
{
    // [SerializeField] GameSession gameStatus;
    private void Start() 
    {
        GameSession gameStatus = GetComponent<GameSession>();    
    }
    public void LoadNextScene ()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadfirstScene()
    {
        // Destroy GameSession Game Object
        
        SceneManager.LoadScene(0);
        FindObjectOfType<GameSession>().ResetGameSession();
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
