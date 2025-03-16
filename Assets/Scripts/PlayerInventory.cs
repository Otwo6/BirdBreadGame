using System.Collections;
using System.Collections.Generic;
using Unity.Netcode; // Importing Netcode for GameObjects
using UnityEngine;

public class PlayerInventory : NetworkBehaviour
{
    public GameObject breadHat;

    // Use NetworkVariable to sync the bread state across the network
    private NetworkVariable<bool> networkHasBread = new NetworkVariable<bool>(false);

    private bool canUpdate = true;

    // ServerRpc to transfer the bread state
    [ServerRpc(RequireOwnership = false)]
    public void SetHasBreadServerRpc(bool has)
    {
        if (canUpdate)
        {
            networkHasBread.Value = has; // This will automatically sync across clients
            SetBreadHatActiveOnClientsClientRpc (has); // Ensure the clients update the GameObject state
            SetCanUpdate();
            Debug.Log($"Server updated bread state to: {has}");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClearHasBreadServerRpc()
    {
        networkHasBread.Value = false; // This will automatically sync across clients
        SetBreadHatActiveOnClientsClientRpc (false); // Ensure the clients update the GameObject state
        Debug.Log($"Server updated bread state to: {false}");
    }

    // ClientRpc to update bread hat state for all clients
    [ClientRpc]
    public void SetBreadHatActiveOnClientsClientRpc(bool has)
    {
        if (breadHat != null)
        {
            breadHat.SetActive(has);
        }
    }

    public bool GetHasBread()
    {
        return networkHasBread.Value; // Return the synced value
    }

    void SetCanUpdate()
    {
        canUpdate = false;
        StartCoroutine(ResetCanUpdateAfterDelay());
    }

    private IEnumerator ResetCanUpdateAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        canUpdate = true;
    }
}
