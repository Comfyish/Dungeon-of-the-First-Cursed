using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody2D myRB;
    public Vector2 velocity;
    public Vector2 respawnPos;
    private Quaternion zero;
    public GameManager gm;

    public int health = 3;
    private int jumps = 0;
    private int dash = 0;
    public float speed = 5;
    public float jumpHeight = 6.25f;
    public bool isGrounded = true;
    public float groundDetectDistance = .1f;
    public Vector2 groundDetection;
    public int dashDistance = 8;
    private int direction;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        respawnPos = new Vector2(0, 0);
        zero = new Quaternion();

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        groundDetection.x = transform.position.x;
        groundDetection.y = transform.position.y - 1f;

        if (health <= 0)
        {
            transform.SetPositionAndRotation(respawnPos, zero);
            health = 3;
        }
        velocity = myRB.velocity;


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
        if (Input.GetKeyDown(KeyCode.Z) && direction == 1)
            velocity = new Vector2(dashDistance, velocity.y);
        else if (Input.GetKeyDown(KeyCode.Z) && direction == -1)
            velocity = new Vector2(-dashDistance, velocity.y);

        myRB.velocity = velocity;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.name.Contains("checkpoint"))
        {
            respawnPos = collision.gameObject.transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        if (Collision.gameObject.name.Contains("enemy"))
        {
            health--;
        }
    }

}
