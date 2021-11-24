using System;
using DefaultNamespace;
using Unity.Netcode;
public class Connector : NetworkBehaviour
{
    public event Action<Connector> OnNetworkStart = delegate { };

    public event Action<UnmanagedTestContainer> OnList01Changed = delegate { };
    public bool NetworkHasStarted { get; private set; }

    public NetworkList<UnmanagedTestContainer> Value01;


    private void Awake()
    {
        Value01 = new NetworkList<UnmanagedTestContainer>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkHasStarted = true;
        Value01.OnListChanged += OnListChanged;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        Value01.OnListChanged -= OnListChanged;
    }

    private void OnListChanged(NetworkListEvent<UnmanagedTestContainer> changeevent)
    {
        OnList01Changed(changeevent.Value);
    }

    public void ExecuteWhenNetworkHasStarted(Action action) => ExecuteWhenNetworkHasStarted((connector) => action());

    public void ExecuteWhenNetworkHasStarted(Action<Connector> action)
    {
        if (NetworkHasStarted)
            action(this);
        else
            OnNetworkStart += action;
    }
}
