using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodController : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject playerObject;
    private GameObject currentTeleportPoint = null;
    private Coroutine activeRoutineController = null;
    private Coroutine cooldownRoutineController = null; 
    private bool canTeleport = true; //to enable or disable teleportation

    [Header("Teleportation Info")]
    public GameObject teleportPointPrefab;
    public float teleportationRadius = 10; 
    public float teleportationCooldown = 10;
    public string platformTag = "Platform";

    private void Start()
    {
        mainCamera = Camera.main;
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    //destroys teleportation point after player goes past the teleportation radius
    IEnumerator teleportActiveRoutine(Vector3 spawnPoint)
    {
        currentTeleportPoint = Instantiate(teleportPointPrefab, spawnPoint, Quaternion.identity);
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (!playerInTeleportationRadius())
            {
                break;
            }
        }
        ResetTeleportation();
        activeRoutineController = null;
    }
    
    //cooldown after a succesful teleportation
    IEnumerator teleportCooldownRoutine()
    {
        canTeleport = false;
        yield return new WaitForSeconds(teleportationCooldown);
        canTeleport = true;
    }
    private void Update()
    {
        //left click to place teleportation device, left click again to teleport
        if(Input.GetMouseButtonDown(0) && canTeleport)
        {
            //if teleportation point does not exist, drop one
            if (!currentTeleportPoint)
            {
                Vector3 mousePos = Input.mousePosition;
                if (!clickedOnGround(mousePos))
                {
                    mousePos.z = 10;
                    Vector3 spawnPoint = mainCamera.ScreenToWorldPoint(mousePos);
                    activeRoutineController = StartCoroutine(teleportActiveRoutine(spawnPoint));
                }
            }
            //if teleportation point exists, and player in radius, teleport player
            else if(playerInTeleportationRadius())
            {
                playerObject.transform.position = currentTeleportPoint.transform.position;
                ResetTeleportation();
                ResetActiveCoroutine();
                cooldownRoutineController = StartCoroutine(teleportCooldownRoutine());
            }
        }
    }
    
    private bool playerInTeleportationRadius()
    {
        //if no teleport point exists
        if (!currentTeleportPoint)
            return false;

        //if within radius
        if (Vector3.SqrMagnitude(playerObject.transform.position - currentTeleportPoint.transform.position)
                                                            < teleportationRadius * teleportationRadius)
            return true;

        //if outside radius
        return false;
    }

    private bool clickedOnGround(Vector3 mousePos)
    {
        RaycastHit rayCastHit;
        
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        Debug.Log(ray.origin);
        Debug.Log(ray.direction);
        Debug.DrawRay(ray.origin, ray.direction, Color.red,100,false);

        if (Physics.Raycast(mainCamera.ScreenPointToRay(mousePos), out rayCastHit, 1000f) && rayCastHit.collider.CompareTag(platformTag))
            return true;
        return false;
    }

    //reset the half done teleportation
    private void ResetTeleportation()
    {
        if (currentTeleportPoint)
        {
            Destroy(currentTeleportPoint);
            currentTeleportPoint = null;
        }
    }
    //reset the routine tracking the teleportation
    private void ResetActiveCoroutine()
    {
        if (activeRoutineController != null)
        {
            StopCoroutine(activeRoutineController);
            activeRoutineController = null;
        }
    }
}
