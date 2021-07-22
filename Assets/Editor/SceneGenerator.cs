using Hierarchy2;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace OctanGames
{
	[InitializeOnLoad]
	public class SceneGenerator
	{
		private const string EDITOR_ONLY_TAG = "EditorOnly";
		private const string UI_LAYER = "UI";
		private const string SET_PARENT_COMMAND = "Set new parent";

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
			var scene = SceneManager.GetActiveScene();
			Debug.Log($"Scene {scene.name} updated");
		}

		private static void SceneGenerate()
		{
			var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
			
			var uiObjects = rootObjects.Where(e => e.layer.Equals(LayerMask.NameToLayer(UI_LAYER))).ToArray();
			var cameras = rootObjects.Where(e => e.TryGetComponent(out Camera c)).ToArray();
			var lights = rootObjects.Where(e => e.TryGetComponent(out Light l)).ToArray();
			var eventSystem = Object.FindObjectOfType<EventSystem>();

			TryCreateSplitter("$Setup");
			Transform cameraFolder = TryCreateFolder("Cameras");
			Transform managerFolder = TryCreateFolder("Managers");

			TryCreateSplitter("$Environment");
			Transform lightFolder = TryCreateFolder("Lights");
			TryCreateFolder("World");

			TryCreateSplitter("$Characters");
			TryCreateFolder("Dynamic");

			TryCreateSplitter("$UI");
			Transform uiFolder = TryCreateFolder("UI");

			Undo.SetTransformParent(eventSystem.transform, managerFolder, SET_PARENT_COMMAND);

			for (int i = uiObjects.Length - 1; i >= 0; i--)
			{
				Undo.SetTransformParent(uiObjects[i].transform, uiFolder, SET_PARENT_COMMAND);
			}
			for (int i = lights.Length - 1; i >= 0; i--)
			{
				Undo.SetTransformParent(lights[i].transform, lightFolder, SET_PARENT_COMMAND);
			}
			for (int i = cameras.Length - 1; i >= 0; i--)
			{
				Undo.SetTransformParent(cameras[i].transform, cameraFolder, SET_PARENT_COMMAND);
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
				splitter.transform.SetAsLastSibling();
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
				splitter.transform.SetAsLastSibling();
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
