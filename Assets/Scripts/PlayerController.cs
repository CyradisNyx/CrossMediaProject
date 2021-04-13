using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool selected = false;
    public float moveSpeed = 5f;
    public Transform movePoint;
    public Transform startPoint;
    public GameObject levelEnd;
    public Collider2D playerCollider;

    private void Start()
    {
        movePoint.parent = null;
        startPoint.parent = null;

        if (playerCollider == null)
        {
            playerCollider = gameObject.GetComponent<Collider2D>();
        }

        EventMaster.Instance.ONEndTurn += OnEndTurn;
        EventMaster.Instance.ONHighlightedTileClicked += OnHighlightedTileClicked;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        selected = !selected;
        if (selected)
        {
            EventMaster.Instance.SelectPlayer();
        }
        else
        {
            EventMaster.Instance.DeselectPlayer();
        }
    }

    private void OnEndTurn()
    {
        startPoint.position = movePoint.position;

        if (Vector3.Distance(transform.position, levelEnd.transform.position) <= .05f)
        {
            EventMaster.Instance.CompleteLevel();
        }
    }

    private void OnHighlightedTileClicked(GameObject which)
    {
        Vector3 newPos = which.transform.position;
        movePoint.position = new Vector3(newPos.x, newPos.y, gameObject.transform.position.z);
    }

    private void MoveDirectControl()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
            }
            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                movePoint.position += new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
            }
        }
    }
}
