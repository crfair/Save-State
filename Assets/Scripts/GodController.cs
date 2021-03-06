using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodController : MonoBehaviour,PlayPause
{
    bool player2Paused = false;

    private Camera mainCamera;
    private GameObject playerObject;

    private GameObject currentTeleportPoint = null;    

    private Coroutine activeRoutineController = null;
    private Coroutine cooldownRoutineController = null;
    
    private bool canTeleport = true; //to enable or disable teleportation
    private bool canSetTrap = true; //to enable or disable trap setting

    public static int activeTraps = 0; //tracks active traps at the moment

    //variables to check ground collision
    Collider2D[] colliders;
    ContactFilter2D contactFilter;

    [Header("Teleportation Info")]
    public GameObject teleportPointPrefab;
    public float teleportationRadius = 10; 
    public float teleportationCooldown = 5;
    public string platformTag = "Platform";

    [Header("Trap Information")]
    public GameObject trapPrefab;
    public string playerBoundaryTag = "TheForce";
    public int maxAllowedActiveTraps = 2;

    private void Awake() 
    {
        mainCamera = Camera.main;
        playerObject = GameObject.FindGameObjectWithTag("Player");

        colliders = new Collider2D[5];
        contactFilter = new ContactFilter2D();
        contactFilter = contactFilter.NoFilter();
    }

    private void Update()
    {

        if (GameManager.isPaused && !player2Paused)
            Pause();
        else if (!GameManager.isPaused && player2Paused)
            Play();

        if (player2Paused)
            return;

        //left click to place teleportation device, left click again to teleport
        if (Input.GetMouseButtonDown(0) && canTeleport)
        {
            //if teleportation point does not exist, drop one
            if (!currentTeleportPoint)
            {
                SoundManager.PlaySound("save");
                Vector3 spawnPoint = getMousePosInWorldSpace();
                if (!clickedOnGround(spawnPoint))
                    activeRoutineController = StartCoroutine(teleportActiveRoutine(spawnPoint));
            }
            //if teleportation point exists, and player in radius, teleport player
            else if(playerInTeleportationRadius())
            {
                SoundManager.PlaySound("load");
                playerObject.transform.position = currentTeleportPoint.transform.position;
                ResetTeleportation();
                ResetActiveCoroutine();
                cooldownRoutineController = StartCoroutine(teleportCooldownRoutine());
            }
        }

        if(Input.GetMouseButtonDown(1) && activeTraps<maxAllowedActiveTraps && canSetTrap)
        {
            Vector3 spawnPoint = getMousePosInWorldSpace();
            if(!clickedOnGround(spawnPoint) && !clickedOnPlayerForceField(spawnPoint))
            {
                Instantiate(trapPrefab, spawnPoint, Quaternion.identity);
                ++activeTraps;
            }
        }
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

    private Vector3 getMousePosInWorldSpace()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.localPosition.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    private bool clickedOnGround(Vector3 spawnPoint)
    {
        int results = Physics2D.OverlapPoint(spawnPoint, contactFilter, colliders);
        for(int i = 0;i<results;i++)
        {
            if (colliders[i].tag == platformTag)
                return true;
        }
        return false;
    }
    
    private bool clickedOnPlayerForceField(Vector3 clickPoint)
    {
        int results = Physics2D.OverlapPoint(clickPoint, contactFilter, colliders);
        for (int i = 0; i < results; i++)
        {
            if (colliders[i].tag == playerBoundaryTag)
                return true;
        }
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

    public void Play()
    {
        canSetTrap = true;
        canTeleport = true;
        player2Paused = false;
    }

    public void Pause()
    {
        canSetTrap = false;
        canTeleport = false;
        player2Paused = true;

    }
}
