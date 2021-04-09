using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : MonoBehaviour
{
    private Connector connector;

    private void Awake() => connector = GetComponent<Connector>();

    private void Start() => connector.ExecuteWhenNetworkHasStarted(Initialize);
    
    private void Initialize()
    {
        connector.Value01.Value = 123f;
        // Debug.Log("Initialized Value01=" + connector.Value01.Value);
        connector.Value02 = 456f;
        // Debug.Log("Initialized Value02=" + connector.Value02);
        connector.Value03 = new SerializableItem { Value = 789f };
        // Debug.Log("Initialized Value03=" + connector.Value03.Value);
    }
}
