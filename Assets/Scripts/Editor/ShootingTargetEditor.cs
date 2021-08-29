#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShootingTarget))]
public class ShootingTargetEditor : Editor
{
    ShootingTarget script;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        script = (ShootingTarget)target;
        EditorGUILayout.LabelField("Cannons", script.NumberOfCannons.ToString());
        CreateButtons(script);

        
    }

    private void OnSceneGUI()
    {
        for (int i = 0; i < script.transform.childCount; i++)
        {
            Transform cannon = script.transform.GetChild(i);
            if (cannon.CompareTag("Cannon"))
            {
                cannon.rotation = Handles.RotationHandle(cannon.rotation, cannon.GetChild(2).position);
            }
        }
    }
    [DrawGizmo(GizmoType.Pickable | GizmoType.Selected)]
    static void DrawGizmoSelected(ShootingTarget shootingTarget, GizmoType gType)
    {
        
        for (int i = 0; i < shootingTarget.transform.childCount; i++)
        {
            Transform cannon = shootingTarget.transform.GetChild(i);
            if (cannon.CompareTag("Cannon"))
            {
                Handles.DrawDottedLine(cannon.GetChild(0).position, cannon.GetChild(1).position, 3);
            }
        }
    }
    private void OnEnable()
    {
        script = (ShootingTarget)target;
    }
  
   

    private void CreateButtons(ShootingTarget script)
    {
        if (script.gameObject.activeInHierarchy && !Application.isPlaying)
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
#endif
