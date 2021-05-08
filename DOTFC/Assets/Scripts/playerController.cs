using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D myRB;
    public Vector2 velocity;
    public Vector2 respawnPos;
    private Quaternion zero;
    public GameManager gm;
    public int health = 5;
    public float speed = 4;
    public float groundDetectDistance = .1f;
    public Vector2 groundDetection;
    public bool isGrounded = true;
    public float jumpHeight = 6.25f;


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

        velocity = myRB.velocity;
        velocity.x = Input.GetAxisRaw("Horizontal") * speed;
        myRB.velocity = velocity;

        if (health <= 0)
        {
            transform.SetPositionAndRotation(respawnPos, zero);
            health = 5  ;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Physics2D.Raycast(groundDetection, Vector2.down, groundDetectDistance))
        {
            velocity.y = jumpHeight;
        }
        velocity = myRB.velocity;
        Debug.DrawRay(groundDetection, Vector2.down, UnityEngine.Color.white);

    }
}
