using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody2D myRB;
    public Vector2 velocity;
    public Vector2 respawnPos;
    private Quaternion zero;
    public gameManager gm;

    public int health = 5, maxHealth = 5;
    private int jumps = 0;
    private int dash = 0;
    public float speed = 5, defaultSpeed = 5;
    public float jumpHeight = 6.25f;
    public float groundDetectDistance = .1f;
    public Vector2 groundDetection;
    public int dashDistance = 400;
    private int direction = 1;
    private float dashStamp;
    public float dashDuration = 1;
    public int knives = 10, knivesMax = 10;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        respawnPos = new Vector2(-50, -1);
        zero = new Quaternion();

        gm = GameObject.Find("GameManager").GetComponent<gameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        groundDetection.x = transform.position.x;
        groundDetection.y = transform.position.y - 1f;

        if (health <= 0)
        {
            transform.SetPositionAndRotation(respawnPos, zero);
            health = maxHealth;
        }
        velocity = myRB.velocity;

        if (Time.time >= dashStamp)
            speed = defaultSpeed;

        velocity.x = Input.GetAxisRaw("Horizontal") * speed;
        if (velocity.x >= 1)
            direction = 1;
        else if (velocity.x <= -1)
            direction = -1;

        //Debug.DrawRay(groundDetection, Vector2.down, UnityEngine.Color.white);

        // if your raycast is touching the ground your jumps and dash will reset, but the dash still has a cooldown
        if (Physics2D.Raycast(groundDetection, Vector2.down, groundDetectDistance))
        {
            jumps = 0;
            dash = 0;
        }

        //controls if you can jump
        if (Input.GetKeyDown(KeyCode.Space) && (Physics2D.Raycast(groundDetection, Vector2.down, groundDetectDistance)||jumps < 1))
        {
            jumps += 1;
            velocity.y = jumpHeight;
        }

        //allows you to dash
        if (dash < 1 && dashStamp <= Time.time && Input.GetKeyDown(KeyCode.Z))
        {
            velocity = new Vector2(dashDistance * direction, velocity.y);
            dashStamp = Time.time + dashDuration;
            dash += 1;
            speed = dashDistance;
        }

        myRB.velocity = velocity;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.name.Contains("checkpoint"))
        {
            respawnPos = collision.gameObject.transform.position;
            knives = knivesMax;
            health = maxHealth;
        }
        if (collision.gameObject.name.Contains("lava"))
            health = 0;
        if (collision.gameObject.name.Contains("Falling"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
            Debug.Log("GameObject2 collided with " + collision.name);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //make enenmies repel on collision later
        if (collision.gameObject.name.Contains("Enemy")||collision.gameObject.name.Contains("spike"))
        {
            health-= 1;
        }
        if (collision.gameObject.name.Contains("lava"))
            health = 0;
    }

}
