using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Animator transitionAnim;
    public void Start()
    {
        transitionAnim = GetComponentInChildren<Animator>();        
        transitionAnim.SetTrigger("start");
        
        

    }
    public void PlayGame()
    {
        StartCoroutine(NextScene());
    }
    IEnumerator NextScene()
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {

        Application.Quit();
    }
}

