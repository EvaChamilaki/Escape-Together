using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class OutroToIntro : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private bool hasStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(videoPlayer.isPlaying && !hasStarted)
        {
            hasStarted = true;
        }

        if(hasStarted && !videoPlayer.isPlaying)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
