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
    
    public event Action<IdName[]> OnArrayReceived = delegate { };
    
    public event Action<int> OnFixedListVariableChanged = delegate {  };

    public FixedList FixedListVariable { get => fixedListVariable.Value; set {  fixedListVariable.Value = value; fixedListVariable.SetDirty(true); } }
    private NetworkVariable<FixedList> fixedListVariable;
    

    private void Awake()
    {
        Value01 = new NetworkList<UnmanagedTestContainer>();
        fixedListVariable = new NetworkVariable<FixedList>();
    }

    string customMessageTestName = "custommessage.test";
    string customMessageArrayTestName = "custommessage.arraytest";

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkHasStarted = true;
        Value01.OnListChanged += OnListChanged;
        fixedListVariable.OnValueChanged += OnFixedListChanged;
        NetworkManager.CustomMessagingManager.RegisterNamedMessageHandler(customMessageTestName, HandleCustomMessageReceived);
        RegisterNamedMessageWithArrayPayload(customMessageArrayTestName, OnArrayReceived);
    }

    private void OnFixedListChanged(FixedList previousvalue, FixedList newvalue) => OnFixedListVariableChanged(newvalue.testList.Length);


    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        Value01.OnListChanged -= OnListChanged;
        NetworkManager.CustomMessagingManager.UnregisterNamedMessageHandler(customMessageTestName);
        NetworkManager.CustomMessagingManager.UnregisterNamedMessageHandler(customMessageArrayTestName);
    }

    private void OnListChanged(NetworkListEvent<UnmanagedTestContainer> changeevent) { OnList01Changed(changeevent.Value); }

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


    public const int NON_FRAGMENTED_MESSAGE_MAX_SIZE = 1300;
    public const int FRAGMENTED_MESSAGE_MAX_SIZE = int.MaxValue;


    protected void RegisterNamedMessageWithArrayPayload<T>(string messageName, Action<T[]> callback) where T : INetworkSerializable, new()
    {
        NetworkManager.CustomMessagingManager.RegisterNamedMessageHandler(messageName, delegate(ulong id, FastBufferReader reader)
        {
            reader.ReadNetworkSerializable(out T[] payload);
            callback(payload);
        });
    }

    public void SendPayloadWithArray<T>(T[] payload) where T : INetworkSerializable
    {
        using FastBufferWriter writer = new FastBufferWriter(NON_FRAGMENTED_MESSAGE_MAX_SIZE, Allocator.Temp, FRAGMENTED_MESSAGE_MAX_SIZE);
        writer.WriteNetworkSerializable(payload);
        NetworkManager.CustomMessagingManager.SendNamedMessage(customMessageArrayTestName, OwnerClientId, writer);
    }
}