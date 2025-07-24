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

    [Header("Wave Configuration")]
    [SerializeField] private List<EnemyWaveData> waveConfigs; // One per minute
    [SerializeField] private GameObject finalBossPrefab;
    [SerializeField] private Transform finalBossSpawnPoint;

    private float elapsedTime = 0f;
    private int currentWaveIndex = 0;
    private float levelLength = 900f; //15 minutes
    private bool waveRunning = false;


    private PlayerMovement pm;

    private void Start()
    {
        pm = PlayerManager.Instance.PlayerMovement;
    }

    public void StartEnemyWave()
    {
        if (!waveRunning)
        {
            waveRunning = true;
            StartCoroutine(WaveLoop());
        }
    }

    private IEnumerator WaveLoop()
    {
        while (elapsedTime < levelLength)
        {
            int minute = Mathf.FloorToInt(elapsedTime / 60f);

            // Change enemy wave every minute
            if (minute != currentWaveIndex && minute < waveConfigs.Count)
            {
                currentWaveIndex = minute;
                Debug.Log($"[Wave] Minute {minute + 1}, new enemies set.");
            }

            // Spawn enemies every spawnRate seconds
            for (int i = 0; i < spawnAmount; i++)
            {
                SpawnAtRandomPoint();
            }

            // Spawn miniboss every 5 minutes
            if (minute > 0 && minute % 5 == 0 && Mathf.FloorToInt(elapsedTime) % 60 == 0)
            {
                SpawnMiniboss();
            }

            yield return new WaitForSeconds(spawnRate);
            elapsedTime += spawnRate;
        }

        SpawnFinalBoss();
    }

    private void SpawnAtRandomPoint()
    {
        if (currentWaveIndex >= waveConfigs.Count) return;

        var waveData = waveConfigs[currentWaveIndex];
        if (waveData.enemyTypes == null || waveData.enemyTypes.Length == 0) return;

        Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
        Vector2 spawnPos = (Vector2)playerLocation.position + randomPoint;

        GameObject randomEnemy = waveData.enemyTypes[Random.Range(0, waveData.enemyTypes.Length)];
        GameObject newEnemy = Instantiate(randomEnemy, spawnPos, Quaternion.identity, transform);
        Enemy e = newEnemy.GetComponent<Enemy>();
        e.Initialize(playerLocation, this, pm);
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

    private void SpawnMiniboss()
    {
        var waveData = waveConfigs[currentWaveIndex];
        if (waveData.minibossPrefab == null) return;

        Vector2 spawnPos = playerLocation.position + (Vector3)(Random.insideUnitCircle.normalized * spawnRadius);
        GameObject miniboss = Instantiate(waveData.minibossPrefab, spawnPos, Quaternion.identity, transform);
        Enemy e = miniboss.GetComponent<Enemy>();
        e.Initialize(playerLocation, this, pm);
        Debug.Log("[Boss] Miniboss spawned!");
    }

    private void SpawnFinalBoss()
    {
        GameObject boss = Instantiate(finalBossPrefab, finalBossSpawnPoint.position, Quaternion.identity);
        Enemy e = boss.GetComponent<Enemy>();
        e.Initialize(playerLocation, this, pm);
        Debug.Log("[Boss] Final Boss has arrived!");
    }
}
