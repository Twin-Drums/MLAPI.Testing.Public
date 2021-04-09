using System;
using System.Collections;
using MLAPI;
using UnityEngine;

public class ConnectionHandler : MonoBehaviour
{
    private void Awake()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
    }

    private void HandleClientDisconnect(ulong clientId) => throw new NotImplementedException();

    private void ApprovalCheck(byte[] connectionData,
                               ulong clientId,
                               MLAPI.NetworkManager.ConnectionApprovedDelegate callback)
    {
        StartCoroutine(RunApprovalCheck(connectionData, clientId, callback));
    }

    private IEnumerator RunApprovalCheck(byte[] connectionData,
                                         ulong clientId,
                                         MLAPI.NetworkManager.ConnectionApprovedDelegate callback)
    {
        yield return new WaitForSeconds(0.2f);
        callback(true, null, true, null, null);
    }
}