using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int lives;

    public event Action OnDead;
    public event Action OnHurt;

    public int Lives
    {
        get { return lives; }
        private set { lives = value; }
    }

    public void TakeDamage()
    {
        lives--;
        HandleDamageTaken();
    }

    private void HandleDamageTaken()
    {
        if (lives <= 0)
        {
            OnDead?.Invoke();
        }
        else
        {
            OnHurt?.Invoke();
        }
    }

    // Método para adicionar uma vida
    public void AddLife()
    {
        lives++;
        Debug.Log("Vida adicionada! Total de vidas: " + lives);
    }
}
