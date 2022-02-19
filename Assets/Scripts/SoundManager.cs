using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip jump, landing, load, save;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        jump = Resources.Load<AudioClip>("jump");
        landing = Resources.Load<AudioClip>("landing");
        save = Resources.Load<AudioClip>("save");
        load = Resources.Load<AudioClip>("load");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "jump":
                audioSrc.PlayOneShot(jump);
                break;
            case "landing":
                audioSrc.PlayOneShot(landing);
                break;
            case "save":
                audioSrc.PlayOneShot(save);
                break;
            case "load":
                audioSrc.PlayOneShot(load);
                break;
        }
    }

}
