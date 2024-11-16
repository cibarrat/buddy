using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DefeatMenu : MonoBehaviour
{
    public AudioSource actionSound;

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
        this.gameObject.SetActive(false);
    }
}
