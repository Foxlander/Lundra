using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    [Header("Respawn Settings")]
    [SerializeField] private float deathDuration = 2f;

    private PlayerController _controller;
    private CharacterStats _stats;
    private bool _isDead = false;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _stats = GetComponent<CharacterStats>();
    }

    public void OnPlayerDeath()
    {
        if (_isDead) return;

        _isDead = true;
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        Debug.Log("Player is dead !");

        _controller.enabled = false;

        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();

        yield return new WaitForSecondsRealtime(deathDuration);

        Respawn();
    }

    private void Respawn()
    {
        Debug.Log("Player respawned !");

        _isDead = false;
        _stats.FullHeal();
        _stats.FullMana();
        _controller.enabled = true;

        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadHub();
        else
            Debug.Log("No SceneLoader found !");
    }
}