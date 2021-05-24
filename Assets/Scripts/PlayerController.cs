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
    public GameObject coin;
    public Collider2D playerCollider;

    private void Start()
    {
        if (movePoint == null)
        {
            movePoint = gameObject.transform.Find("Player Move Point");
        }
        
        if (startPoint == null)
        {
            startPoint = gameObject.transform.Find("Player Start Point");
        }

        if (levelEnd == null)
        {
            levelEnd = gameObject.transform.parent.Find("LevelEnd").gameObject;
        }
        
        if (coin == null)
        {
            coin = gameObject.transform.parent.Find("Coin").gameObject;
        }

        if (playerCollider == null)
        {
            playerCollider = gameObject.GetComponent<Collider2D>();
        }
        
        movePoint.parent = null;
        startPoint.parent = null;

        EventMaster.Instance.ONStartTurn += OnStartTurn;
        EventMaster.Instance.ONEndTurn += OnEndTurn;
        EventMaster.Instance.ONHighlightedTileClicked += OnHighlightedTileClicked;
    }

    public void OnDestroy()
    {
        EventMaster.Instance.ONEndTurn -= OnEndTurn;
        EventMaster.Instance.ONHighlightedTileClicked -= OnHighlightedTileClicked;
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
        selected = false;
    }

    private void OnEndTurn()
    {
        startPoint.position = movePoint.position;

        if (Compare2D(transform.position, levelEnd.transform.position))
        {
            Debug.Log("complete level");
            EventMaster.Instance.CompleteLevel();
        }

        if (Compare2D(transform.position, coin.transform.position))
        {
            Debug.Log("collect coin");
            coin.GetComponent<CoinComponent>().collected = true;
            coin.SetActive(false);
        }
    }

    private void OnHighlightedTileClicked(GameObject which)
    {
        Vector3 newPos = which.transform.position;
        movePoint.position = new Vector3(newPos.x, newPos.y, gameObject.transform.position.z);
    }

    private bool Compare2D(Vector3 a, Vector3 b)
    {
        Vector2 a2 = new Vector2(a.x, a.y);
        Vector2 b2 = new Vector2(b.x, b.y);

        if (Vector2.Distance(a, b) <= .05f)
        {
            return true;
        }

        return false;
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
