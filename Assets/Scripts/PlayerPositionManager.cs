using UnityEngine;

public class PlayerPositionManager : MonoBehaviour
{
    // Variáveis para as coordenadas X e Y
    private float teleportPosX;
    private float teleportPosY;

    private void Start()
    {
        // Verificar se as coordenadas de teletransporte estão salvas
        if (PlayerPrefs.HasKey("TeleportPosX") && PlayerPrefs.HasKey("TeleportPosY"))
        {
            // Carregar as coordenadas salvas no PlayerPrefs
            teleportPosX = PlayerPrefs.GetFloat("TeleportPosX");
            teleportPosY = PlayerPrefs.GetFloat("TeleportPosY");

            // Posicionar o jogador na nova posição
            transform.position = new Vector2(teleportPosX, teleportPosY);
        }
        else
        {
            // Caso não haja coordenadas salvas, podemos definir uma posição padrão
            transform.position = new Vector2(0, 0); // Posição inicial padrão
        }
    }
}
