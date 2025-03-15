using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GameManagerScript : NetworkBehaviour
{
	public float timeRemaining = 60f;  // Timer in seconds
	public bool timerIsRunning = false;

	float countdownTimeRemaining = 5f;
	bool countdownTimerRunning = false;

	public TMP_Text timerText;
	public TMP_Text countdownText;

	private NetworkVariable<int> playersReady = new NetworkVariable<int>(0);

	void Update()
	{
		// If the timer is running
		if (timerIsRunning)
		{
			if (timeRemaining > 0)
			{
				timeRemaining -= Time.deltaTime;  // Subtract time
				UpdateTimerUI();  // Update the UI with the remaining time
			}
			else
			{
				// Timer reaches zero
				timeRemaining = 0;
				timerIsRunning = false;
				TimerComplete();  // Call a function when the timer ends
			}
		}

		if (countdownTimerRunning)
		{
			if (countdownTimeRemaining > 0f)
			{
				countdownTimeRemaining -= Time.deltaTime;  // Subtract time
				UpdateTimerUI();  // Update the UI with the remaining time
			}
			else
			{
				// Timer reaches zero
				countdownTimeRemaining = 0;
				countdownTimerRunning = false;
				CountDownComplete();  // Call a function when the timer ends
			}
		}
	}

	void UpdateTimerUI()
	{
		// Format the time as minutes and seconds, e.g., "02:45"
		float minutes = Mathf.Floor(timeRemaining / 60);
		float seconds = timeRemaining % 60;
		string timeFormatted = string.Format("{0:00}:{1:00}", minutes, seconds);

		// Update the UI text (if there is a UI element set)
		if (timerText != null)
		{
			timerText.text = timeFormatted;
		}
	}

	void UpdateCountdownUI()
	{
		if (countdownText != null)
		{
			countdownText.text = countdownTimeRemaining.ToString();
		}
	}

	void TimerComplete()
	{
		// Action to take when the timer reaches zero
		Debug.Log("Time's up!");
		// You can call any function here, like ending the game or triggering an event
	}

	void CountDownComplete()
	{
		timerIsRunning = true;
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
		countdownTimerRunning = true;
	}
}
