using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Wave", menuName = "Enemy/Enemy Wave")]
public class EnemyWaveData : ScriptableObject
{
    public GameObject[] enemyTypes;
    public GameObject minibossPrefab;
}
