using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor
{
    private MapGenerator map;

	private void OnEnable()
	{
        map = target as MapGenerator;
    }

	public override void OnInspectorGUI()
    {
        if (DrawDefaultInspector())
        {
            map.GenerateMap();
        }

        if(GUILayout.Button("Generate Map"))
        {
            map.GenerateMap();
        }
    }
}
