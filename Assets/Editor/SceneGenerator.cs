using Hierarchy2;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OctanGames
{
	[InitializeOnLoad]
	public class SceneGenerator
	{
		private const string EDITOR_ONLY_TAG = "EditorOnly";

		static SceneGenerator()
		{
			EditorSceneManager.newSceneCreated += SceneCreate;
		}

		public static void SceneCreate(Scene scene, NewSceneSetup setup, NewSceneMode mode)
		{
			SceneGenerate();
			EditorSceneManager.MarkSceneDirty(scene);

			Debug.Log("New scene created");
		}

		[MenuItem("Tools/Scene/Update", priority = 1)]
		public static void SceneUpdate()
		{
			SceneGenerate();

			Debug.Log("Scene updated");
		}

		private static void SceneGenerate()
		{
			CreateSplitter("$Setup");
			Transform camerasFolder = CreateFolder("Cameras");
			CreateFolder("Managers");

			CreateSplitter("$Environment");
			Transform lightsFolder = CreateFolder("Lights");
			CreateFolder("World");

			CreateSplitter("$Characters");
			CreateFolder("Dynamic");

			CreateSplitter("$UI");
			CreateFolder("UI");

			var camera = Camera.main;
			if (camera != null)
			{
				Undo.SetTransformParent(camera.transform, camerasFolder, "Set new parent");
			}

			GameObject light = GameObject.Find("Directional Light");
			if (light != null)
			{
				Undo.SetTransformParent(light.transform, lightsFolder, "Set new parent");
			}
		}

		private static Transform CreateSplitter(string name)
		{
			var splitter = new GameObject(name) { tag = EDITOR_ONLY_TAG };
			Undo.RegisterCreatedObjectUndo(splitter, name);

			return splitter.transform;
		}

		private static Transform CreateFolder(string name)
		{
			var folder = new GameObject(name, typeof(HierarchyFolder));
			Undo.RegisterCreatedObjectUndo(folder, name);

			return folder.transform;
		}
	}
}
