using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject HeartPrefab;
    public bool ShowHUD;
    [SerializeField] private int PlayerMaxHealth;
    public int PlayerHealth { get; private set; }
    private GameObject HUD;
    private List<GameObject> Hearts = new List<GameObject>();
    private Vector2 lastHeartPosition;
    public GameObject DefeatMenu;
    public AudioSource actionSound;
    public GameObject victoryMenuPrefab;
    private GameObject victoryMenuInstance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayerHealth = PlayerMaxHealth;
        HUD = transform.Find("HUD").gameObject;
        HUD.GetComponent<Canvas>().worldCamera = Camera.main;
        DrawHearts(PlayerHealth);
    }

    private void Update()
    {
        HUD.SetActive(ShowHUD);     
        if (!HUD.GetComponent<Canvas>().worldCamera)
        {
            HUD.GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }

    public void DrawHearts(int number)
    {
        Hearts.Clear();
        for (int i = 0; i < number; i++)
        {
            GameObject heart = Instantiate(HeartPrefab, HUD.transform);
            Hearts.Add(heart);
            heart.transform.localPosition = new Vector2(heart.transform.localPosition.x + (i * 180), heart.transform.localPosition.y);
            lastHeartPosition = heart.transform.localPosition;
        }
    }

    private void setHearts()
    {
        foreach(GameObject heart in Hearts) 
        {
            heart.GetComponent<SpriteRenderer>().enabled = false;
        }
        for (int i = 0; i < PlayerHealth; i++)
        {
            Hearts[i].GetComponent<SpriteRenderer>().enabled = true;
        }
        
    }

    public void IncreaseHealth(int health)
    {
        if (PlayerHealth < PlayerMaxHealth)
        {
            PlayerHealth += health;
            if (PlayerHealth > PlayerMaxHealth)
            {
                PlayerHealth = PlayerMaxHealth;
            }
        }
        setHearts();
    }

    public void DamagePlayer(int dmg)
    {
        PlayerHealth -= dmg;
        if (PlayerHealth <= 0)
        {
            GameOver();
        }
        setHearts();
    }

    public void GameOver()
    {
        DefeatMenu.SetActive(true);
        PlayerHealth = PlayerMaxHealth;
        DrawHearts(PlayerMaxHealth);
        Time.timeScale = 0.0f;
    }
    private IEnumerator LoadSceneAfterSound(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ReloadLeve()
    {
        bool isActive = DefeatMenu.activeSelf;
        DefeatMenu.SetActive(!isActive);
        Time.timeScale = 1.0f;
        if (actionSound != null)
        {
            actionSound.Play();
            StartCoroutine(LoadSceneAfterSound(actionSound.clip.length));
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void ShowVictoryMenu()
    {
        Debug.Log("Victory menu");
        if (victoryMenuInstance == null)
        {
            Debug.Log("era null");
            victoryMenuInstance = Instantiate(victoryMenuPrefab);
            Debug.Log("creado");
        }
        victoryMenuInstance.SetActive(true);
        Debug.Log("set active tru");
    }

}
