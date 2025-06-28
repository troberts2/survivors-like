using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPBehavior : MonoBehaviour
{
    [SerializeField] private float _xpValue = 10f;
    [SerializeField] private float _playerCollectDelay = 1.5f;
    private BoxCollider2D _boxCollider;

    public float PlayerCollectDelay { get => _playerCollectDelay; set => _playerCollectDelay = value; }

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public void GoToPlayer(Transform target, float duration, PlayerUpgrade playerUpgrade)
    {
        StartCoroutine(MoveToPosition(target, duration, playerUpgrade));
    }

    private IEnumerator MoveToPosition(Transform targetPosition, float duration, PlayerUpgrade playerUpgrade)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        _boxCollider.enabled = false;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition.position, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition.position; // ensure it ends exactly at the target
        playerUpgrade.CollectExperience(_xpValue);
        Destroy(gameObject);
    }

}
