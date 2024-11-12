using UnityEngine;
using Cinemachine;
 
public class FollowPlayerByTag : MonoBehaviour
{
    public string playerTag = "Player"; // Tag que a câmera irá procurar
    private CinemachineVirtualCamera cinemachineCamera;
 
    private void Start()
    {
        // Obtém a referência à câmera virtual do Cinemachine anexada ao GameObject
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
 
        if (cinemachineCamera != null)
        {
            // Procura um GameObject com a tag especificada
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
 
            if (player != null)
            {
                // Configura o Follow e/ou LookAt para o objeto encontrado
                cinemachineCamera.Follow = player.transform;
                cinemachineCamera.LookAt = player.transform;
            }
            else
            {
                Debug.LogWarning("Nenhum objeto com a tag '" + playerTag + "' foi encontrado.");
            }
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera não encontrada no GameObject.");
        }
    }
}
