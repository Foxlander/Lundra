using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    private void Start()
    {
        // Si on est déjà dans une scène de test, on ne charge pas le Hub
        if (SceneManager.GetActiveScene().name == "TestScene")
            return;

        SceneLoader.Instance.LoadHub();
    }
}