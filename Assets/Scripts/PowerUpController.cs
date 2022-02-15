using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    Vector2 floatY;
    float originalY;
    public float floatStrength = 0.25f;
    public PlayerCharacter player;

    // Start is called before the first frame update
    void Start()
    {
        originalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        floatY = transform.position;
        floatY.y = originalY + (Mathf.Sin(Time.time) * floatStrength);
        transform.position = floatY;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.AddLives(1);
            player.SetPowerUp(true);
            Debug.Log("Player lives increased to " + player.GetLives());
            gameObject.SetActive(false);
        }
    }
}
