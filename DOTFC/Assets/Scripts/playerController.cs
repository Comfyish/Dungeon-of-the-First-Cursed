using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    private Rigidbody2D myRB;
    private Quaternion zero;
    private int jumps = 0, dash = 0, direction = 1;
    private float dashStamp, knifeStamp, repelX, repelY, repelTime, attackStamp;
    private bool exit = false;

    public Vector2 velocity, respawnPos, groundDetection;
    public GameManager gm;
    public GameObject knife, meleeR, meleeL;
    public int health = 5, maxHealth = 5, dashDistance = 400, knives = 10, knivesMax = 10;
    public float speed = 5, defaultSpeed = 5, jumpHeight = 6.25f, groundDetectDistance = .1f, dashDuration = 1, knifeCooldown = 1, knifeSpeed = 20, knifeLife = 2, repelDur;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        respawnPos = new Vector2(-50, -1);
        zero = new Quaternion();
        meleeL.SetActive(false);
        meleeR.SetActive(false);

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
            health = maxHealth;
            knives = knivesMax;
        }
        velocity = myRB.velocity;

        if (Time.time >= dashStamp)
            speed = defaultSpeed;
        if (repelTime < Time.time)
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

        if (knives > 0 && Time.time > knifeStamp)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                GameObject b = Instantiate(knife, new Vector2(transform.position.x - 1, transform.position.y), transform.rotation);
                b.GetComponent<Rigidbody2D>().velocity = new Vector2(-knifeSpeed, 0);

                //Debug.Log(b.GetComponent<Rigidbody2D>().velocity);
                knifeStamp = Time.time + knifeCooldown;
                Destroy(b, knifeLife);

                knives --;
            }

            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                GameObject b = Instantiate(knife, new Vector2(transform.position.x + 1, transform.position.y), transform.rotation);
                b.GetComponent<Rigidbody2D>().velocity = new Vector2(knifeSpeed, 0);
                knifeStamp = Time.time + knifeCooldown;
                Destroy(b, knifeLife);

                knives--;
            }
        }

        //for melee
        if (Input.GetKeyDown(KeyCode.LeftShift) && attackStamp < Time.time)
        {
            if (direction == 1)
            {
                
            }
            else if( direction == -1)
            {

            }
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
        if (collision.gameObject.name.Contains("arrow"))
        {
            health--;
            repelX = collision.gameObject.GetComponent<Rigidbody2D>().velocity.x;
            repelY = collision.gameObject.GetComponent<Rigidbody2D>().velocity.y;
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(repelX, repelY);
            repelTime = Time.time + repelDur;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name.Contains("Level"));
        {
            gm.levelName = collision.gameObject.name;
            gm.GameEnd = true;
        }
    }

}
