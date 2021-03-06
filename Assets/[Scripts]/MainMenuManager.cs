using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        PlayerPrefs.SetInt("Diff", 100);
        SceneManager.LoadScene(1);
    }
    public void StartGameEasy()
    {
        PlayerPrefs.SetInt("Diff", 100);
        SceneManager.LoadScene(1);
    }
    public void StartGameMedium()
    {
        PlayerPrefs.SetInt("Diff", 50);
        SceneManager.LoadScene(1);
    }
    public void StartGameHard()
    {
        PlayerPrefs.SetInt("Diff", 30);
        SceneManager.LoadScene(1);
    }
    

    public void LoadOption()
    {
        SceneManager.LoadScene(5);
    }

    public void LoadMultiplayer()
    {
        SceneManager.LoadScene(6);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void InscrctionScreen()
    {
        SceneManager.LoadScene(8);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
