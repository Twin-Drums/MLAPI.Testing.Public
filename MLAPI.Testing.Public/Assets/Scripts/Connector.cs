using System;
using MLAPI;
using MLAPI.NetworkVariable;

public class Connector : NetworkBehaviour
{
    public event Action<Connector> OnNetworkStart = delegate { };
    public event Action<float, float> OnValue02Changed = delegate { };
    public event Action<SerializableItem, SerializableItem> OnValue03Changed = delegate { };

    public bool NetworkHasStarted { get; private set; }

    public NetworkVariable<float> Value01 = new NetworkVariable<float>(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.ServerOnly, ReadPermission = NetworkVariablePermission.OwnerOnly });
    
    public float Value02 { get => value02.Value; set => value02.Value = value; }
    private NetworkVariable<float> value02 = new NetworkVariable<float>(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.ServerOnly, ReadPermission = NetworkVariablePermission.OwnerOnly });

    public SerializableItem Value03 { get => value03.Value; set => value03.Value = value; }
    private NetworkVariable<SerializableItem> value03 = new NetworkVariable<SerializableItem>(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.ServerOnly, ReadPermission = NetworkVariablePermission.OwnerOnly });

    public override void NetworkStart()
    {
        base.NetworkStart();
        NetworkHasStarted = true;
        OnNetworkStart(this);
        OnNetworkStart = delegate { };
        Initialize();                
    }

    private void Initialize()
    {
        value02.OnValueChanged += (previous, current) => OnValue02Changed(previous, current);
        value03.OnValueChanged += (previous, current) => OnValue03Changed(previous, current);
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