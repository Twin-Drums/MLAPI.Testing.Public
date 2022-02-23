using DefaultNamespace;
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
    }
    private void HandleList01Changed(UnmanagedTestContainer obj)
    {
        Debug.Log("new value entry: " + obj.Value01 + ", " + obj.Value02 + ", " + obj.Value03 + " list size:  " + connector.Value01.Count);
    }
}
