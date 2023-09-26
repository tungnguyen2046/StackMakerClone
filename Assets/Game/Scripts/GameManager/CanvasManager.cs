using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public Animator anim;
    public TextMeshProUGUI lvTxt;
    public GameObject loseScreen;
    public GameObject winScreen;
    public GameObject nextScreen;

    #region Singleton
    public static CanvasManager Ins;
    private void Awake()
    {
        Ins = this;
    }
    #endregion

    void Start()
    {
        lvTxt.text = "Level: " +  SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadNextLv()
    {
        StartCoroutine(LoadScene((SceneManager.GetActiveScene().buildIndex + 1)));
    }

    public void ReloadScene()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void BackMenu()
    {
        StartCoroutine(LoadScene(0));
    }

    IEnumerator LoadScene(int scene)
    {
        anim.SetTrigger("end");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }

    public void LoseScreen()
    {
        loseScreen.SetActive(true);
    }

    public void WinScreen()
    {
        winScreen.SetActive(true);
    }

    public void NextScreen()
    {
        nextScreen.SetActive(true);
    }
}
