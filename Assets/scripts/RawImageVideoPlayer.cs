using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class RawImageVideoPlayer : MonoBehaviour
{
    public RawImage scanAni;
    public VideoPlayer VideoPlayer;
    public void Start()
    {
        VideoPlayer.enabled = false;
        VideoPlayer.prepareCompleted += PrepareCompleted;
    }
    public void Update()
    {
        if (!VideoPlayer.enabled) {
            VideoPlayer.enabled = true;
            VideoPlayer.Play();
        }
    }
    private void PrepareCompleted(VideoPlayer player)
    {
        scanAni.texture = player.texture;
    }
}
