using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public Rigidbody playerRB;
    public Rigidbody enemy;
    public Light playerLight;
    public SpriteRenderer spriteRenderer;
    public GameObject childSprite;
    public SpriteRenderer childRenderer;

    public float agroDistance = 10f;
    private bool isAgro = false;

    private Vector3 lookVect;

    public float moveSpeed = 1.5f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponentInChildren<Rigidbody>();
        playerLight = player.GetComponentInChildren<Light>();

        enemy = this.GetComponent<Rigidbody>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        childRenderer = childSprite.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (playerLight.enabled && (playerRB.position - enemy.position).magnitude <= playerLight.range)
        {
            spriteRenderer.enabled = true;
            childRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
            childRenderer.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        //if player is within agro distance and light is on
        if ((playerRB.position - enemy.position).magnitude <= agroDistance && playerLight.enabled)
        {
            isAgro = true;
        }

        EnemyLook();

        if (isAgro)
            EnemyMove();
    }

    //controls the direction that the enemy is facing
    private void EnemyLook()
    {
        //vector subtraction gets the player position relative to enemy's
        lookVect = (playerRB.position - enemy.position).normalized;

        enemy.transform.forward = new Vector3(lookVect.x, 0, lookVect.z);
    }

    private void EnemyMove()
    {
        enemy.transform.position += lookVect * moveSpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("You've got got");
        }
    }

    private void OnDestroy()
    {
        moveSpeed = 0;
        Debug.Log("Death animation plays");
    }
}

