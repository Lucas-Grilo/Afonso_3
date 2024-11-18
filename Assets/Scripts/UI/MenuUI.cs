using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("Menu UI properties")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        // Adiciona os listeners aos botões
        startButton.onClick.AddListener(GoToGameplayScene);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void GoToGameplayScene()
    {
        // Carregar a cena do jogo
        SceneManager.LoadScene("Fase_1"); // Nome da cena de gameplay
    }

    private void ExitGame()
    {
        // Sair do jogo
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // Este método pode ser chamado quando o jogador morrer e a cena de menu for carregada
    public void ShowMenuAfterDeath()
    {
        gameObject.SetActive(true); // Ativa o painel do menu
    }
}
