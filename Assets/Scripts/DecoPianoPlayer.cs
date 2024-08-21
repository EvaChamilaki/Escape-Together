using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoPianoPlayer : MonoBehaviour
{
    public AudioSource main_music_src;

    private uint music_index = 0;
    private uint prev_music_index = 0;

    private float timer_start, timer_val;
    private float delay = 2.0f;

    private bool has_started = false;

    // Start is called before the first frame update
    void Start()
    {
        music_index = 0;
        has_started = false;
    }

    public bool IsPlaying()
    { return GetComponents<AudioSource>()[prev_music_index].isPlaying; }

    public void PlayPiano()
    {
        if (GetComponents<AudioSource>()[prev_music_index].isPlaying)
            return;

        main_music_src.volume = 0.05f;
        has_started = true;

        GetComponents<AudioSource>()[music_index].Play();
        prev_music_index = music_index;
        music_index = (music_index == 4) ? 0 : music_index + 1;
    }

    void Update()
    {
        if (has_started && !GetComponents<AudioSource>()[prev_music_index].isPlaying)
        {
            main_music_src.volume = 0.1f;
            has_started = false;
        }
    }
}
