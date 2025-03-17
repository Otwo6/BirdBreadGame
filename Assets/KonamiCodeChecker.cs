using UnityEngine;
using Unity.Netcode;

public class KonamiCodeChecker : NetworkBehaviour
{
    public MeshFilter head;
    public Mesh bluejayHead;
    public Renderer headMeshRen;
    public Material headMat;

    // The Konami Code sequence (Up, Up, Down, Down, Left, Right, Left, Right, B, A)
    private KeyCode[] konamiCode = new KeyCode[]
    {
        KeyCode.UpArrow, KeyCode.UpArrow,
        KeyCode.DownArrow, KeyCode.DownArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.B, KeyCode.A
    };

    private int currentIndex = 0;

    void Update()
    {
        // Check if the current key input matches the next Konami Code input
        if (Input.anyKeyDown)
        {
            KeyCode keyPressed = GetKeyPressed();
            
            // If the key matches the expected key in the Konami Code
            if (keyPressed == konamiCode[currentIndex])
            {
                currentIndex++;

                // If the entire Konami Code is entered correctly
                if (currentIndex == konamiCode.Length)
                {
                    OnKonamiCodeEntered();
                    currentIndex = 0; // Reset the index to allow the code to be entered again
                }
            }
            else
            {
                currentIndex = 0; // Reset the sequence if the wrong key is pressed
            }
        }
    }

    // Method to handle what happens when the Konami Code is entered
    private void OnKonamiCodeEntered()
    {
        Debug.Log("Konami Code entered! You've unlocked the secret!");
        // You can trigger any special event here, e.g., unlocking a cheat, a secret level, etc.

        NotifyServerOfKonamiCodeEnteredServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void NotifyServerOfKonamiCodeEnteredServerRpc(ServerRpcParams rpcParams = default)
    {
        // Call the ClientRpc to change the mesh for the player who entered the code
        ChangeMeshClientRpc(rpcParams.Receive.SenderClientId);
    }

    // ClientRpc to change the player's mesh
    [ClientRpc]
    private void ChangeMeshClientRpc(ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            // Change the mesh and material only for the client who entered the code
            head.mesh = bluejayHead;
            Material[] materials = headMeshRen.materials;
            materials[0] = headMat;
            headMeshRen.materials = materials;
        }
    }

    // Get the key that was pressed
    private KeyCode GetKeyPressed()
    {
        // Check which key was pressed
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                return key;
            }
        }
        return KeyCode.None; // Return None if no key was pressed
    }
}
