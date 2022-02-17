using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherManager : MonoBehaviour
{
    private Vector3 homePosition;
    private Vector3 targetPosition;
    public Vector3 downPosition;
    private float speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        homePosition = gameObject.transform.position;
        targetPosition = homePosition;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetPosition = downPosition;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetPosition = homePosition;
        }
    }
}
