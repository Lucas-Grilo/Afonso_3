using UnityEngine;
 
public class CharacterPersistence : MonoBehaviour
{
    private static CharacterPersistence instance;
 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
}