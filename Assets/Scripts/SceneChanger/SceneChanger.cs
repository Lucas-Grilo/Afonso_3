using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other)
{
    Debug.Log("OnTriggerEnter2D chamado!"); // Log para verificar se a colisão é detectada
    Debug.Log("Objeto colidido: " + other.name);

    if (other.CompareTag("Player"))
    {
        Debug.Log("Mudando de fase!");
        SceneManager.LoadScene("Fase_2");
    }
    else
    {
        Debug.Log("O objeto não é o jogador.");
    }
}
}



