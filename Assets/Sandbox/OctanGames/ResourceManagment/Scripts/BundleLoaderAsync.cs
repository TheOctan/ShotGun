using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OctanGames
{
	public class BundleLoaderAsync : MonoBehaviour
	{
		[SerializeField] private string assetName = "BundledSpriteObject";
		[SerializeField] private string bundleName = "testbundle";

		private IEnumerator Start()
		{
			AssetBundleCreateRequest asyncBundleRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, bundleName));

			yield return asyncBundleRequest;

			AssetBundle localAssetBundle = asyncBundleRequest.assetBundle;
			if (localAssetBundle == null)
			{
				Debug.Log("Failed to load AssetBundle!");
				yield break;
			}

			AssetBundleRequest assetRequest = localAssetBundle.LoadAssetAsync<GameObject>(assetName);
			yield return assetRequest;

			GameObject prefab = assetRequest.asset as GameObject;
			Instantiate(prefab);

			localAssetBundle.Unload(false);
		}
	}
}
