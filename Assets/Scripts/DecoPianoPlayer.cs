using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoPianoPlayer : MonoBehaviour
{
    private uint music_index = 0;
    private uint prev_music_index = 0;

    private float timer_start, timer_val;
    private float delay = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        music_index = 0;
    }

    public bool IsPlaying()
    { return GetComponents<AudioSource>()[prev_music_index].isPlaying; }

    public void PlayPiano()
    {
        if (GetComponents<AudioSource>()[prev_music_index].isPlaying)
            return;

        GetComponents<AudioSource>()[music_index].Play();
        prev_music_index = music_index;
        music_index = (music_index == 4) ? 0 : music_index + 1;
    }
}
