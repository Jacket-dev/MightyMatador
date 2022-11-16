using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private float jumpTime;
    private bool grounded;
    private bool shot;
    private bool jumping;
    public float moveSpeed;
    public float jumpSpeed;
    public float maxJumpTime;
    public GameObject lance;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        jumpTime = 0;
        jumping= false;
        shot = true;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal")*moveSpeed, rb.velocity.y);
        RaycastHit2D groundedRayCast = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.down, capsuleCollider.bounds.extents.y+.1f, LayerMask.GetMask("Ground"));
        //jumpTime=Jump(jumpTime,maxJumpTime,jumping);
        /*if(IsGrounded())
        {
            rb.velocity.Set(rb.velocity.x, Input.GetAxis("Jump")*jumpSpeed);
        }*/


        //Jump Handle
        //Might need changes because right now player sticks on walls could be something else tho
        if (Input.GetAxis("Jump") != 0 && IsGrounded())
        {
            jumping = true;
            jumpTime = 0;
        }
        if(jumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, Input.GetAxis("Jump") * jumpSpeed);
            jumpTime += Time.deltaTime;
        }
        if(jumpTime>maxJumpTime || Input.GetAxis("Jump")==0)
        {
            jumping = false;
        }

        //Projectile Handle
        //Work in progress
        if(Input.GetAxis("Jump") != 0 && jumping==false && !IsGrounded() && !shot)
        {
            Instantiate(lance,this.transform.position, new Quaternion(0,0,0,1));
            shot = true;
        }
        if(Input.GetAxis("Jump") == 0) //Doublon avec un if dans le saut
        {
            shot = false;
        }
    }

    private float Jump(float jumpTime, float maxJumpTime, bool jumping) //TODO change this so it works
    {
        return jumpTime;
    }

    private bool IsGrounded()
    {
        Color rayColor;
        RaycastHit2D groundedRayCast = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.down, capsuleCollider.bounds.extents.y + .1f, LayerMask.GetMask("Ground"));
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
