using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class LanceManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool onGround;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(boxCollider, GameObject.FindGameObjectWithTag("GroundedLance").GetComponent<Collider2D>());
        onGround = false;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.down * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            Physics2D.IgnoreCollision(boxCollider, GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());
            for(int i=0;i<collision.gameObject.GetComponents<Collider2D>().Length;i++)
            {
                Physics2D.IgnoreCollision(boxCollider, GameObject.FindGameObjectWithTag("Enemy").GetComponents<Collider2D>()[i]);
            }
            //boxCollider.isTrigger = true;
        }
        if (collision.gameObject.CompareTag("Enemy")&& !onGround)
        {
            collision.gameObject.SendMessage("Hit");
            Destroy(this.gameObject);
        }
    }
}
