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
		private const string PLAYER_TAG = "Player";

		private const string UI_LAYER = "UI";
		private const string PLAYER_LAYER = PLAYER_TAG;

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
			var players = rootObjects.Where(e => e.tag.Equals(PLAYER_TAG) || e.layer.Equals(LayerMask.NameToLayer(PLAYER_LAYER))).ToArray();
			var cameras = rootObjects.Where(e => e.TryGetComponent(out Camera c)).ToArray();
			var lights = rootObjects.Where(e => e.TryGetComponent(out Light l)).ToArray();
			var staticObjects = rootObjects.Where(e => e.isStatic).ToArray();
			var eventSystem = Object.FindObjectOfType<EventSystem>();

			//var entities = rootObjects
			//	.Except(uiObjects)
			//	.Except(staticObjects)
			//	.Except(cameras)
			//	.Except(lights);

			TryCreateSplitter("$Setup");
			Transform cameraFolder = TryCreateFolder("Cameras");
			Transform managerFolder = TryCreateFolder("Managers");

			TryCreateSplitter("$Environment");
			Transform lightFolder = TryCreateFolder("Lights");
			Transform worldFolder = TryCreateFolder("World");

			TryCreateSplitter("$Entities");
			TryCreateFolder("Dynamic");
			Transform characterFoloder = TryCreateFolder("Characters");

			TryCreateSplitter("$UI");
			Transform uiFolder = TryCreateFolder("UI");

			AttachToParent(cameras, cameraFolder);
			AttachToParent(lights, lightFolder);
			AttachToParent(staticObjects, worldFolder);
			AttachToParent(players, characterFoloder);
			AttachToParent(uiObjects, uiFolder);
			if (eventSystem != null)
			{
				AttachToParent(eventSystem, managerFolder);
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

		public static void AttachToParent(GameObject go, Transform parent)
		{
			Undo.SetTransformParent(go.transform, parent, SET_PARENT_COMMAND);
		}
		public static void AttachToParent<T>(T component, Transform parent) where T : Component
		{
			AttachToParent(component.gameObject, parent);
		}
		public static void AttachToParent(GameObject[] objects, Transform parent)
		{
			for (int i = objects.Length - 1; i >= 0; i--)
			{
				AttachToParent(objects[i], parent);
			}
		}
		public static void AttachToParent<T>(T[] objects, Transform parent) where T : Component
		{
			for (int i = objects.Length - 1; i >= 0; i--)
			{
				AttachToParent(objects[i], parent);
			}
		}
	}
}
