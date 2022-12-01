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
    private AudioSource audioOnPickupAmmo;
    private Animator animator;

    private SpriteRenderer spriteRenderer;

    private bool canShoot;
    private bool jumping;
    private bool alive;
    private int ammoLeft;
    private float jumpTime; //Kinda unused
    public float jumpSpeed;

    private float actualLanceCooldown;

    public GameObject lance;
    public int maxAmmo;
    public float moveSpeed;
    public float maxJumpTime; //Actually set at 0, kinda unused
    public float jumpHeight;
    public float gravityScale;
    public float jumpDuration; //Testing stuff
    public float lanceCooldown;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        jumpTime = 0;
        jumping= false;
        canShoot = false;
        ammoLeft = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        //Another way to do the jump, with more control over height but no variable height jump
        /*
        jumpSpeed = Mathf.Sqrt(Mathf.Abs(2 * Physics2D.gravity.y *jumpHeight));
        rb.gravityScale = Mathf.Sqrt(Mathf.Abs(2 * Physics2D.gravity.y * jumpHeight)) / (jumpDuration+maxJumpTime); //Testing stuff
        jumpSpeed *= Mathf.Sqrt(rb.gravityScale); //Will move to Start() when finished Debugging if we keep it
        */
        Move();
        Jump();
        Shoot();
        //Other stuff


    }


    //Checks if player is touching ground by raycasting from the player toward the ground. If raycast detects a collision it returns false
    //Raycast only checks collision with "Ground" Layer
    private bool IsGrounded()
    {
        Color rayColor; //Debugging stuff
        RaycastHit2D groundedRayCast = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.down, capsuleCollider.bounds.extents.y + .1f, LayerMask.GetMask("Ground"));
        if(groundedRayCast.collider!=null)
        {
            rayColor = Color.green;
            Debug.DrawRay(capsuleCollider.bounds.center, Vector2.down * (capsuleCollider.bounds.extents.y + 0.1f), rayColor); //Debugging stuff
            animator.SetBool("Grounded",true);
            return true;
        }
        else
        {
            rayColor = Color.red;
            Debug.DrawRay(capsuleCollider.bounds.center, Vector2.down * (capsuleCollider.bounds.extents.y + 0.1f), rayColor); //Debugging stuff
            animator.SetBool("Grounded", false);
            return false;
        }
    }
    
    private void Shoot()
    {
        actualLanceCooldown -= Time.deltaTime;
        if (Input.GetAxis("Jump") != 0 && canShoot && ammoLeft > 0)
        {
            if (actualLanceCooldown <= 0)
            {
                GameObject projectile = Instantiate(lance, this.transform.position, new Quaternion(0, 0, 0, 1));
                projectile.GetComponent<Rigidbody2D>().gravityScale = rb.gravityScale;
                ammoLeft--;
                canShoot = false; //This decides if player needs to keep pressing button to shoot or not
                actualLanceCooldown = lanceCooldown;
            }
        }
        if (Input.GetAxis("Jump") == 0) //Doublon avec un if dans le saut
        {
            canShoot = true;
        }
    }

    private void Jump()
    {
        jumpTime += Time.deltaTime;
        if (IsGrounded())
        {
            jumpTime = 0;
        }

        if (Input.GetAxis("Jump") != 0)
        {
            jumping = true;
        }
        if (jumpTime > maxJumpTime || Input.GetAxis("Jump") == 0)
        {
            jumping = false;
        }
        if (jumping)
        {
            animator.SetTrigger("Jump");
            rb.velocity = new Vector2(rb.velocity.x, Input.GetAxis("Jump") * jumpSpeed);
            canShoot = false;
        }
    }
    private void JumpV2()
    {

        jumpSpeed = Mathf.Sqrt(Mathf.Abs(2 * Physics2D.gravity.y * jumpHeight));
        rb.gravityScale = Mathf.Sqrt(Mathf.Abs(2 * Physics2D.gravity.y * jumpHeight)) / (jumpDuration + maxJumpTime); //Testing stuff
        jumpSpeed *= Mathf.Sqrt(rb.gravityScale); //Will move to Start() when finished Debugging if we keep it


        jumpTime += Time.deltaTime;
        if (IsGrounded())
        {
            jumpTime = 0;
        }

        if (Input.GetAxis("Jump") != 0)
        {
            jumping = true;
        }
        if (jumpTime > maxJumpTime || Input.GetAxis("Jump") == 0)
        {
            jumping = false;
        }
        if (jumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, Input.GetAxis("Jump") * jumpSpeed);
            canShoot = false;
        }
    }


    //IDLE Animation activates when switching direction TODO Change this
    private void Move()
    {
        RaycastHit2D leftRayCast = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.left, capsuleCollider.bounds.extents.x + .1f, LayerMask.GetMask("Ground"));
        RaycastHit2D rightRayCast = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.right, capsuleCollider.bounds.extents.x + .1f, LayerMask.GetMask("Ground"));
        if (leftRayCast.collider != null && Input.GetAxis("Horizontal") < 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            transform.localScale = new Vector3(-.67f, transform.localScale.y, transform.localScale.z);
            animator.SetBool("Moving",true);
        }
        else if (rightRayCast.collider != null && Input.GetAxis("Horizontal") > 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            transform.localScale = new Vector3(.67f, transform.localScale.y, transform.localScale.z);
            animator.SetBool("Moving", true);
        }
        else
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y);
            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-.67f, transform.localScale.y, transform.localScale.z);

            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                transform.localScale = new Vector3(.67f, transform.localScale.y, transform.localScale.z);
            }
            Debug.Log(Input.GetAxis("Horizontal"));
            animator.SetBool("Moving", Input.GetAxis("Horizontal") != 0 && rb.velocity!=Vector2.zero);
        }
    }
    private void Kill()
    {
        GameObject.FindGameObjectWithTag("GameController").SendMessage("Defeat");
        Destroy(this.gameObject);
    }
    private void AddLance()
    {
        ammoLeft++;
        switch (ammoLeft)
        {
            case 1:
                audioOnPickupAmmo.pitch = 1f;
                audioOnPickupAmmo.Play();
                break;
            case 2:
                audioOnPickupAmmo.pitch = 1.5f;
                audioOnPickupAmmo.Play();
                break;
            case 3:
                audioOnPickupAmmo.pitch = 2f;
                audioOnPickupAmmo.Play();
                break;
        }
    }
}
