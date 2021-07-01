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
    	static SceneGenerator()
		{
			EditorSceneManager.newSceneCreated += SceneCreate;
        }

		public static void SceneCreate(Scene scene, NewSceneSetup setup, NewSceneMode mode)
		{
			//var camera = Camera.main.transform;
			//var light = GameObject.Find("Directional Light").transform;

			CreateSplitter("$Setup");
			var camerasFolder = CreateFolder("Cameras");
			CreateFolder("Managers");
			//camera.parent = camerasFolder;

			CreateSplitter("$Environment");
			var lightsFolder = CreateFolder("Lights");
			//light.parent = lightsFolder;
			CreateFolder("World");

			CreateSplitter("$UI");
			CreateFolder("UI");

			CreateSplitter("$Characters");
			CreateFolder("Dynamic");

			EditorSceneManager.MarkSceneDirty(scene);

			Debug.Log("New scene created");
		}

		private static Transform CreateSplitter(string name)
		{
			var splitter = new GameObject(name).transform;
			splitter.tag = "EditorOnly";
			return splitter;
		}

		private static Transform CreateFolder(string name)
		{
			return new GameObject(name, typeof(HierarchyFolder)).transform;
		}
	}
}
