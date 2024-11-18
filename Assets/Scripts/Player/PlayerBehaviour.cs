using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float jumpForce = 3;
    [SerializeField] private float crouchSpeed = 2;
    [SerializeField] private Vector2 crouchColliderSize = new Vector2(1, 0.5f);
    [SerializeField] private Vector2 normalColliderSize = new Vector2(1, 2);
    [SerializeField] private float slideSpeed = 8;
    [SerializeField] private float slideDuration = 0.5f;

    [Header("Propriedades de ataque")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private LayerMask attackLayer;

    private float moveDirection;
    private bool isSliding = false;
    private float slideTimer;
    private bool isCrouching = false;

    private Rigidbody2D playerRigidbody;
    private BoxCollider2D boxCollider;
    private Health health;
    private IsGroundedChecker isGroundedChecker;
    private Animator animator;

    private Transform movingPlatform;
    private Vector3 previousPlatformPosition;

    private bool hasDied = false;

    private void Awake()
    {
        // Inicializando os componentes necessários
        playerRigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        isGroundedChecker = GetComponent<IsGroundedChecker>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();

        // Assinando eventos de dano e morte
        health.OnHurt += PlayHurtAudio;
        health.OnDead += HandlePlayerDeath;
    }

    private void Start()
    {
        // Inicializando as entradas do jogador
        GameManager.Instance.InputManager.OnJump += HandleJump;
        GameManager.Instance.InputManager.OnSlide += HandleSlide;
        GameManager.Instance.InputManager.OnCrouch += HandleCrouch;
    }

    private void Update()
    {
        // Movimento do jogador, slide ou pulo
        if (isSliding)
        {
            SlidePlayer(); // Quando estiver deslizando, chamamos o método SlidePlayer
        }
        else
        {
            MovePlayer();  // Senão, movimentamos o jogador normalmente
        }

        // Virar o sprite de acordo com a direção do movimento
        FlipSpriteAccordingToMoveDirection();

        // Manter o jogador na plataforma móvel
        if (movingPlatform != null)
        {
            Vector3 platformMovement = movingPlatform.position - previousPlatformPosition;
            transform.position += platformMovement;
            previousPlatformPosition = movingPlatform.position;
        }
    }

    private void MovePlayer()
    {
        // Obter o movimento do jogador
        moveDirection = GameManager.Instance.InputManager.Movement;

        // Verificar se o jogador está agachado ou não
        if (isCrouching)
        {
            transform.Translate(moveDirection * Time.deltaTime * crouchSpeed, 0, 0);
        }
        else
        {
            transform.Translate(moveDirection * Time.deltaTime * moveSpeed, 0, 0);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Verificar se o jogador está sobre uma plataforma móvel
        if (collision.gameObject.CompareTag("Movel"))
        {
            movingPlatform = collision.transform;
            previousPlatformPosition = movingPlatform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Quando o jogador sai de uma plataforma móvel, a plataforma é nula
        if (collision.gameObject.CompareTag("Movel"))
        {
            movingPlatform = null;
        }
    }

    private void HandleCrouch(bool crouching)
    {
        // Lógica de agachar
        isCrouching = crouching;

        if (crouching)
        {
            boxCollider.size = crouchColliderSize;
            moveSpeed = crouchSpeed;
        }
        else
        {
            boxCollider.size = normalColliderSize;
            moveSpeed = 5;
        }
    }

    private void SlidePlayer()
    {
        // Deslizar o jogador
        slideTimer -= Time.deltaTime;
        if (slideTimer <= 0)
        {
            isSliding = false;  // Se o tempo de slide acabar, interromper o slide
            return;
        }

        transform.Translate(moveDirection * Time.deltaTime * slideSpeed, 0, 0);
    }

    private void HandleSlide()
    {
        // Se o jogador não está no chão ou já está deslizando, não inicia o slide
        if (!isGroundedChecker.IsGrounded() || isSliding) return;

        isSliding = true;
        slideTimer = slideDuration;
    }

    private void FlipSpriteAccordingToMoveDirection()
    {
        // Virar o sprite para a esquerda ou direita dependendo da direção
        if (moveDirection < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); 
        }
        else if (moveDirection > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); 
        }
    }

    private void HandleJump()
    {
        // Realizar o pulo do jogador
        if (!isGroundedChecker.IsGrounded() || isCrouching) return;
        playerRigidbody.velocity += Vector2.up * jumpForce;
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerJump);
    }

    private void PlayHurtAudio()
    {
        // Tocar som de dano
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerHurt);
    }

    private void HandlePlayerDeath()
    {
        // Lógica para a morte do jogador
        if (!hasDied)
        {
            hasDied = true;
            GetComponent<Collider2D>().enabled = false; // Desabilitar colisão
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll; // Congelar movimento
            GameManager.Instance.InputManager.DisablePlayerInput(); // Desabilitar entradas
            GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerDeath);

            animator.SetTrigger("dead");  // Trigger de animação de morte
        }
    }

    private void ShowMenuAfterDeath()
    {
        // Carregar a cena do menu após a morte
        SceneManager.LoadScene("MenuScene"); 
    }

    private void Attack()
    {
        // Lógica de ataque
        Collider2D[] hittedEnemies = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, attackLayer);
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerAttack);

        foreach (Collider2D hittedEnemy in hittedEnemies)
        {
            if (hittedEnemy.TryGetComponent(out Health enemyHealth))
            {
                enemyHealth.TakeDamage();
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Desenhar gizmos para o alcance do ataque no editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }

    // Método para acessar o estado de agachamento (adicionado para o PlayerAnim)
    public bool IsCrouching()
    {
        return isCrouching;
    }

    // Método para acessar o estado de deslize (adicionado para o PlayerAnim)
    public bool IsSliding()
    {
        return isSliding;
    }
}
