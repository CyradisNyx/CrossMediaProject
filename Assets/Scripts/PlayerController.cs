﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool selected = false;
    public float moveSpeed = 5f;
    public Transform movePoint;
    public Transform startPoint;
    public Collider2D playerCollider;

    private void Start()
    {
        movePoint.parent = null;
        startPoint.parent = null;

        if (playerCollider == null)
        {
            playerCollider = gameObject.GetComponent<Collider2D>();
        }

        EventMaster.Instance.ONStartTurn += OnStartTurn;
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

    private void OnStartTurn()
    {
        startPoint.position = gameObject.transform.position;
    }

    private void OnHighlightedTileClicked(GameObject which)
    {
        movePoint.position = which.transform.position;
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
