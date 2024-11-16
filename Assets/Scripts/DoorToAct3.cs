using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToAct3 : MonoBehaviour
{
    //[SerializeField] private string sceneToLoad; 

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            Debug.Log("entro");
            SceneManager.LoadScene("Scenes/ActIII");
        }
    }
}


