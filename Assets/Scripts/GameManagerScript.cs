using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GameManagerScript : NetworkBehaviour
{
	public NetworkVariable<float> timeRemaining = new NetworkVariable<float>(100f);
	public NetworkVariable<bool> timerIsRunning = new NetworkVariable<bool>(false);

	NetworkVariable<float> countdownTimeRemaining = new NetworkVariable<float>(6f);
	NetworkVariable<bool> countdownTimerRunning = new NetworkVariable<bool>(false);

	public TMP_Text timerText;
	public TMP_Text countdownText;

	private NetworkVariable<int> playersReady = new NetworkVariable<int>(0);

	void Update()
	{
		// If the timer is running
		if (timerIsRunning.Value)
		{
			if (timeRemaining.Value > 0)
			{
				if(IsServer)
				{
					timeRemaining.Value -= Time.deltaTime;  // Subtract time
				}
				UpdateTimerUI();  // Update the UI with the remaining time
			}
			else
			{
				// Timer reaches zero
				timeRemaining.Value = 0;
				timerIsRunning.Value = false;
				TimerComplete();  // Call a function when the timer ends
			}
		}

		if (countdownTimerRunning.Value)
		{
			if (countdownTimeRemaining.Value > 1f)
			{
				if(IsServer)
				{
					countdownTimeRemaining.Value -= Time.deltaTime;  // Subtract time
				}
				UpdateCountdownUI();  // Update the UI with the remaining time
			}
			else
			{
				// Timer reaches zero
				countdownTimeRemaining.Value = 0;
				countdownTimerRunning.Value = false;
				ClearCountdownUI();
				CountDownComplete();  // Call a function when the timer ends
			}
		}
	}

	void UpdateTimerUI()
	{
		// Format the time as minutes and seconds, e.g., "02:45"
		float minutes = Mathf.Floor(timeRemaining.Value / 60);
		float seconds = timeRemaining.Value % 60;
		string timeFormatted = string.Format("{0:00}:{1:00}", minutes, seconds);

		// Update the UI text (if there is a UI element set)
		if (timerText != null)
		{
			timerText.text = timeFormatted;
		}
	}

	void UpdateCountdownUI()
	{
		int time = (int)countdownTimeRemaining.Value;
		if (countdownText != null)
		{
			countdownText.text = time.ToString();
		}
	}

	void ClearCountdownUI()
	{
		if (countdownText != null)
		{
			countdownText.text = "";
		}
	}

	void TimerComplete()
	{
		// Action to take when the timer reaches zero
		Debug.Log("Time's up!");
		
		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject player in allPlayers)
		{
			if(player.GetComponent<PlayerInventory>().GetHasBread())
			{
				countdownText.text = (player.GetComponent<PlayerStats>().characterName + " has won");
				break;
			}
		}

		timerIsRunning.Value = false;
		timeRemaining.Value = 100.0f;
		countdownTimerRunning.Value = false;
		countdownTimeRemaining.Value = 6.0f;
		timerText.text = "Waiting For Players To Ready";

		foreach(GameObject player in allPlayers)
		{
			player.GetComponent<PlayerInput>().SetReadyServerRpc(false);
			player.GetComponent<PlayerInventory>().ClearHasBreadServerRpc();
		}
	}

	void CountDownComplete()
	{
		timerIsRunning.Value = true;
		GiveFirstBread();
	}

	[ServerRpc(RequireOwnership = false)]
	void CheckPlayersReadyServerRpc()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		bool everyoneReady = true;
		for(int i = 0; i < players.Length; i++)
		{
			if(!players[i].GetComponentInParent<PlayerInput>().isReady.Value)
			{
				everyoneReady = false;
				break;
			}
		}

		if(everyoneReady)
		{
			NotifyClientsClientRpc("Start the game were all ready");
			StartGame();
		}
		else
		{
			NotifyClientsClientRpc("We stay waiting");
		}
	}

	[ClientRpc]
    void NotifyClientsClientRpc(string message)
    {
        print(message);
    }

	public void CheckPlayersReady()
    {
        CheckPlayersReadyServerRpc();
    }

	void StartGame()
	{
		countdownTimerRunning.Value = true;
		
	}

	void GiveFirstBread()
	{
		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

		int firstBread = Random.Range(0, allPlayers.Length);

		allPlayers[firstBread].GetComponentInParent<PlayerInventory>().SetHasBreadServerRpc(true);
	}
}
