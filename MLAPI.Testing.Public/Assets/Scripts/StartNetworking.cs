using Unity.Netcode;
using UnityEngine;
public class StartNetworking : MonoBehaviour
{
    public enum NetworkingTypeId { Client, Server }

    [SerializeField] public NetworkingTypeId networkingType;

    private void Start()
    {
        switch (networkingType)
        {
            case NetworkingTypeId.Client:
                NetworkManager.Singleton.StartClient();
                break;
            case NetworkingTypeId.Server:
                NetworkManager.Singleton.StartServer();
                break;
        }
    }
}
