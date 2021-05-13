﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeScript : MonoBehaviour
{
    private Rigidbody2D myRB;
    public GameObject playerTarget;
    public float movementSpeed = 3;
    public bool isFollowing = false;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        playerTarget = GameObject.Find("player");
        playerController playerController = playerTarget.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookPos = playerTarget.transform.position - transform.position;
        //float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        //myRB.rotation = angle;
        lookPos.Normalize();

        if (isFollowing)
        {
            myRB.velocity = new Vector2(lookPos.x * movementSpeed, 0);

        // Checking to see if we're moving to the right
            if (myRB.velocity.x > 0)
                GetComponent<SpriteRenderer>().flipX = false;

        // Checking to see if we're moving to the left
            else if (myRB.velocity.x < 0)
                GetComponent<SpriteRenderer>().flipX = true;
        }

        else if(isFollowing == false)
            myRB.velocity = new Vector2(0, 0); 

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFollowing && (collision.gameObject.name == "player"))
        {
            isFollowing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isFollowing && (collision.gameObject.name == "player"))
        {
            isFollowing = false;
        }
    }
}