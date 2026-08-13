using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private Animator anim;
    private Rigidbody2D rb;

    public AudioSource flapSound;

    [Space(20)]

    public float flapForce;
    public float gravity;
    public float rotationSpeed;
    public float minRotation;
    public float maxRotation;

    private bool isDead;
    private float rotation;
    private Quaternion newRot;

    // Start is called before the first frame update
    void Start()
    {
        //initializing stuff
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetInteger("birdType", Random.Range(0, 3));
        isDead = false;
        rb.gravityScale = 0;
        newRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameStarted)
        {
            //flapping on mouse click
            if (Input.GetMouseButtonDown(0) && !isDead)
            {
                Flap();
            }

            //handling bird rotation
            rotation -= rotationSpeed * rb.linearVelocity.magnitude * Time.deltaTime;
            if (rotation < minRotation)
            {
                rotation = minRotation;
            }
            if (rotation > maxRotation)
            {
                rotation = maxRotation;
            }
            newRot.eulerAngles = new Vector3(0, 0, rotation);
            transform.rotation = newRot;
        }
    }

    void Flap()
    {
        flapSound.Play();
        rb.linearVelocity = Vector2.up * flapForce;
        rotation = maxRotation;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            //dying on touching ground
            if (!isDead)
            {
                Die();
            }
            rotation = minRotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Point"))
        {
            //gaining a point
            gameManager.UpdateScore(1);
            other.enabled = false;
        }
        else if (other.gameObject.CompareTag("Pipes") && !isDead)
        {
            //dying on touching pipes
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        anim.speed = 0;
        gameManager.GameOver();
    }

    public void StartGame()
    {
        rb.gravityScale = gravity;
        Flap();
    }
}
