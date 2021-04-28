using System;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

public class TestNetworkVariableSpam : NetworkBehaviour
{
    public NetworkVariable<float> Value01 = new NetworkVariable<float>(
        new NetworkVariableSettings {
            WritePermission = NetworkVariablePermission.ServerOnly,
            ReadPermission = NetworkVariablePermission.OwnerOnly
            });
    

    public override void NetworkStart()
    {
        base.NetworkStart();
        Value01.OnValueChanged += HandleValueChanged;
    }

    private void HandleValueChanged(float previousValue, float newValue) => Debug.Log("OnValueChanged previousValue=" + previousValue + " newValue=" + newValue);

    public void ClientRequestData() => ClientRequestDataServerRpc();

    [ServerRpc]
    private void ClientRequestDataServerRpc()
    {
        Value01.Value += 1f;
    }
}