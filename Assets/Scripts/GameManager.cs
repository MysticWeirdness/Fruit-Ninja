using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Variables")]
    private int points;
    private int lives;
    private float pointMulti;
    private bool running = false;

    [Header("Fruit Variables")]
    [SerializeField] private GameObject[] fruits;
    [SerializeField] private GameObject[] specialFruits;
    private Vector2 spawnMinMax;
    private Vector2 defaultSpawnMinMax;
    private float spawnCooldown;

    [Header("Components")]
    private FollowMouse mouseScript;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI pointsTextElement;
    [SerializeField] private TextMeshProUGUI[] lifeList;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI gameOverScore;
    private Animator gameOverAnimator;
    private List<TextMeshProUGUI> livesObject = new List<TextMeshProUGUI>();


    private void Awake()
    {
        defaultSpawnMinMax = new Vector2 (1, 2);
        gameOverAnimator = gameOverScreen.GetComponent<Animator>();
    }
    public void NewGame()
    {
        running = true;
        points = 0;
        lives = 3;
        pointMulti = 1;
        spawnMinMax = defaultSpawnMinMax;
        livesObject = new List<TextMeshProUGUI>();
        foreach (TextMeshProUGUI life in lifeList)
        {
            livesObject.Add(life);
            life.color = Color.red;
        }
        gameOverScreen.SetActive(false);
        Time.timeScale = 1.0f;
    }

    private async void Start()
    {
        await StartTimer();
    }

    private async Task StartTimer()
    {
        await Task.Delay(3000);
        NewGame();
    }
    public void SetMulti(float Multi)
    {
        pointMulti = Multi;
    }
    private void Update()
    {
        if (running)
        {
            if (spawnCooldown > 0)
            {
                spawnCooldown -= Time.deltaTime;
            }
            else
            {
                SpawnFruit();
            }
        }
    }

    private void UpdateUI()
    {
        pointsTextElement.text = points.ToString("0000");
    }

    private void SpawnFruit()
    {
        spawnCooldown = Random.Range(spawnMinMax.x, spawnMinMax.y);
        Vector2 spawnPoint = new Vector2(Random.Range(-6f, 6f), -6f);
        Instantiate(fruits[RandomChoice(fruits.Length)], spawnPoint, Quaternion.identity);
    }

    public void AddPoints(int amount)
    {
        points += (int)(amount * pointMulti);
        UpdateUI();
    }

    public void LoseLife()
    {
        lives--;
        if(lives <= 0)
        {
            Die();
        }
        TextMeshProUGUI life = livesObject[livesObject.Count - 1];
        livesObject.Remove(life);
        life.color = Color.gray;
    }

    private int RandomChoice(int length)
    {
        return Random.Range(0, length);
    }

    private void Die()
    {
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
        pointsTextElement.enabled = false;
        if (PlayerPrefs.HasKey("Highscore"))
        {
            if(points > PlayerPrefs.GetInt("Highscore"))
            {
                PlayerPrefs.SetInt("Highscore", points);
            }
        }
        else
        {
            PlayerPrefs.SetInt("Highscore", 0);
        }

        highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore").ToString("0000");
        gameOverScore.text = "Score: " + points.ToString("000");
        lives = 0;
        foreach(TextMeshProUGUI life in livesObject)
        {
            life.color = Color.gray;
        }
    }
}
