using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    public float moveSpeed;
    public float jumpSpeed;
    private bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal")*moveSpeed,0);
        grounded = Physics2D.Raycast(transform.position, -Vector2.up, capsuleCollider.bounds.extents.y+.1f);
        Debug.Log("grounded " + grounded);
        if(grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, Input.GetAxis("Jump")*jumpSpeed);
            grounded=false;
        }

    }
}
