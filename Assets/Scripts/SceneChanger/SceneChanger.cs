using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Defina o ponto de teletransporte na nova cena
    public Vector2 teleportPosition = new Vector2(125.74f, -3.031f); // Posição padrão com sufixo 'f' para float

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D chamado!"); // Log para verificar se a colisão é detectada
        Debug.Log("Objeto colidido: " + other.name);

        // Verifica se o objeto colidido tem a tag "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("Mudando de fase!");

            // Salvar a posição de teletransporte antes de mudar a cena
            PlayerPrefs.SetFloat("TeleportPosX", teleportPosition.x);
            PlayerPrefs.SetFloat("TeleportPosY", teleportPosition.y);

            // Carregar a cena "Fase_2"
            SceneManager.LoadScene("Fase_2");
        }
        else
        {
            Debug.Log("O objeto não é o jogador.");
        }
    }
}
