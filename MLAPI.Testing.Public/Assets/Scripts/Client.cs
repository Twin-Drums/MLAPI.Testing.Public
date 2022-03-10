using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

public class Client : MonoBehaviour
{
    private Connector connector;

    private void Awake() => connector = GetComponent<Connector>();

    private void Start() => connector.ExecuteWhenNetworkHasStarted(Initialize);

    private void Initialize()
    {
        Debug.Log("Value01=" + connector.Value01);
        connector.OnList01Changed += HandleList01Changed;
        connector.OnArrayReceived += HandleArrayReceived;
    }

    private void OnDestroy()
    {
        connector.OnList01Changed -= HandleList01Changed;
        connector.OnArrayReceived -= HandleArrayReceived;
    }

    private void HandleList01Changed(UnmanagedTestContainer obj)
    {
        Debug.Log("new value entry: " + obj.Value01 + ", " + obj.Value02 + ", " + obj.Value03 + " list size:  " +
                  connector.Value01.Count);
    }
    
    
    private void HandleArrayReceived<T>(T[] obj) where T : INetworkSerializable, new()
    {
        foreach (T networkSerializable in obj)
        {
            string s = networkSerializable.ToString();
            Debug.Log($"[{nameof(Connector)}::{nameof(HandleArrayReceived)}] s");
        }
    }
}