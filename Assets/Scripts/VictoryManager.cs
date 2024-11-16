using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public AudioSource actionSound;
    [SerializeField] private GameObject victoryMenu;


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
        victoryMenu.gameObject.SetActive(false);
    }
}
