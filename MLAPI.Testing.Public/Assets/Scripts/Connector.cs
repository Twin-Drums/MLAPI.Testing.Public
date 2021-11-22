using System;
using Unity.Netcode;
using UnityEngine;
public class Connector : NetworkBehaviour
{
    public event Action<Connector> OnNetworkStart = delegate { };

    public event Action<float> OnList01Changed = delegate { };
    public bool NetworkHasStarted { get; private set; }

    public NetworkList<float> Value01;


    private void Awake()
    {
        Value01 = new NetworkList<float>();
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

    private void OnListChanged(NetworkListEvent<float> changeevent)
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
