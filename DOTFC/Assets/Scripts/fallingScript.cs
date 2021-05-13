using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingScript : MonoBehaviour
{
    public Vector2 playerDetection;
    public float playerDetectDistance = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerDetection.x = transform.position.x;
        playerDetection.y = transform.position.y - 1f;

        if (Physics2D.Raycast(playerDetection, Vector2.down, playerDetectDistance).collider.name == "player")
        {
            GetComponent<Rigidbody2D>().constraints = ~RigidbodyConstraints2D.FreezePositionY;
        }
        //Debug.DrawRay(playerDetection, Vector2.down, playerDetectDistance, UnityEngine.Color.white);
    }
}
