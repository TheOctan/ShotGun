using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Extesions
{
	[MenuItem("GameObject/Create Separator", false, 0)]
	public static void CreateSeparator(MenuCommand menuCommand)
	{
		GameObject go = new GameObject("======================");
		go.tag = "EditorOnly";

		GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
		Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
		Selection.activeObject = go;
	}
}
