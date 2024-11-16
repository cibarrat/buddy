using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public AudioSource actionSound;
    [SerializeField] private GameObject pause;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            pause.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        pause.SetActive(false);
        
    }
    public void ChangeScene(int scene)
    {
        Time.timeScale = 1f;
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
    private IEnumerator LoadSceneAfterSound(int scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
        pause.gameObject.SetActive(false);
    }
}

