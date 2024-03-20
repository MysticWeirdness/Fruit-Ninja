using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Variables")]
    private int points;
    private int lives;
    private bool isRunning;
    private float pointMulti;

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
    private List<TextMeshProUGUI> livesObject;

    private void Awake()
    {
        defaultSpawnMinMax = new Vector2 (1, 5);
    }
    public void NewGame()
    {
        points = 0;
        lives = 3;
        pointMulti = 1;
        spawnMinMax = defaultSpawnMinMax;
        isRunning = true;


        livesObject = new List<TextMeshProUGUI>();
        foreach (TextMeshProUGUI life in lifeList)
        {
            livesObject.Add(life);
            life.color = Color.red;
        }
    }

    private void Start()
    {
        NewGame();
    }

    private void UpdateUI()
    {
        pointsTextElement.text = points.ToString("0000");
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

    private void Die()
    {

    }
}
