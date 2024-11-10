using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator animator;
    private IsGroundedChecker groundedChecker;
    private Health playerHealth;
    private PlayerBehaviour playerBehaviour;  // Referência para verificar o status da escorregada

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
            GameManager.Instance.InputManager.OnCrouch += HandleCrouchAnim;  // Liga o evento de agachamento
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
        // Verificar se animator é válido antes de usar
        if (animator != null)
        {
            bool isMoving = GameManager.Instance.InputManager.Movement != 0;
            bool isSliding = playerBehaviour != null && playerBehaviour.IsSliding();  // Verifica se o personagem está escorregando
            bool isJumping = groundedChecker != null && !groundedChecker.IsGrounded();

            // Define o estado de agachamento antes de definir o estado de andar
            animator.SetBool("isCrouching", animator.GetBool("isCrouching"));  // Manter o estado atual de agachamento

            // Atualiza o estado de andar
            animator.SetBool("isWalking", isMoving && !isSliding && !animator.GetBool("isCrouching"));  // Não andar se estiver agachado

            animator.SetBool("isJumping", isJumping);
            animator.SetBool("isSliding", isSliding);  // Define o estado de escorregada
        }
    }

    private void HandleCrouchAnim(bool isCrouching)
    {
        if (animator != null)
        {
            animator.SetBool("isCrouching", isCrouching);  // Atualiza o estado de agachamento
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
        }
    }
}
