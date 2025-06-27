using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyManager))]
public class EnemyManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnemyManager enemyManager = (EnemyManager)target;

        if (GUILayout.Button("Start Wave"))
        {
            enemyManager.StartEnemyWave();
        }
    }
}
