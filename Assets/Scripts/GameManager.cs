using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isPaused = false;

    [Header("Pause Menu Stuff")]
    public GameObject pauseMenuPanel;
    
    [Header("Save Stuff")]
    public GameObject player1;
    public float autoSaveInterval = 30f;
    public GameObject powerUps;
    private float autoSaveTicker = 0f;

    private void Start()
    {
        this.Play();

        PlayerCharacter charController = player1.GetComponent<PlayerCharacter>();

        //player 1 position
        player1.transform.position = new Vector3(PlayerPrefs.GetFloat("posX",player1.transform.position.x), PlayerPrefs.GetFloat("posY", player1.transform.position.y), PlayerPrefs.GetFloat("posZ", player1.transform.position.z));

        //lives
        charController.lives = PlayerPrefs.GetInt("Lives",charController.lives);

        //powerups
        if (PlayerPrefs.GetInt("PowerUps",0) == 1)
            charController.hasPowerUp = true;
        else
            charController.hasPowerUp = false;

        //respawn point
        charController.respawnPoint = new Vector3(PlayerPrefs.GetFloat("resX", player1.transform.position.x), PlayerPrefs.GetFloat("resY", player1.transform.position.y), PlayerPrefs.GetFloat("resZ", player1.transform.position.z));

        //reset number of traps
        GodController.activeTraps = 0;

        //enable or disable powerups
        int children = powerUps.transform.childCount;
        for (int i = 0; i < children; i++)
        {
            if (PlayerPrefs.GetInt("PowerUp" + (i + 1).ToString(), 1) == 0)
            {
                powerUps.transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                powerUps.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Play();
            else
                Pause();
        }

        //auto saving
        autoSaveTicker += Time.deltaTime;
        if(autoSaveTicker>autoSaveInterval)
        {
            this.OnSave();
            autoSaveTicker = 0f;
        }
    }
    public void Play()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void Menu()
    {
        //load title scene
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        this.OnSave();
        Application.Quit();
    }

    public void OnSave()
    {
        PlayerCharacter charController = player1.GetComponent<PlayerCharacter>();
        
        //player 1 position
        PlayerPrefs.SetFloat("posX", player1.transform.position.x);
        PlayerPrefs.SetFloat("posY", player1.transform.position.y);
        PlayerPrefs.SetFloat("posZ", player1.transform.position.z);

        //lives
        PlayerPrefs.SetInt("Lives", charController.lives);
        
        //powerups
        if(charController.hasPowerUp)
            PlayerPrefs.SetInt("PowerUps",1);
        else
            PlayerPrefs.SetInt("PowerUps", 0);

        //respawn point
        PlayerPrefs.SetFloat("resX", charController.respawnPoint.x);
        PlayerPrefs.SetFloat("resY", charController.respawnPoint.y);
        PlayerPrefs.SetFloat("resZ", charController.respawnPoint.z);

        //powerups active
        int children = powerUps.transform.childCount;
        for (int i = 0; i < children; i++)
        {
            if (powerUps.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("PowerUp" + (i + 1).ToString(), 1);
            }
            else
            {
                PlayerPrefs.SetInt("PowerUp" + (i + 1).ToString(), 0);
            }
        }
    }
    public void OnLoad()
    {
        //reload scene with saved player prefs
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClear()
    {
        //clear save
        PlayerPrefs.DeleteAll();
    }

}
