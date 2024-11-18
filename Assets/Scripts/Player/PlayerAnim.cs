using UnityEngine;
using UnityEngine.SceneManagement;  // Certifique-se de que você tem esse using para o SceneManager

public class PlayerAnim : MonoBehaviour
{
    private Animator animator;
    private IsGroundedChecker groundedChecker;
    private Health playerHealth;
    private PlayerBehaviour playerBehaviour;

    private bool hasDied = false;

    private void Awake()
    {
        // Tenta pegar os componentes
        animator = GetComponent<Animator>();
        groundedChecker = GetComponent<IsGroundedChecker>();
        playerHealth = GetComponent<Health>();
        playerBehaviour = GetComponent<PlayerBehaviour>();  // Obtenha a referência do PlayerBehaviour

        // Verificações de null
        if (animator == null)
        {
            Debug.LogError("Animator não encontrado no GameObject " + gameObject.name);
        }
        if (groundedChecker == null)
        {
            Debug.LogError("IsGroundedChecker não encontrado no GameObject " + gameObject.name);
        }
        if (playerHealth == null)
        {
            Debug.LogError("Health não encontrado no GameObject " + gameObject.name);
        }
        if (playerBehaviour == null)
        {
            Debug.LogError("PlayerBehaviour não encontrado no GameObject " + gameObject.name);
        }

        // Inscrevendo nos eventos
        if (GameManager.Instance != null && GameManager.Instance.InputManager != null)
        {
            GameManager.Instance.InputManager.OnAttack += PlayAttackAnim;
        }
        else
        {
            Debug.LogError("GameManager ou InputManager não encontrados.");
        }

        if (playerHealth != null)
        {
            playerHealth.OnHurt += PlayHurtAnim;
            playerHealth.OnDead += PlayDeadAnim;
        }
    }

    private void Update()
    {
        if (animator == null || playerBehaviour == null || groundedChecker == null)
            return;  // Se qualquer referência importante estiver ausente, não faça nada.

        bool isMoving = GameManager.Instance.InputManager.Movement != 0;
        bool isSliding = playerBehaviour.IsSliding();  // Verifica se o personagem está escorregando
        bool isJumping = !groundedChecker.IsGrounded();

        // Atualiza o estado de agachamento
        bool isCurrentlyCrouching = playerBehaviour.IsCrouching();
        animator.SetBool("isCrouching", isCurrentlyCrouching);  // Refatorando para obter diretamente o estado de agachamento

        // Atualiza o estado de andar (só atualiza se o estado for realmente diferente)
        bool shouldWalk = isMoving && !isSliding && !isCurrentlyCrouching;
        if (animator.GetBool("isWalking") != shouldWalk)
        {
            animator.SetBool("isWalking", shouldWalk);
        }

        // Atualiza os estados de pulo e escorregada
        if (animator.GetBool("isJumping") != isJumping)
        {
            animator.SetBool("isJumping", isJumping);
        }

        if (animator.GetBool("isSliding") != isSliding)
        {
            animator.SetBool("isSliding", isSliding);  // Define o estado de escorregada
        }

        // Verifica se a animação de morte acabou
        if (hasDied)
        {
            Debug.Log("A animação de morte está em execução.");
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // Verificando se a animação de morte terminou
            if (stateInfo.IsName("anim_Dead"))
            {
                Debug.Log("A animação de morte está tocando.");
            }
            if (stateInfo.normalizedTime >= 1f && stateInfo.IsName("anim_Dead"))
            {
                Debug.Log("A animação de morte terminou. Carregando cena do menu...");
                ShowMenuAfterDeath();
            }
        }
    }

    private void PlayAttackAnim()
    {
        if (animator != null)
        {
            animator.SetTrigger("attack");
        }
    }

    private void PlayHurtAnim()
    {
        if (animator != null)
        {
            animator.SetTrigger("hurt");
        }
    }

    private void PlayDeadAnim()
    {
        if (animator != null)
        {
            animator.SetTrigger("dead");
            hasDied = true;  // Marca que o jogador morreu
            Debug.Log("Trigger de morte ativado.");
        }
    }

    private void ShowMenuAfterDeath()
    {
        // Carregar a cena do menu após a morte
        Debug.Log("Carregando a cena do menu...");
        SceneManager.LoadScene("MenuScene");
    }
}
