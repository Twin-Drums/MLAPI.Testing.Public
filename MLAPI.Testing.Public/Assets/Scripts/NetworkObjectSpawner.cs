using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Code.Server.Visibility
{
    public class NetworkObjectSpawner : MonoBehaviour
    {
        [SerializeField] private NetworkVisibilityObject networkVisibilityObject;

        private IEnumerator Start()
        {
            while (!NetworkManager.Singleton.IsServer)
            {
                yield return null;
            }

            for (int i = 0; i < 10; i++)
            {
                SpawnNetworkVisibilityObjects();
            }
        }
        private void SpawnNetworkVisibilityObjects()
        {
            var networkVisibilityObjectInstance = Instantiate(networkVisibilityObject, transform);
            networkVisibilityObjectInstance.GetComponent<NetworkObject>().Spawn();
        }
    }
}
