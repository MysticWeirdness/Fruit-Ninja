using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject particleObject;
    [SerializeField] private Collider2D gameCollider;

    [Header("Variables")]
    private bool mouseDown = false;

    private float cooldownDur = 0.1f;
    private float cooldown;
    [SerializeField] private AudioClip[] slashingSounds;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        Cursor.visible = false;
        CuttingActivation(mouseDown);
    }
    private void Update()
    {
        CursorPosition();
        CuttingActivation(mouseDown);

        if(cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    private void CursorPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mousePos.x, mousePos.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (mouseDown)
        {
            if (collision.gameObject.CompareTag("Fruit"))
            {
                if(cooldown <= 0)
                {
                    cooldown = cooldownDur;

                    if (collision.gameObject.GetComponent<FruitScript>().isSliced == false)
                    {
                        FruitScript fruitScript = collision.GetComponent<FruitScript>();
                        audioSource.PlayOneShot(slashingSounds[RandomChoice(slashingSounds.Length)]);
                        fruitScript.DamageFruit();
                    }
                }
            }
        }
    }
    private void CuttingActivation(bool active)
    {
        gameCollider.enabled = active;
        particleObject.SetActive(active);
    }

    private void OnMouseDown()
    {
        mouseDown = true;
    }

    private void OnMouseUp() 
    {
        mouseDown = false;
    }

    private int RandomChoice(int length)
    {
        return Random.Range(0, length);
    }
}
