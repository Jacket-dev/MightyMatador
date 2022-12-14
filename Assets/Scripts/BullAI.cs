using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BullAI : MonoBehaviour
{

    private Rigidbody2D rb;
    private bool facingLeft;
    public float bullSpeed;
    public int bullHealth;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        facingLeft = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(facingLeft)
        {
            rb.velocity = Vector2.left * bullSpeed;
        }
        else rb.velocity = Vector2.right * bullSpeed;

        if(bullHealth<=0)
        {
            GameObject.FindGameObjectWithTag("GameController").SendMessage("Victory");
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            facingLeft = !facingLeft;
            transform.localScale = new Vector2(-transform.localScale.x,transform.localScale.y);

        }
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("Kill");
        }
    }
    private void Hit(int lanceDamage)
    {
        bullHealth -= lanceDamage;
    }
}
