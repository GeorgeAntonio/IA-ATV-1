using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private int[] playerPositions;
    private float horizontalInput, verticalInput,moveSpeed;
    Rigidbody2D rb;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SpaceBlock")) {
            playerPositions[0] = (int)collision.gameObject.name[name.Length - 2];
            playerPositions[1] = (int)collision.gameObject.name[name.Length];
        }
    }

    private int lifePoints;
    void Start()
    {
        moveSpeed = 5f;
        rb = GetComponent<Rigidbody2D>();
        lifePoints = 5;

    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive())
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            // Calculate the movement vector based on the input values and the moveSpeed
            Vector2 movement = new Vector2(horizontalInput, verticalInput);
            if (movement.magnitude > 1) movement.Normalize();
            movement *= moveSpeed;

            if (movement != Vector2.zero)
            {
                rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
            }
        }
    }

    public bool isAlive()
    {
        return lifePoints > 0;
    }
    public int[] getPosition()
    {
        return playerPositions;
    }
}
