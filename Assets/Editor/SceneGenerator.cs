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
		}

		[MenuItem("Tools/Scene/Update", priority = 1)]
		public static void SceneUpdate()
		{
			SceneGenerate();

			Debug.Log("Scene updated");
		}

		private static void SceneGenerate()
		{
			TryCreateSplitter("$Setup");
			Transform camerasFolder = TryCreateFolder("Cameras");
			TryCreateFolder("Managers");

			TryCreateSplitter("$Environment");
			Transform lightsFolder = TryCreateFolder("Lights");
			TryCreateFolder("World");

			TryCreateSplitter("$Characters");
			TryCreateFolder("Dynamic");

			TryCreateSplitter("$UI");
			TryCreateFolder("UI");

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

		public static Transform TryCreateSplitter(string name)
		{
			var splitter = GameObject.Find(name);

			if (splitter == null)
			{
				return CreateSplitter(name);
			}
			else
			{
				return splitter.transform;
			}
		}

		public static Transform TryCreateFolder(string name)
		{
			var splitter = GameObject.Find(name);

			if (splitter == null)
			{
				return CreateFolder(name);
			}
			else
			{
				return splitter.transform;
			}
		}

		public static Transform CreateSplitter(string name)
		{
			var splitter = new GameObject(name) { tag = EDITOR_ONLY_TAG };
			Undo.RegisterCreatedObjectUndo(splitter, name);

			return splitter.transform;
		}

		public static Transform CreateFolder(string name)
		{
			var folder = new GameObject(name, typeof(HierarchyFolder));
			Undo.RegisterCreatedObjectUndo(folder, name);

			return folder.transform;
		}
	}
}
