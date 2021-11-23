using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Code.Server.Visibility
{
    public class NetworkObjectSpawner : MonoBehaviour
    {
        [SerializeField] private NetworkObject networkVisibilityObject;
        [SerializeField] private int Amount;


        private IEnumerator Start()
        {
            while (!NetworkManager.Singleton.IsServer)
            {
                yield return null;
            }

            for (int i = 0; i < Amount; i++)
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
