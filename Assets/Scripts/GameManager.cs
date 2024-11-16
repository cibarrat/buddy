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
    [SerializeField] private bool ShowHUD;
    [SerializeField] private int PlayerMaxHealth;
    public int PlayerHealth { get; private set; }
    private Canvas HUD;
    private List<GameObject> Hearts = new List<GameObject>();
    private Vector2 lastHeartPosition;
    public GameObject DefeatMenu;
    public AudioSource actionSound;

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

    private void Update()
    {
        HUD.enabled = ShowHUD;            
    }

    private void Start()
    {
        PlayerHealth = PlayerMaxHealth;
        HUD = GetComponentInChildren<Canvas>();
        DrawHearts(PlayerHealth);
    }

    public void DrawHearts(int number)
    {
        Hearts.Clear();
        for (int i = 0; i < number; i++)
        {
            GameObject heart = Instantiate(HeartPrefab, HUD.gameObject.transform);
            Hearts.Add(heart);
            heart.transform.localPosition = new Vector2(heart.transform.localPosition.x + (i * 50), heart.transform.localPosition.y);
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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        bool isActive = DefeatMenu.activeSelf;
        DefeatMenu.SetActive(!isActive);
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

}
