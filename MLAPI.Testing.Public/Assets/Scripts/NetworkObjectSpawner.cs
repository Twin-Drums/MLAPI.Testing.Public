using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Code.Server.Visibility
{
    public class NetworkObjectSpawner : MonoBehaviour
    {
        [SerializeField] private NetworkObject networkVisibilityObject;
        [SerializeField] private int Amount;

        private List<NetworkObject> spawnedObjects = new List<NetworkObject>();


        private IEnumerator Start()
        {
            while (!NetworkManager.Singleton.IsServer)
            {
                yield return null;
            }

            SpawnNetworkVisibilityObjects();

            InvokeRepeating(nameof(RespawnNetworkObjects), 3f, 3f);
        }

        private void SpawnNetworkVisibilityObjects()
        {
            for (int i = 0; i < Amount; i++)
            {
                SpawnNetworkVisibilityObject();
            }
        }

        private void RespawnNetworkObjects()
        {
            SpawnNetworkVisibilityObjects();
        }

        private void SpawnNetworkVisibilityObject()
        {
            var networkVisibilityObjectInstance = Instantiate(networkVisibilityObject, transform);
            networkVisibilityObjectInstance.GetComponent<NetworkObject>().Spawn();

            spawnedObjects.Add(networkVisibilityObjectInstance);
        }
    }
}
