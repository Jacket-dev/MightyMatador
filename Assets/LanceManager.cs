using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.down * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            rb.Sleep();
            boxCollider.isTrigger = true;
        }
    }
}
