using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] public int value;

    public bool isSliced { get; private set; } = false;

    [Header("Components")]
    protected private Rigidbody2D rb;
    private ParticleSystem particles;
    protected private SpriteRenderer spriteRenderer;
    private FollowMouse player;
    private AudioSource audioSource;
    private GameManager gameManager;
    [SerializeField] AudioClip[] clipList;

    [Header("Generation Variables")]
    [SerializeField] private int genIndex;
    [SerializeField] private int genLength;
    [SerializeField] private Sprite[] spriteList;

    [Header("Physics Variables")]
    [SerializeField] private Vector2 min;
    [SerializeField] private Vector2 max;

    protected private void Awake()
    {
        genLength = spriteList.Length - 1;
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        player = GetComponent<FollowMouse>();
        audioSource = GetComponent<AudioSource>();
        particles = GetComponentInChildren<ParticleSystem>();
        isSliced = false;
    }

    private void Start()
    {
        StartPhysics();
    }

    private void Update()
    {
        DeathCheck();
    }

    private void DeathCheck()
    {
        if(transform.position.y <= -8)
        {
            if(genIndex == 0 & !isSliced)
            {
                gameManager.LoseLife();
            }
            Destroy(gameObject);
        }
    }
    private void StartPhysics()
    {
        float yVel = Random.Range(min.y, max.y);

        if(genIndex > 0)
        {
            min.x = -5;
            max.x = 5;
            min.y = 0;
            max.y = 8;
            yVel = Random.Range(min.y, max.y);
        }
        else
        {
            if (transform.position.x < -2)
            {
                min.x = 5;
                max.x = 8;
            }
            else if (transform.position.x > 2)
            {
                min.x = -8;
                max.x = -5;
            }
            else
            {
                min.x = -7;
                max.x = 7;
            }
        }

        float xVel = Random.Range(min.x, max.x);

        rb.AddForce(new Vector2(xVel, yVel), ForceMode2D.Impulse);

        rb.angularVelocity = ((xVel * yVel) / 2) * 10;
    }

    public void DamageFruit()
    {
        health -= 1;

        if(health <= 0)
        {
            SliceFruit();
        }
    }

    public void Initialize(int Index)
    {
        genIndex = Index + 1;
        spriteRenderer.sprite = spriteList[genIndex];

        if(genIndex >= genLength)
        {
            Destroy(GetComponent<Collider2D>());
        }
    }
    public virtual void SliceFruit()
    {
        isSliced = true;
        gameManager.AddPoints(value);
        audioSource.PlayOneShot(clipList[RandomChoice(clipList.Length)]);

        if(genIndex < genLength)
        {
            Instantiate(gameObject, transform.position, Quaternion.identity).GetComponent<FruitScript>().Initialize(genIndex);
            Instantiate(gameObject, transform.position, Quaternion.identity).GetComponent<FruitScript>().Initialize(genIndex);
            ParticleBurst();
            Destroy(gameObject);
        }
        Destroy(GetComponent<Collider2D>());
        Destroy(gameObject, 3f);
    }

    private void ParticleBurst()
    {
        if(particles == null)
        {
            return;
        }

        particles.transform.parent = null;
        particles.Stop();
        particles.Play();
        Destroy(particles.gameObject, 0.5f);
    }

    private int RandomChoice(int length)
    {
        return Random.Range(0, length);
    }
}
