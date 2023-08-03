using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed;   // Movement speed of the enemy.
    [SerializeField] private float stoppingDistance;   // Distance at which the enemy stops moving towards the player.

    private Transform player;   // Reference to the player's Transform component.
    private float pastPosition;
    public float waitTime;
    public Transform[] patrolDestination;
    private int currentPointIndex;
    private bool callOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;   // Assuming the player tag is set to "Player".
    }


    // Update is called once per frame
    void Update()
    {
        pastPosition = transform.position.x;

        //Set a fixed distance for enemy sense player
        if (Vector2.Distance(transform.position, player.position) < 12f)
        {
            //Chase player
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            //Start patrol if enemy are not sense player
            if(transform.position != patrolDestination[currentPointIndex].position) 
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolDestination[currentPointIndex].position, moveSpeed * Time.deltaTime);
            }
            else
            {
                if (callOnce == false)
                {
                    callOnce = true;
                    StartCoroutine(Wait());
                }
            }
        }
        CheckMoveDirection();
    }

    void CheckMoveDirection()
    {
        if(transform.position.x > pastPosition)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (transform.position.x < pastPosition) 
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        
        currentPointIndex++;
        if (currentPointIndex >= patrolDestination.Length)
        {
            currentPointIndex = 0;
        }
        callOnce = false;
    }
}