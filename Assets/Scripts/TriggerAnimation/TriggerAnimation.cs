using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    private Animator animator;
    public AudioSource audioSource; // Referência ao AudioSource
    public AudioClip playAnimSound; // Som para a animação PlayAnim

    public int maxCollisions = 3; // Define o número máximo de colisões antes de destruir o objeto
    public int currentCollisions = 0; // Contador de colisões

    void Start()
    {
        // Inicializando componentes
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Obtém o AudioSource do objeto

        // Verificando se os componentes essenciais estão presentes
        if (animator == null)
        {
            Debug.LogError("Animator não encontrado no objeto " + gameObject.name);
        }
        if (audioSource == null)
        {
            Debug.LogError("AudioSource não encontrado no objeto " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se não for o player, não processa
        if (!other.CompareTag("Player"))
        {
            Debug.LogWarning("Objeto que colidiu não tem a tag 'Player'.");
            return;
        }

        // Verifica se o player entrou na colisão
        Debug.Log("Player entrou na colisão!");

        // Se ainda não atingiu o limite de colisões, conta a colisão
        if (currentCollisions < maxCollisions)
        {
            currentCollisions++; // Incrementa a contagem de colisões de 1 em 1
            animator.SetTrigger("PlayAnim"); // Toca a animação

            // Toca o som da animação PlayAnim
            PlaySound(playAnimSound);
        }

        // Se atingiu o número máximo de colisões, destrói o objeto
        if (currentCollisions >= maxCollisions)
        {
            Debug.Log("Número máximo de colisões atingido.");

            // Destrói o objeto
            Debug.Log("MaxCollisions atingido. Destruindo o objeto.");
            Destroy(gameObject); // Destrói o objeto
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip; // Atribui o clip de áudio
            audioSource.Play(); // Toca o áudio
        }
        else
        {
            Debug.LogError("AudioSource ou clip está nulo ao tentar tocar o som.");
        }
    }

    private void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop(); // Para o som
            Debug.Log("Som parado após a animação.");
        }
    }
}
