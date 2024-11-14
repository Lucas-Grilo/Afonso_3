using UnityEngine;
using UnityEngine.UI;

public class Final : MonoBehaviour
{
    // Referência à imagem da UI que será mostrada após a colisão
    public GameObject imagemUI;

    // Método chamado quando o player entra na área do trigger
    private void OnTriggerEnter2D(Collider2D colisao)
    {
        Debug.Log("OnTriggerEnter2D chamado!");  // Verifica se o método é chamado

        // Verifica se a colisão é com o objeto específico (por tag)
        Debug.Log("Colidiu com: " + colisao.gameObject.name); // Imprime o nome do objeto com o qual o trigger foi ativado

        if (colisao.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colisão com o objeto com a tag 'Final' detectada!");  // Log para confirmar que a tag foi encontrada
            // Torna a imagem visível
            imagemUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Debug.Log("A tag do objeto não é 'Final'."); // Caso a tag não seja encontrada
        }
    }
}
