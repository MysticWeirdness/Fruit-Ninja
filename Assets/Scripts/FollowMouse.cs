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

    private void Start()
    {
        Cursor.visible = false;
        CuttingActivation(mouseDown);
    }
    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mousePos.x, mousePos.y);
        CuttingActivation(mouseDown);
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
}
