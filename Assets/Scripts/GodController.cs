using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodController : MonoBehaviour
{
    public GameObject teleportPoint;
    public Camera mainCamera;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            Vector3 spawnPoint = mainCamera.ScreenToWorldPoint(mousePos);
            Instantiate(teleportPoint,spawnPoint,Quaternion.identity);
        }
    }
}
