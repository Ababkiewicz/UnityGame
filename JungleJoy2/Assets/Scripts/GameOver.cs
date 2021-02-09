using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    public Text pointText;
    private Animator transitionAnim;
    public static string previousLevel;


    public void Setup(int score)
    {
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
