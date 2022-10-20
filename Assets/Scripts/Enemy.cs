using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
// main class for the enemy
public class Enemy : MonoBehaviour
{
    public Rigidbody rb;

    // these are used during instantiation
    public int spawnTime;
    public float yOffset;

    // new variable to store the max number this enemy can be in the spawn queue
    public int maxQueue;

    public virtual void Attack()
    {
        //Base Method for attacking
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // check for input parameters
        if (spawnTime <= 0)
        {
            Debug.Log("Using default spawnTime; 1 sec!");
            spawnTime = 1;
        }
        if (yOffset <= 0)
        {
            Debug.Log("Using default yOffset; 0.5f!");
            yOffset = 0.5f;
        }
        if (maxQueue < 0)
        {
            Debug.Log("Using default maxQueue; 0!");
            maxQueue = 0;
        }
    }
    void Update()
    {
        // use key R to despawn every enemy from the scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Despawn enemy!");
            DestroyEnemy();
        }
        // use key E to attack with every enemy on the scene
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Enemy attack!");
            Attack();
        }
    }

}
