using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public AudioClip press;
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Level");
    }
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ClickSound()
    {
        GetComponent<AudioSource>().PlayOneShot(press);
    }
}
