using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformManager : MonoBehaviour
{
    private Vector3 homePosition;
    private Vector3 targetPosition;
    public Vector3 endPosition;
    private float speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        homePosition = gameObject.transform.position;
        targetPosition = endPosition;
    }

    void FixedUpdate()
    {
        if (gameObject.transform.position == homePosition)
        {
            targetPosition = endPosition;
        }
        else if (gameObject.transform.position == endPosition)
        {
            targetPosition = homePosition;
        }

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}
