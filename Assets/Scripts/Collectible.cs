using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] ItemType type;
    GameManager gameManager;
    enum ItemType
    {
        HEAL = 1,
        POWER_UP = 2,
        VICTORY =3
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void Effect()
    {
        switch (type)
        {
            case ItemType.HEAL:
                gameManager.IncreaseHealth(1);
                break;
            case ItemType.VICTORY:
                gameManager.victoryMenu.SetActive(true);
                break;
        }


    }

}
