using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer; // arraste o VideoPlayer para este campo no Inspector
    public string playerTag = "Player"; // Tag do jogador para detectar colisão

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Inicia o vídeo se o jogador colidir com o objeto
            videoPlayer.Play();
        }
    }
}
