using System;
using MLAPI;

public class Connector : NetworkBehaviour
{
    public event Action<Connector> OnNetworkStart = delegate { };

    public bool NetworkHasStarted { get; private set; }

    public override void NetworkStart()
    {
        base.NetworkStart();
        NetworkHasStarted = true;
        OnNetworkStart(this);
        OnNetworkStart = delegate { };
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