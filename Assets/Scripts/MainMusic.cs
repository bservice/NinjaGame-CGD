using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusic : MonoBehaviour
{
    private AudioSource mainMusic;
    private MainMusic[] allPlayers;
    private void Awake()
    {
        //Only allow one audio player at a time
        allPlayers = FindObjectsOfType<MainMusic>();
        if (allPlayers.Length > 1)
        {
            Destroy(allPlayers[1]);
            Destroy(allPlayers[1].gameObject);
        }

        DontDestroyOnLoad(transform.gameObject);
        mainMusic = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (mainMusic.isPlaying) return;
        mainMusic.Play();
    }

    public void StopMusic()
    {
        mainMusic.Stop();
    }
}
