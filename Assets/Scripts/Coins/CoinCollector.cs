using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public int currentCoinCount = 0; // Contador de moedas coletadas
    public TextMeshProUGUI coinText; // Referência ao componente TMP

    void Start()
    {
        UpdateCoinText(); // Atualiza o texto no início
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colidiu com: " + other.gameObject.name); // Log para verificar a colisão

        // Verifica se o objeto colidido está na camada "Moeda"
        if (other.gameObject.layer == LayerMask.NameToLayer("Moeda"))
        {
            Debug.Log("Moeda coletada!"); // Log para verificar a coleta
            CoinAnimation coinAnimation = other.GetComponent<CoinAnimation>();
            if (coinAnimation != null)
            {
                coinAnimation.CollectCoin(); // Aciona a coleta da moeda
                currentCoinCount++; // Incrementa o contador de moedas
                UpdateCoinText(); // Atualiza o texto para refletir o novo total
            }
            else
            {
                Debug.LogError("CoinAnimation não encontrado na moeda: " + other.gameObject.name);
            }
        }
        else
        {
            Debug.Log("O objeto não é uma moeda.");
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
