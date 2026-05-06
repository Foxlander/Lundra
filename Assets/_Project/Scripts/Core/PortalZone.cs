using UnityEngine;
using UnityEngine.InputSystem;

public class PortalZone : MonoBehaviour
{
    [SerializeField] private ZoneData zoneData;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool _playerInRange = false;

    private void Update()
    {
        if (_playerInRange && Keyboard.current != null)
        {
            if (UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
                EnterZone();
        }
    }

    private void EnterZone()
    {
        if (zoneData == null)
        {
            Debug.LogWarning("No ZoneData assigned to portal !");
            return;
        }

        Debug.Log($"Entering zone : {zoneData.ZoneName}");
        SceneLoader.Instance.LoadZone(zoneData.SceneName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
            Debug.Log($"Press E to enter {zoneData?.ZoneName}");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _playerInRange = false;
    }
}