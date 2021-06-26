using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShootingTarget))]
public class ShootingTargetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ShootingTarget script = (ShootingTarget)target;
        EditorGUILayout.LabelField("Cannons", script.NumberOfCannons.ToString());
        if (script.gameObject.activeInHierarchy)
        {
            if (GUILayout.Button("Add cannon"))
            {
                script.CreateCannon();
            }
            if (GUILayout.Button("Remove cannon"))
            {
                script.RemoveCannon();
            }
        }
    }

}
