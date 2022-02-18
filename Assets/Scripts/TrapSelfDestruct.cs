using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSelfDestruct : MonoBehaviour
{
    public float selfDestructTime = 5;
    private float timer = 0f;

    private void Update()
    {
        if (GameManager.isPaused)
            return;


        timer += Time.deltaTime;
        if (timer > selfDestructTime)
        {
            --GodController.activeTraps;
            Destroy(gameObject);
        }
    }
}
