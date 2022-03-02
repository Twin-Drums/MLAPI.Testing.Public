using System;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Connector : NetworkBehaviour
{
    public event Action<Connector> OnNetworkStart = delegate { };

    public event Action<UnmanagedTestContainer> OnList01Changed = delegate { };
    public bool NetworkHasStarted { get; private set; }

    public NetworkList<UnmanagedTestContainer> Value01;


    private void Awake() { Value01 = new NetworkList<UnmanagedTestContainer>(); }

    string customMessageTestName = "custommessage.test";

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkHasStarted = true;
        Value01.OnListChanged += OnListChanged;

        NetworkManager.CustomMessagingManager.RegisterNamedMessageHandler(customMessageTestName, HandleCustomMessageReceived);
    }



    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        Value01.OnListChanged -= OnListChanged;
        NetworkManager.CustomMessagingManager.UnregisterNamedMessageHandler(customMessageTestName);
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

    private void HandleCustomMessageReceived(ulong sender, FastBufferReader payload)
    {
        payload.ReadNetworkSerializable(out CustomMessageType combatSetupSummary);
        Debug.Log($"[{nameof(Connector)}::{nameof(HandleCustomMessageReceived)}] Received custom message: {combatSetupSummary}");
    }


    public void SendCustomMessage(CustomMessageType message)
    {
        using FastBufferWriter writer = new FastBufferWriter(1000, Allocator.Temp, 10000000);
        writer.WriteNetworkSerializable(message);
        NetworkManager.CustomMessagingManager.SendNamedMessage(customMessageTestName, OwnerClientId, writer, NetworkDelivery.Reliable);
    }
}