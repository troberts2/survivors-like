using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerUpgrade))]
public class PlayerUpgradeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerUpgrade playerUpgrade = (PlayerUpgrade)target;
        if(GUILayout.Button("Open Upgrade panel"))
        {
            playerUpgrade.OpenUpgradeScreen();
        }

        if(GUILayout.Button("Give XP"))
        {
            playerUpgrade.CollectExperience(10);
        }
    }
}
