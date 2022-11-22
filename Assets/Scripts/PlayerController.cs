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

    private SpriteRenderer spriteRenderer;
    private Sprite[] spriteList;

    private bool canShoot;
    private bool jumping;
    private bool alive;
    private int ammoLeft;
    private float jumpTime;

    private float actualLanceCooldown;

    
    public GameObject lance;
    public int maxAmmo;
    public float moveSpeed;
    public float jumpSpeed;
    public float maxJumpTime;

    public float lanceCooldown;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        jumpTime = 0;
        jumping= false;
        canShoot = false;
        ammoLeft = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        
        rb.velocity = Move();


        //Jump Handle
        if (IsGrounded())
        {
            jumpTime = 0;
        }
        if(Input.GetAxis("Jump") != 0)
        {
            jumping = true;
        }
        if(jumpTime>maxJumpTime || Input.GetAxis("Jump")==0)
        {
            jumping = false;
        }
        if(jumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, Input.GetAxis("Jump") * jumpSpeed);
            jumpTime += Time.deltaTime;
            canShoot = false;
        }


        //Projectile Handle
        actualLanceCooldown -= Time.deltaTime;
        if(Input.GetAxis("Jump") != 0 && canShoot && ammoLeft>0) //Will see some changes in the future
        {
            if(actualLanceCooldown <=0)
            {
                Instantiate(lance,this.transform.position, new Quaternion(0,0,0,1));
                ammoLeft--;
                canShoot = true;
                actualLanceCooldown = lanceCooldown;
            }
        }
        if(Input.GetAxis("Jump") == 0) //Doublon avec un if dans le saut
        {
            canShoot = true;
        }

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
            return true;
        }
        else
        {
            rayColor = Color.red;
            Debug.DrawRay(capsuleCollider.bounds.center, Vector2.down * (capsuleCollider.bounds.extents.y + 0.1f), rayColor); //Debugging stuff
            return false;
        }
    }
    
    private Vector2 Move()
    {
        RaycastHit2D leftRayCast = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.left, capsuleCollider.bounds.extents.x + .1f, LayerMask.GetMask("Ground"));
        RaycastHit2D rightRayCast = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.right, capsuleCollider.bounds.extents.x + .1f, LayerMask.GetMask("Ground"));
        Vector2 result;
        if (leftRayCast.collider != null && Input.GetAxis("Horizontal") < 0)
        {
            result = new Vector2(0, rb.velocity.y);
        }
        else if (rightRayCast.collider != null && Input.GetAxis("Horizontal") > 0)
        {
            result = new Vector2(0, rb.velocity.y);
        }
        else
        {
            result = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y);
        }
        return result;
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
