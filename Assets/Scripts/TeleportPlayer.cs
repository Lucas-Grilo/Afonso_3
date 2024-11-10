using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    void Start()
    {
        // Verifique se a posição de teletransporte foi salva
        if (PlayerPrefs.HasKey("TeleportPosX") && PlayerPrefs.HasKey("TeleportPosY"))
        {
            float x = PlayerPrefs.GetFloat("TeleportPosX");
            float y = PlayerPrefs.GetFloat("TeleportPosY");

            // Defina a posição do jogador com base nos valores salvos
            transform.position = new Vector2(x, y);

            // Limpe os valores após o teletransporte para evitar que o jogador seja teleportado novamente sem querer
            PlayerPrefs.DeleteKey("TeleportPosX");
            PlayerPrefs.DeleteKey("TeleportPosY");
        }
        else
        {
            Debug.LogWarning("Posição de teletransporte não encontrada!");
        }
    }
}
