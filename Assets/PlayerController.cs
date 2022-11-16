using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private float jumpTime;
    public float moveSpeed;
    public float jumpSpeed;
    private bool grounded;
    public float maxJumpTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        jumpTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal")*moveSpeed, rb.velocity.y);
        RaycastHit2D groundedRayCast = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.down, capsuleCollider.bounds.extents.y+.1f, LayerMask.GetMask("Ground"));
        IsGrounded();
        jumpTime=Jump(jumpTime,maxJumpTime);
        Debug.Log(jumpTime);
        /*if(IsGrounded())
        {
            rb.velocity.Set(rb.velocity.x, Input.GetAxis("Jump")*jumpSpeed);
        }*/


    }


    private float Jump(float jumpTime,float maxJumpTime)
    {
        if(Input.GetAxis("Jump")!=0)
        {
            if(IsGrounded())
            {
                jumpTime = 0;
                rb.velocity = new Vector2(rb.velocity.x, Input.GetAxis("Jump") * jumpSpeed);
            }
            if(jumpTime<maxJumpTime)
            {
                jumpTime+=Time.deltaTime;
                rb.velocity = new Vector2(rb.velocity.x, Input.GetAxis("Jump") * jumpSpeed);
            }
        }
        return jumpTime;
    }
    private bool IsGrounded()
    {
        Color rayColor;
        RaycastHit2D groundedRayCast = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.down, capsuleCollider.bounds.extents.y + .1f, LayerMask.GetMask("Ground"));
        Debug.Log(groundedRayCast.collider);
        if(groundedRayCast.collider!=null)
        {
            rayColor = Color.green;
            Debug.DrawRay(capsuleCollider.bounds.center, Vector2.down * (capsuleCollider.bounds.extents.y + 0.1f), rayColor);
            return true;
        }
        else
        {
            rayColor = Color.red;
            Debug.DrawRay(capsuleCollider.bounds.center, Vector2.down * (capsuleCollider.bounds.extents.y + 0.1f), rayColor);
            return false;
        }
    }
}
