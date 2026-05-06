using UnityEngine;

public class BootLoader : MonoBehaviour
{
    private void Start()
    {
        SceneLoader.Instance.LoadHub(); // ← charge le Hub au démarrage
    }
}