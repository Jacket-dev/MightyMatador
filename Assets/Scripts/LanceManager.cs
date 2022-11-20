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
    public int lanceDamage;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        onGround = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!onGround)
        {
            rb.velocity = Vector2.down * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            rb.bodyType = RigidbodyType2D.Static;
            boxCollider.isTrigger = true;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.SendMessage("Hit",lanceDamage);
            GameObject.FindGameObjectWithTag("Player").SendMessage("AddLance"); //To do better, Unity is screaming about this
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("AddLance");
            Destroy(this.gameObject);
        }
    }
}
