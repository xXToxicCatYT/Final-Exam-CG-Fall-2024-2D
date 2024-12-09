using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarioController : MonoBehaviour
{
    [SerializeField] float runForce;
    [SerializeField] float jumpForce;
    [SerializeField] float maxSpeed;

    Transform trans;
    Rigidbody2D body;

    float runInput;
    bool jumpInput;

    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        body = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        runInput = Input.GetAxis("Horizontal");


        if (Input.GetKey(KeyCode.W))
        {
            jumpInput = true;
        }
        else
        {
            jumpInput = false;
        }

        if (runInput == 0 && body.velocity.y == 0)
        {
            body.drag = 3;
        }
        else
        {
            body.drag = 1;
        }
    }

    void FixedUpdate()
    {
        if (runInput != 0)
        {
            Run();
        }

        if (jumpInput && isGrounded)
        {
            Jump();
        }
    }

    void Run()
    {
        if (Mathf.Abs(body.velocity.x) >= maxSpeed)
        {
            return;
        }
        if (runInput > 0)
        {
            body.AddForce(Vector2.right * runForce, ForceMode2D.Force);
        }
        if (runInput < 0)
        {
            body.AddForce(Vector2.left * runForce, ForceMode2D.Force);
        }
    }

    void Jump()
    {
        body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (collision.contacts[i].normal.y > 0.5)
            {
                isGrounded = true;
            }
        }

        if (collision.gameObject.name.Contains("Mushroom"))
        {
            Destroy(collision.gameObject);
        }
    }
}
