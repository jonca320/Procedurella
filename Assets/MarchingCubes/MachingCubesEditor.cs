using UnityEngine;
using UnityEditor;
#if (UNITY_EDITOR) 

[CustomEditor(typeof(MarchingCubes))]
public class MarchingCubesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MarchingCubes MC = (MarchingCubes)target;
        if(GUILayout.Button("Generate Mesh"))
        {
            MC.Generate();
        }
    }
}
#endif