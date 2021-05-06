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
        velocity = myRB.velocity;
        velocity.x = Input.GetAxisRaw("Horizontal") * speed;
        myRB.velocity = velocity;

    }
}
