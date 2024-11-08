using UnityEngine;
using UnityEngine.InputSystem;

public class CoinAnimation : MonoBehaviour
{
    private Animator animator;
    public AudioSource audioSource;
    public AudioClip collectSound;

    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Verifica se os componentes estão atribuídos
        if (animator == null) Debug.LogError("Animator não encontrado.");
        if (audioSource == null) Debug.LogError("AudioSource não encontrado.");
        if (collectSound == null) Debug.LogError("CollectSound não está atribuído.");
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Gameplay.Collect.performed += _ => CollectCoin();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void CollectCoin()
    {
        Debug.Log("Moeda coletada!");
        PlaySound();
        animator.SetTrigger("Moeda");

        // Destrói o objeto após o tempo de duração do som, ou 0.5 segundos se o som estiver ausente
        Destroy(gameObject, collectSound != null ? collectSound.length : 0.05f);
    }

    private void PlaySound()
    {
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position, audioSource ? audioSource.volume : 1.0f);
            Debug.Log("Som reproduzido: " + collectSound.name);
        }
        else
        {
            Debug.LogError("CollectSound está nulo.");
        }
    }
}
