using UnityEngine;
using UnityEditor;

#if(UNITY_EDITOR)
[CustomEditor(typeof(RockSpawner))]
public class RockSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RockSpawner RS = (RockSpawner)target;
        if(GUILayout.Button("Spawn Rocks"))
        {
            //RS.SpawnRocks();
        }

        if (GUILayout.Button("Increase LOD"))
        {
            //RS.IncreaseLOD();
        }

        if (GUILayout.Button("Decrease LOD"))
        {
            //RS.DecreaseLOD();
        }
    }
}
#endif