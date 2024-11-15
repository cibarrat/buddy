using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Menu : MonoBehaviour
{
    public AudioSource actionSound; 

    public void ChangeScene(int scene)
    {
        if (actionSound != null)
        {
            actionSound.Play();
            StartCoroutine(LoadSceneAfterSound(scene, actionSound.clip.length));
        }
        else
        {
            SceneManager.LoadScene(scene);
        }
    }

    public void ExitGame()
    {
        if (actionSound != null)
        {
            actionSound.Play();
            StartCoroutine(QuitAfterSound(actionSound.clip.length));
        }
        else
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    private IEnumerator LoadSceneAfterSound(int scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }

    private IEnumerator QuitAfterSound(float delay)
    {
        yield return new WaitForSeconds(delay);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void Start()
    {

    }


    void Update()
    {

    }
}
