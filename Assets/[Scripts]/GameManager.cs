using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int enemiesAlive = 0;
    public int round = 0;
    public GameObject[] spawnPoints;
    public GameObject enemyPrefab;
    public Text roundNumber;
    public GameObject gameOverScreen;
    public Text roundStatistics;
    public GameObject pauseMenu;
    public GameObject doorToFreedom;
    //public Text freedomText;

    public Animator fadeScreenAnimator;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        //Cursor.lockState = CursorLockMode.None;
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesAlive == 0)
        {
            round++;
            NextWave(round);
            roundNumber.text = "Round: " + round.ToString();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        
    }

    public void NextWave(int round)
    {
        for (var x = 0; x < round; x++)
        {
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            //EnemyPrefab, spawn position, rotaion)
            GameObject enemySpawned = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
            enemySpawned.GetComponent<EnemyManager>().gameManager = GetComponent<GameManager>();
            enemiesAlive++;
        }
        
        if(round > 3)
        {
            Destroy(doorToFreedom);
        }
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1;
        AudioListener.volume = 1;
        fadeScreenAnimator.SetTrigger("FadeIn");
        Invoke("LoadMainMenu", 0.5f);

    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        roundStatistics.text = "ROUNDS SURVIVED: " + round.ToString();
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        AudioListener.volume = 0;
    }

    public void ContinueGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        AudioListener.volume = 1;

    }
}
