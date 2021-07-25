using System.IO;
using UnityEngine;

namespace OctanGames
{
	public class BundleObjectLoader : MonoBehaviour
	{
		[SerializeField] private string assetName = "BundledSpriteObject";
		[SerializeField] private string bundleName = "testbundle";

		private void Start()
		{
			AssetBundle localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
			if (localAssetBundle == null)
			{
				Debug.Log("Failed to load AssetBundle!");
				return;
			}
			
			GameObject prefab = localAssetBundle.LoadAsset<GameObject>(assetName);
			Instantiate(prefab);
			
			localAssetBundle.Unload(false);
		}
	}
}
