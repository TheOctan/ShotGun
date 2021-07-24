using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace OctanGames
{
    public class CreateAssetBundle
    {
        [MenuItem("Assets/Build AssetBundles")]
    	private static void BuildAllAssetBundles()
		{
            string assetBundleDirectory = "Assets/StreamingAssets";
			if (!Directory.Exists(Application.streamingAssetsPath))
			{
                Directory.CreateDirectory(assetBundleDirectory);
			}

            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
		}
    }
}
