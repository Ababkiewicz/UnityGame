using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    public Text pointText;
    public Text mainText;
    public Text subText;
    private Animator transitionAnim;
    public static string previousLevel;


    public void SetupGameOver(int score)
    {
        gameObject.SetActive(true);
        mainText.text = "Game Over";
        subText.enabled = false;
        pointText.text = score.ToString() + " Points";
        transitionAnim = GameObject.Find("WallTransition").GetComponentInChildren<Animator>();

    }
    public void SetupFinished(int score)
    {
        mainText.text = "The End";
        subText.text = "Thank you for playing the game!";
        gameObject.SetActive(true);
        pointText.text = score.ToString() + " Points";
        transitionAnim = GameObject.Find("WallTransition").GetComponentInChildren<Animator>();

    }
    public void RestartButton()
    {
        StartCoroutine(Restart());
    }
    public void MenuButton()
    {
        StartCoroutine(Menu());
    }
    public void ExitButton()
    {
        Application.Quit();
    }
    IEnumerator Menu()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("mainMenu");
    }
    IEnumerator Restart()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        transitionAnim.SetTrigger("end");        
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("FirstLevel");
    }


}
