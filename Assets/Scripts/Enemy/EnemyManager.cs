using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [Header("Spawn Settings")]
    [SerializeField] private int spawnAmount = 3;
    [SerializeField] private float spawnRate = 5f;
    [SerializeField] private float spawnRadius = 10f;

    [Header("Enemy Stats")]
    [SerializeField] private GameObject[] enemyTypes;

    [Header("Damage numbers")]
    [SerializeField] Transform playerLocation;
    [SerializeField] private Canvas enemyDamageCanvas;
    [SerializeField] private GameObject textMeshObject;
    [SerializeField] private float textFadeAfter = 0.3f;

    //temp test enemy variables
    public float enemyHealth = 10f;
    public float enemySpeed = 2.5f;
    public float enemyDamage = 1f;
    public float attackCooldown = 1.0f;

    public void StartEnemyWave()
    {
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        while (true)
        {
            for(int i = 0; i < spawnAmount; i++)
            {
                SpawnAtRandomPoint();
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void SpawnAtRandomPoint()
    {
        Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;

        Vector2 randomPointAroundPlayer = (Vector2)playerLocation.position + randomPoint;

        GameObject newEnemy = Instantiate(enemyTypes[0], transform);
        Enemy e = newEnemy.GetComponent<Enemy>();
        e.Initialize(playerLocation, this);
        newEnemy.transform.position = randomPointAroundPlayer;
    }

    public void EnemyDamageTaken(float amount, Vector2 enemyPos)
    {
        GameObject enemyDamageText = Instantiate(textMeshObject, enemyDamageCanvas.transform);
        enemyDamageText.GetComponent<TextMeshProUGUI>().text = amount.ToString();
        Vector2 randomTextSpawn = new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(0.2f, 0.3f));
        Vector2 textSpawnWOffset = enemyPos + randomTextSpawn;
        enemyDamageText.transform.position = textSpawnWOffset;
        enemyDamageText.transform.DOMoveY(textSpawnWOffset.y + .25f, textFadeAfter, false).SetEase(Ease.OutSine);
        Destroy(enemyDamageText, textFadeAfter);
    }
}
