using System;
using UnityEngine;

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

    private Transform movingPlatform;
    private Vector3 previousPlatformPosition;

    private bool hasDied = false; // Variável de controle para tocar o som de morte apenas uma vez

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        isGroundedChecker = GetComponent<IsGroundedChecker>();
        health = GetComponent<Health>();
        health.OnHurt += PlayHurtAudio;
        health.OnDead += HandlePlayerDeath;
    }

    private void Start()
    {
        GameManager.Instance.InputManager.OnJump += HandleJump;
        GameManager.Instance.InputManager.OnSlide += HandleSlide;
        GameManager.Instance.InputManager.OnCrouch += HandleCrouch;
    }

    private void Update()
    {
        if (isSliding)
        {
            SlidePlayer();
        }
        else
        {
            MovePlayer();
        }

        FlipSpriteAccordingToMoveDirection();

        if (movingPlatform != null)
        {
            Vector3 platformMovement = movingPlatform.position - previousPlatformPosition;
            transform.position += platformMovement;
            previousPlatformPosition = movingPlatform.position;
        }
    }

    private void MovePlayer()
    {
        moveDirection = GameManager.Instance.InputManager.Movement;

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
        if (collision.gameObject.CompareTag("Movel"))
        {
            movingPlatform = collision.transform;
            previousPlatformPosition = movingPlatform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Movel"))
        {
            movingPlatform = null;
        }
    }

    private void HandleCrouch(bool crouching)
    {
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
        slideTimer -= Time.deltaTime;
        if (slideTimer <= 0)
        {
            isSliding = false;
            return;
        }

        transform.Translate(moveDirection * Time.deltaTime * slideSpeed, 0, 0);
    }

    private void HandleSlide()
    {
        if (!isGroundedChecker.IsGrounded() || isSliding) return;

        isSliding = true;
        slideTimer = slideDuration;
    }

    private void FlipSpriteAccordingToMoveDirection()
    {
        if (moveDirection < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveDirection > 0)
        {
            transform.localScale = Vector3.one;
        }
    }

    private void HandleJump()
    {
        if (!isGroundedChecker.IsGrounded() || isCrouching) return;
        playerRigidbody.velocity += Vector2.up * jumpForce;
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerJump);
    }

    private void PlayHurtAudio() 
    { 
            GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerHurt);
    }

    private void HandlePlayerDeath()
    {
        if (!hasDied) // Verifica se o jogador já morreu
        {
            hasDied = true; // Marca que o som de morte já foi tocado
            GetComponent<Collider2D>().enabled = false;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            GameManager.Instance.InputManager.DisablePlayerInput();
            GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerDeath);
        }
    }

    private void Attack()
    {
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }

    public bool IsSliding()
    {
        return isSliding;
    }
}
