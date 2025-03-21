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
    [SerializeField] GameObject startCam;

    [HideInInspector] public float playerSens = 50.0f; 

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
        Destroy(startCam);
		codeText.text = "Code: " + joinCode; 

        var relayServerData = new RelayServerData(allocation, "dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartHost();
        SetName();
        SetSens();
    }

    async void JoinRelay(string joinCode) {
        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        var relayServerData = new RelayServerData(joinAllocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartClient();
		joinWidget.SetActive(false);
		playerHUD.SetActive(true);
        Destroy(startCam);
		codeText.text = "Code: " + joinCode; 
        StartCoroutine(DelaySetName());
        StartCoroutine(DelaySetSens());
    }

    void SetName()
    {
        GameObject playerChar = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        if(playerChar != null)
        {
            PlayerStats stats = playerChar.GetComponent<PlayerStats>();

            if(stats != null && nameInput != null)
            {
                if(!string.IsNullOrEmpty(nameInput.text))
                {
                    stats.SetPlayerNameServerRpc(nameInput.text);
                }
                else
                {
                    stats.SetPlayerNameServerRpc("Player " + GameObject.FindGameObjectsWithTag("Player").Length);
                }
            }
            else
            {
                print("Doesn't wanna work bc screw you");
            }
        }
    }

    IEnumerator DelaySetName()
    {
        while (NetworkManager.Singleton.LocalClient.PlayerObject == null)
        {
            yield return null;
        }
    
        // Once PlayerObject is available, set the name
        SetName();
    }

    void SetSens()
    {
        GameObject playerChar = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        if(playerChar != null)
        {
            PlayerMovement move = playerChar.GetComponent<PlayerMovement>();

            if(move != null)
            {
                move.turnSpeed = playerSens;
            }
            else
            {
                print("Doesn't wanna work bc screw you");
            }
        }
    }

    IEnumerator DelaySetSens()
    {
        while (NetworkManager.Singleton.LocalClient.PlayerObject == null)
        {
            yield return null;
        }
    
        // Once PlayerObject is available, set the name
        SetSens();
    }
}
