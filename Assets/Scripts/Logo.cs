using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += GoToMenuScene;
    }

    void GoToMenuScene(VideoPlayer vp)
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
