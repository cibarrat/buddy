using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToAct2 : MonoBehaviour
{
    //[SerializeField] private string sceneToLoad; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("Scenes/ActII"); 
        }
    }
}
