using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInput : NetworkBehaviour
{
    private GameManagerScript gameManager;

    // This will track the player's ready state on the server
    public NetworkVariable<bool> isReady = new NetworkVariable<bool>(false);

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    void Update()
    {
        // Handle the input for the ready state toggle
        if (Input.GetKeyDown(KeyCode.Y) && IsOwner)  // Ensure only the owner (player) can toggle
        {
            ToggleReadyState();
        }
    }

    // Toggle the ready state and notify the server to update it
    void ToggleReadyState()
    {
        if (isReady.Value)
        {
            isReady.Value = false;
            Debug.Log("Unready");
        }
        else
        {
            isReady.Value = true;
            Debug.Log("Ready");
        }

        // Notify the GameManager about the readiness change
        gameManager.CheckPlayersReady();
    }
}
