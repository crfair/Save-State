using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject count;
    [SerializeField] GameObject icon;

    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        count.GetComponent<Text>().text = player.GetComponent<PlayerCharacter>().GetLives() + " x"; 
    }
}
