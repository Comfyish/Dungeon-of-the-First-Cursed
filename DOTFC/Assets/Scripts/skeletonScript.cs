using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonScript : MonoBehaviour
{
    private Rigidbody2D myRB;
    private GameObject playerTarget;
    private bool isShooting = false;
    private float arrowStamp, direction;

    public GameObject arrow;
    public float arrowSpeed = 25, arrowCooldown = 1.25f, arrowLife = .5f;

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
//        lookPos.Normalize();

        if (isShooting && Time.time > arrowStamp)
        {
            // Checking to see if we're moving to the right
            if (lookPos.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                direction = -1;
            }
            // Checking to see if we're moving to the left
            else if (lookPos.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                direction = 1;
            }

            float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            GameObject b = Instantiate(arrow, new Vector2(transform.position.x + direction, transform.position.y), transform.rotation);
            b.GetComponent<Rigidbody2D>().rotation = angle;
            b.GetComponent<Rigidbody2D>().velocity = new Vector3(arrowSpeed * direction, lookPos.y);

            //Debug.Log(b.GetComponent<Rigidbody2D>().velocity);
            arrowStamp = Time.time + arrowCooldown;
            Destroy(b, arrowLife);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isShooting && (collision.gameObject.name == "player"))
        {
            isShooting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isShooting && (collision.gameObject.name == "player"))
        {
            isShooting = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //make enenmies repel on collision later
        if (collision.gameObject.name.Contains("knife") || collision.gameObject.name.Contains("spike") || collision.gameObject.name.Contains("lava"))
        {
            Destroy(collision.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
