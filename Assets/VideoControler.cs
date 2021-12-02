using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent (typeof(VideoPlayer))]
public class VideoControler : MonoBehaviour
{
    VideoPlayer player;
    bool started;
    
    // Start is called before the first frame update
    void Start()
    {
        player = (VideoPlayer)GetComponent(typeof(VideoPlayer));
        print(player.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            player.Play();
            started = true;
        }
        
        if(started && player.frame >= ((long)player.frameCount - 5))
        {
          this.gameObject.SetActive(false);
        }
    }
}
