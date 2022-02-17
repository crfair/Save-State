using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject count;
    [SerializeField] GameObject icon;
    public float colorCycleSpeed = 0.5f;

    Color defaultColor;
    
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        defaultColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        count.GetComponent<Text>().text = player.GetComponent<PlayerCharacter>().GetLives() + " x";
        if (player.GetComponent<PlayerCharacter>().HasPowerUp())
        {
            icon.GetComponent<Image>().material.color += new Color(colorCycleSpeed, colorCycleSpeed, colorCycleSpeed, 0);
        }
        else 
        {
            icon.GetComponent<Image>().material.color = defaultColor;
        }
    }
}
