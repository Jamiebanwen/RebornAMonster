using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{   
    [SerializeField] int playerLives = 3;
    int playerDeaths;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] int score;
    [SerializeField] AudioClip playerdeathSFX;
    int scoreOnSpawn;

    // Start is called before the first frame update
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        } else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() 
    {
        livesText.text = "Deaths : " + playerDeaths.ToString();
        scoreText.text = "Score : " +score.ToString();
        scoreOnSpawn=score;
    }

    // Update is called once per frame
    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            StartCoroutine(TakeLife());
        } else
        {
            StartCoroutine(ResetGameSession());
        }
    }

    public void AddToScore( int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Score : " +score.ToString();
    }

    IEnumerator TakeLife()
    {
        AudioSource.PlayClipAtPoint(playerdeathSFX,Camera.main.transform.position);
        yield return new WaitForSecondsRealtime(1f);
        playerDeaths ++;
        //score=scoreOnSpawn;
        //scoreText.text = "Score : " +score.ToString();
        int currentSession = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSession);
        livesText.text = "Deaths : " + playerDeaths.ToString();
        
    }

    public IEnumerator ResetGameSession()
    {
        yield return new WaitForSecondsRealtime(1f);
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
