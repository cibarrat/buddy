using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryObject : MonoBehaviour
{
    [SerializeField] private GameObject VictoryMenu;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Mirror");
            VictoryMenu.SetActive(true);
        }
    }
}
