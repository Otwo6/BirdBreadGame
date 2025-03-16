using System.Collections;
using System.Collections.Generic;
using Unity.Netcode; // Importing Netcode for GameObjects
using UnityEngine;

public class PlayerInventory : NetworkBehaviour
{
    public bool hasBread = false; // public for testing; set to private when finished
    public GameObject breadHat;

    // Use NetworkVariable to sync the bread state across the network
    private NetworkVariable<bool> networkHasBread = new NetworkVariable<bool>(false);

	bool canUpdate = true;

    public void SetHasBread(bool has)
    {
        if (IsServer) // Only the server should set the value
        {
			if(canUpdate)
			{
				networkHasBread.Value = has; // This will automatically sync across clients
            	SetBreadHatActiveOnClients(has); // Ensure the clients update the GameObject state
				SetCanUpdate();
                print("Worked now i have ? " + has);
			}
            print("phuck you");
        }
        else
        {
            print("nah");
        }
    }

	void Update()
	{
		if(Input.GetKeyDown("k"))
		{
			SetHasBread(true);
		}
	}

    public bool GetHasBread()
    {
        return networkHasBread.Value; // Return the synced value
    }

    // Update the state of the breadHat across clients
    private void SetBreadHatActiveOnClients(bool isActive)
    {
        breadHat.SetActive(isActive);
    }

    // Sync the breadHat's state on all clients using a ClientRpc
    [ClientRpc]
    private void SetBreadHatActiveClientRpc(bool isActive)
    {
        breadHat.SetActive(isActive);
    }

    // Update network variables from clients to server
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        if (IsOwner) // Only update for the local player
        {
            SetBreadHatActiveOnClients(networkHasBread.Value);
        }
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
