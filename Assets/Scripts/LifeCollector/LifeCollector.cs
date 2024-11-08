using UnityEngine;
using UnityEngine.UI;

public class LifeCollector : MonoBehaviour
{
    private Animator animator;
    public Slider vidaSlider;
    public Health playerHealth; // Referência para o Health do player

    void Start()
    {
        // Verifica se o playerHealth foi atribuído
        if (playerHealth == null)
        {
            Debug.LogError("Referência ao playerHealth não atribuída!");
            return; // Não prossegue se não tiver referência
        }

        // Verifica se o vidaSlider foi atribuído
        if (vidaSlider == null)
        {
            Debug.LogError("Referência ao vidaSlider não atribuída!");
            return; // Não prossegue se não tiver referência
        }

        // Configura o valor máximo do Slider com base nas vidas do player
        vidaSlider.maxValue = playerHealth.Lives;

        animator = GetComponent<Animator>();
        UpdateVidaSlider(); // Atualiza o Slider de vida na inicialização
    }

    void Update()
    {
        // Atualiza o valor do Slider para refletir as vidas do player
        if (playerHealth != null)
        {
            vidaSlider.value = playerHealth.Lives;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto colidido é um bloco de vida
        if (collision.gameObject.CompareTag("LucyBlock"))
        {
            Debug.Log("Colidiu com o bloco: " + collision.gameObject.name);

            // Chama o método para adicionar uma vida no playerHealth
            playerHealth.AddLife();

            // Atualiza o valor do Slider após adicionar a vida
            UpdateVidaSlider();

            // Executa a animação
            if (animator != null)
            {
                animator.SetTrigger("PlayAnim");
            }
            else
            {
                Debug.LogError("Animator não configurado.");
            }
        }
    }

    private void UpdateVidaSlider()
    {
        // Atualiza o valor do Slider para exibir o total de vidas do jogador
        vidaSlider.value = playerHealth.Lives;
        Debug.Log("Slider de vida atualizado: " + vidaSlider.value);
    }
}
