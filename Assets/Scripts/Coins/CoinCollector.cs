using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public int currentCoinCount = 0; // Contador de moedas coletadas
    public TextMeshProUGUI coinText; // Referência ao componente TMP
    public AudioClip coinSound; // Som da coleta da moeda
    private AudioSource audioSource; // Referência ao AudioSource do jogador

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Obtém o AudioSource do objeto jogador (Player)
        UpdateCoinText(); // Atualiza o texto no início
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto colidido é uma moeda com a camada "Moeda"
        if (other.gameObject.layer == LayerMask.NameToLayer("Moeda"))
        {
            Debug.Log("Moeda coletada!"); // Log para verificar a coleta

            // Reproduz o som da coleta
            if (audioSource != null && coinSound != null)
            {
                audioSource.PlayOneShot(coinSound); // Toca o som da coleta
            }

            // Incrementa a contagem de moedas
            currentCoinCount++; 
            UpdateCoinText(); // Atualiza o texto da quantidade de moedas

            // Destrói a moeda imediatamente após tocar o som
            Destroy(other.gameObject); // Destrói a moeda imediatamente
        }
        else
        {
            Debug.Log("A colisão não foi com um objeto na camada 'Moeda'.");
        }
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Moedas: " + currentCoinCount; // Atualiza o texto no componente TMP
        }
        else
        {
            Debug.LogError("Referência de coinText não está atribuída!"); // Log de erro para ajudar na depuração
        }
    }
}
