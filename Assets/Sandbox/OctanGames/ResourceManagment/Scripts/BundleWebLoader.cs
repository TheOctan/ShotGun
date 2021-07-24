using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace OctanGames
{
    public class BundleWebLoader : MonoBehaviour
    {
        [SerializeField] private string bundleUrl = "http://localhost/assetbundles/testbundle";
        [SerializeField] private string assetName = "BundledSpriteObject";

        private uint version = 0;

        private IEnumerator Start()
    	{
            var request = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl, version);

            yield return request.SendWebRequest();

            AssetBundle remoteAssetBundle = DownloadHandlerAssetBundle.GetContent(request);

            if (remoteAssetBundle == null)
            {
                Debug.Log("Failed to download AssetBundle!");
                yield break;
            }

            AssetBundleRequest assetRequest = remoteAssetBundle.LoadAssetAsync<GameObject>(assetName);
            yield return assetRequest;

            GameObject prefab = assetRequest.asset as GameObject;
            Instantiate(prefab);
        }
    }
}
