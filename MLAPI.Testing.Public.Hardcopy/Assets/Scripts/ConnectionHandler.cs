using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
public class ConnectionHandler : MonoBehaviour
{
    [SerializeField] float approvalDelay = 1f;

    private void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
    }

    private void HandleClientDisconnect(ulong clientId) => throw new NotImplementedException();

    private void ApprovalCheck(byte[] connectionData,
                               ulong clientId,
                               NetworkManager.ConnectionApprovedDelegate callback)
    {
        StartCoroutine(RunApprovalCheck(connectionData, clientId, callback));
    }

    private IEnumerator RunApprovalCheck(byte[] connectionData,
                                         ulong clientId,
                                         NetworkManager.ConnectionApprovedDelegate callback)
    {
        yield return new WaitForSeconds(approvalDelay);
        callback(true, null, true, null, null);
    }
}
