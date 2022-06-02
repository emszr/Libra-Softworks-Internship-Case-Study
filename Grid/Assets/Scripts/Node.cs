using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private GameObject highligt;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite [] sprites;

    private int x;
    private int y;
    private bool isFull;

    private void Start()
    {
        isFull = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetValues(int x, int y)
    {
        this.x = x;
        this.y = y;  
    }

    private void OnMouseDown()
    {
        if (isFull == false)
        {
            spriteRenderer.sprite = sprites[1];
            isFull = true;
            pointNode();           
        }     
    }

    public void setEmptySprite()
    {
        Debug.Log(x + " " + y);

        isFull = false;
        spriteRenderer.sprite = sprites[0];
    }

    private void pointNode()
    {
        Grid grid = FindObjectOfType<Grid>();
        grid.pointNode(x, y);
    }

}
