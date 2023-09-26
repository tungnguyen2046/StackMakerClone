using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void FirstLevel()
    {
        StartCoroutine(SceneTransition());
    }

    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator SceneTransition()
    {
        anim.SetTrigger("end");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}
