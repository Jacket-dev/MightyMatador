using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    public float moveSpeed;
    public float jumpSpeed;
    private bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = true;
        rb.velocity = new Vector2(Input.GetAxis("Horizontal")*moveSpeed,0);
        if(Input.GetAxis("Jump")!=0 && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, Input.GetAxis("Jump")*jumpSpeed);
        }
    }
}
