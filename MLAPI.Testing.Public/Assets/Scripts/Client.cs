using System;
using System.Collections;
using System.Collections.Generic;
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
    private void HandleList01Changed(float obj)
    {
        Debug.Log("new value entry: " + obj);
    }
}
