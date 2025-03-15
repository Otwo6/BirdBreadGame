using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class RelayManager : MonoBehaviour
{

    [SerializeField] Button hostButton;
    [SerializeField] Button joinButton;
    [SerializeField] TMP_InputField joinInput;
    [SerializeField] TextMeshProUGUI codeText;
    [SerializeField] TMP_InputField nameInput;

	[SerializeField] GameObject joinWidget;
	[SerializeField] GameObject playerHUD;

    // Start is called before the first frame update
    async void Start() {
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        hostButton.onClick.AddListener(CreateRelay);
        joinButton.onClick.AddListener(() => JoinRelay(joinInput.text));
    }

    // Update is called once per frame
    async void CreateRelay() {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
		joinWidget.SetActive(false);
		playerHUD.SetActive(true);
		codeText.text = "Code: " + joinCode; 

        var relayServerData = new RelayServerData(allocation, "dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartHost();
        SetName();
    }

    async void JoinRelay(string joinCode) {
        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        var relayServerData = new RelayServerData(joinAllocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartClient();
		joinWidget.SetActive(false);
		playerHUD.SetActive(true);
		codeText.text = "Code: " + joinCode; 
        SetName();
    }

   void SetName()
    {
        // Step 1: Get the local client from the NetworkManager
        var localClient = NetworkManager.Singleton.LocalClient;
        if (localClient == null)
        {
            print("LocalClient is null. Cannot proceed.");
            return;
        }

        // Step 2: Get the PlayerObject from the LocalClient
        var playerObject = localClient.PlayerObject;
        if (playerObject == null)
        {
            print("PlayerObject is null. Cannot proceed.");
            return;
        }

        // Step 3: Get the GameObject from the PlayerObject
        GameObject playerChar = playerObject.gameObject;
        if (playerChar == null)
        {
            print("PlayerChar (GameObject) is null. Cannot proceed.");
            return;
        }

        // Step 4: Get the PlayerStats component from the playerChar
        PlayerStats stats = playerChar.GetComponent<PlayerStats>();
        if (stats == null)
        {
            print("PlayerStats component is null. Cannot proceed.");
            return;
        }

        // Step 5: Check if nameInput is not null and if the text field is not empty
        if (nameInput != null)
        {
            if (!string.IsNullOrEmpty(nameInput.text))
            {
                // If everything is valid, set the playerName
                stats.playerName.Value = nameInput.text;
            }
            else
            {
                print("Name input is empty. Cannot set player name.");
            }
        }
        else
        {
            print("NameInput is null. Cannot set player name.");
        }
    }

}
