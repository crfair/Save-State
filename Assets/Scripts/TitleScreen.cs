using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject optionsScreen;

    public void PlayButton() 
    {
        SceneManager.LoadScene(1);
    }

    public void OptionsButton() 
    {
        titleScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }

    public void BackButton() 
    {
        optionsScreen.SetActive(false);
        titleScreen.SetActive(true);
    }
}
