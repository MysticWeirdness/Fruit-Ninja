using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    private int health;
    private int value;

    private float duration = 10f;

    private bool isSliced;

    [Header("Components")]
    protected private Rigidbody2D rb;
    private ParticleSystem particles;
    protected private SpriteRenderer spriteRenderer;
    private FollowMouse player;
    private AudioSource audioSource;
    [SerializeField] AudioClip[] clipList;

    [Header("Generation Vars")]
    private int genIndex;
    private int genLength;
    [SerializeField] private Sprite[] spriteList;

    [Header("Physics Variables")]
    [SerializeField] private Vector2 min;
    [SerializeField] private Vector2 max;

    protected private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

    private void FixedUpdate()
    {
        DeathCheck();
    }

    private void DeathCheck()
    {
        duration -= Time.deltaTime;

        if(duration <= 0)
        {
            Destroy(gameObject);
        }

        if(transform.position.y < -15)
        {
            Destroy(gameObject);
        }
    }

    private void StartPhysics()
    {
        float yVel = Random.Range(min.y, max.y);

        if(transform.position.x < -2)
        {
            min.x = 5;
            max.x = 8;
        }
        else if(transform.position.x > 2)
        {
            min.x = -8;
            max.x = -5;
        }
        else
        {
            min.x = -7;
            max.x = 7;
        }

        float xVel = Random.Range(min.x, max.x);

        rb.AddForce(new Vector2(xVel, yVel));

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
        audioSource.PlayOneShot(clipList[RandomChoice(clipList.Length)]);

        if(genIndex < genLength)
        {
            Instantiate(this.gameObject, transform.position, Quaternion.identity).GetComponent<FruitScript>().Initialize(genIndex);
            Instantiate(this.gameObject, transform.position, Quaternion.identity).GetComponent<FruitScript>().Initialize(genIndex);
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
        Destroy(gameObject, 2f);
    }

    private int RandomChoice(int length)
    {
        return Random.Range(0, length);
    }
}
