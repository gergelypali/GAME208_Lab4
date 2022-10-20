using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    Rigidbody rb;
    GameManager GM;

    // Used to control 'Character' movement
    float currentSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float jumpSpeed;

    float moveForward;
    float moveSide;

    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckDistance;

    public GameObject spawnMenu;

    private void Awake()
    {
        GM = GameManager.Instance;
    }

    // Start is called before the first frame update
    void Start()
    { 
        name = "Character";

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        if(walkSpeed <= 0)
        {
            walkSpeed = 5.0f;

            Debug.Log(name + ": walkSpeed not set. Defaulting to " + walkSpeed);
        }

        if (sprintSpeed <= 0)
        {
            sprintSpeed = 10.0f;

            Debug.Log(name + ": sprintSpeed not set. Defaulting to " + sprintSpeed);
        }

        if (jumpSpeed <= 0)
        {
            jumpSpeed = 8.0f;

            Debug.Log(name + ": jumpSpeed not set. Defaulting to " + jumpSpeed);
        }

        if (!groundCheck)
        {
            Debug.LogError(name + ": Missing groundCheck");
        }

        if (groundCheckDistance <= 0)
        {
            groundCheckDistance = 0.3f;

            Debug.Log(name + ": groundCheckDistance not set. Defaulting to " + groundCheckDistance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(groundCheck.position, -groundCheck.up, groundCheckDistance);
        Debug.DrawRay(groundCheck.position, -groundCheck.up * groundCheckDistance, Color.red);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (currentSpeed != sprintSpeed)
                currentSpeed = sprintSpeed;
        }
        else
            currentSpeed = walkSpeed;

        moveForward = Input.GetAxis("Vertical") * currentSpeed;
        moveSide = Input.GetAxis("Horizontal") * currentSpeed;

        if(isGrounded && Input.GetButtonDown("Jump"))
        {
            jump();
        }

        // with the key Q, player can go back to main menu
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Go back to MainMenu!");
            GM.SetGameState(GameState.MainMenu);
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Open SpawnMenu!");
            // let the cursor be visible so it can be used in the spawn menu
            Cursor.lockState = CursorLockMode.Confined;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            fire();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = (transform.forward * moveForward) + (transform.right * moveSide) +
            (transform.up * rb.velocity.y);
    }

    void fire()
    {
        Debug.Log("Firing from all cylinders!");
    }

    void jump()
    {
        Debug.Log("Jumping");

        rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
    }

    // Usage Rules:
    // - Both GameObjects in Scene need to have Colliders
    // - One or Both GameObjects need a Rigidbody
    // - Called once when collision starts
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter: " + collision.gameObject.name);
        Debug.Log("OnCollisionEnter: " + collision.gameObject.tag);
    }

    // Usage Rules:
    // - Both GameObjects in Scene need to have Colliders
    // - One or Both GameObjects need a Rigidbody
    // - Called as long as a collision happens
    void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay: " + collision.gameObject.name);
        Debug.Log("OnCollisionStay: " + collision.gameObject.tag);
    }

    // Usage Rules:
    // - Both GameObjects in Scene need to have Colliders
    // - One or Both GameObjects need a Rigidbody
    // - Called once when collision ends
    void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit: " + collision.gameObject.name);
        Debug.Log("OnCollisionExit: " + collision.gameObject.tag);
    }
}
