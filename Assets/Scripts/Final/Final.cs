using UnityEngine;
using UnityEngine.InputSystem;  // Necessário para usar o Input System
using UnityEngine.SceneManagement;

public class Final : MonoBehaviour
{
    public GameObject imagemUI;
    private bool isPaused = false;

    // Método chamado quando o player entra na área do trigger
    private void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            imagemUI.SetActive(true);
            Time.timeScale = 0f;  // Pausa o jogo
            isPaused = true;      // Marca o jogo como pausado
        }
    }

    // Usando o novo sistema de entrada para detectar o clique
    private void Update()
    {
        if (isPaused && Mouse.current.leftButton.wasPressedThisFrame)  // Verifica se o botão esquerdo do mouse foi pressionado
        {
            Time.timeScale = 1f;  // Retorna o jogo ao seu ritmo normal
            SceneManager.LoadScene("Menu");  // Carrega a cena chamada "Menu"
        }
    }
}
