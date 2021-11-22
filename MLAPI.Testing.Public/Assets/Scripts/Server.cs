using UnityEngine;
public class Server : MonoBehaviour
{
    private Connector connector;

    private void Awake() => connector = GetComponent<Connector>();

    private void Start() => connector.ExecuteWhenNetworkHasStarted(Initialize);

    private void Initialize()
    {
        InvokeRepeating($"SlowAdder", 1f,1f);
    }
    private void SlowAdder()
    {
        connector.Value01.Add(Random.Range(0,100));
    }
}
