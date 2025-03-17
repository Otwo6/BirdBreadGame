using UnityEngine;
using Unity.Netcode;

public class KonamiCodeChecker : NetworkBehaviour
{
    public MeshFilter head;
    public Mesh bluejayHead;
    public Renderer headMeshRen;
    public Material headMat;

	public Material bodyMat;
	public Material bodyWhiteMat;
	public Material beakMat;

	public Renderer bodyMeshRen;
	public Renderer LWingMeshRen;
	public Renderer RWingMeshRen;
	public Renderer LLegMeshRen;
	public Renderer RLegMeshRen;

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

        ChangeMeshServerRpc();
    }

    [ServerRpc(RequireOwnership=false)]
    private void ChangeMeshServerRpc()
    {
        if(IsOwner)
        {
            ChangeMesh();
            ChangeMeshClientRpc();
        }
    }

    [ClientRpc]
    private void ChangeMeshClientRpc()
    {
        ChangeMesh();
    }

	private void ChangeMesh()
	{
		head.mesh = bluejayHead;
        Material[] materials = headMeshRen.materials;
        materials[0] = headMat;
		materials[1] = beakMat;
        headMeshRen.materials = materials; 

		Material[] bodyMaterials = bodyMeshRen.materials;
		bodyMaterials[0] = bodyMat;
		bodyMaterials[1] = bodyWhiteMat;
		bodyMeshRen.materials = bodyMaterials;

		LLegMeshRen.material = beakMat;
		RLegMeshRen.material = beakMat;
		LWingMeshRen.material = bodyMat;
		RWingMeshRen.material = bodyMat;
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
