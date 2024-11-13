using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoUIController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage rawImage;

    void Start()
    {
        // Certifique-se de que o VideoPlayer e RawImage estão atribuídos no Inspector
        if (videoPlayer && rawImage)
        {
            videoPlayer.Play(); // Reproduz o vídeo
        }
    }
}
