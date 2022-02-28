using Unity.Netcode;
using UnityEngine;

namespace Code.Server.Visibility
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkVisibilityObject : NetworkBehaviour
    {

        private NetworkObject networkedObject;

        private void Awake()
        {
            networkedObject = GetComponent<NetworkObject>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            networkedObject.CheckObjectVisibility += HandleCheckObjectVisibility;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            networkedObject.CheckObjectVisibility -= HandleCheckObjectVisibility;
        }


        protected virtual bool HandleCheckObjectVisibility(ulong clientId) => false;

    }
}
