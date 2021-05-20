using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiderScript : MonoBehaviour
{
    private Rigidbody2D myRB;
    private GameObject playerTarget;
    private float directionX, directionY, repelTime;
    public float movementSpeed = 3, repelSpace = 1, repelDur = .2f;
    private bool isFollowing = false;

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
        lookPos.Normalize();

        if (isFollowing && Time.time > repelTime)
        {
            myRB.velocity = new Vector2(lookPos.x * movementSpeed, lookPos.y * movementSpeed);

            if (lookPos.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                directionX = -1; // direction x set to left
            }
            // Checking to see if we're moving to the left
            else if (lookPos.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                directionX = 1; // direction x set to right
            }

            if (lookPos.y < 0)
                directionY = -1; // direction y is set to down
            else if (lookPos.y > 0)
                directionY = 1; // direction y is set to up
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("player"))
        {
            myRB.velocity = new Vector2(directionX * -repelSpace, 0);
            repelTime = Time.time + repelDur;
        }
        if (collision.gameObject.name.Contains("knife") || collision.gameObject.name.Contains("spike") || collision.gameObject.name.Contains("lava"))
        {
            Destroy(collision.gameObject);
            this.gameObject.SetActive(false);   
        }
    }
}