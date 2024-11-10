using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public InputManager InputManager { get; private set; }  // Referência ao InputManager
    public AudioManager AudioManager { get; private set; }  // Referência ao AudioManager

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);  // Evita instância duplicada
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);  // Mantém o GameManager entre cenas

        // Acessando a instância do InputManager via Singleton
        InputManager = InputManager.Instance;

        // Acessando o AudioManager, caso seja necessário
        AudioManager = FindObjectOfType<AudioManager>();  
        if (AudioManager == null)
        {
            Debug.LogError("AudioManager não encontrado na cena.");
        }
    }
}
