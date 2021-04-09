using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    private Connector connector;

    private void Awake() => connector = GetComponent<Connector>();

    private void OnEnable() => connector.OnValue02Changed += HandleValue02Changed;
    
    private void Start() => connector.ExecuteWhenNetworkHasStarted(Initialize);

    private void Initialize()
    {
        Debug.Log("Value01=" + connector.Value01.Value);
        connector.Value01.OnValueChanged += HandleValue01Changed;
        Debug.Log("Value02=" + connector.Value02);
        Debug.Log("Value03=" + connector.Value03 + " Value03.Value=" + (connector.Value03 != null ? connector.Value03.Value.ToString() : ""));
    }

    private void HandleValue01Changed(float previousValue, float newValue)
    {
        Debug.Log("HandleValue01Changed Value01=" + connector.Value01.Value + " previousValue=" + previousValue + " newValue=" + newValue);
    }

    private void HandleValue02Changed(float previousValue, float newValue)
    {
        Debug.Log("HandleValue02Changed Value02=" + connector.Value02 + " previousValue=" + previousValue + " newValue=" + newValue);
    }
}
