using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EvilWizard))]
public class EvilWizardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EvilWizard wizard = (EvilWizard)target;

        if(GUILayout.Button("Attack 1"))
        {
            wizard.AttackOneRight();
        }

        if (GUILayout.Button("Attack 1 Left"))
        {
            wizard.AttackOneLeft();
        }

        if (GUILayout.Button("Attack 2 right"))
        {
            wizard.AttackTwoRight();
        }
        
        if (GUILayout.Button("Attack 2 left"))
        {
            wizard.AttackTwoLeft();
        }

        if (GUILayout.Button("Attack Slam"))
        {
            wizard.StartJumpAttack();
        }
    }

}
