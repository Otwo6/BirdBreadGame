using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GameManagerScript : NetworkBehaviour
{
	public float timeRemaining = 60f;  // Timer in seconds
	public bool timerIsRunning = false;

	public TMP_Text timerText;  // To display the countdown in UI (optional)

	private NetworkList<ulong> playersReady = new NetworkList<ulong>();

	void Start()
	{
		// Start the timer when the game begins
		if (timeRemaining > 0)
		{
			timerIsRunning = true;
		}

		NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerJoined;
	}

	void Update()
	{
		// If the timer is running
		if (timerIsRunning)
		{
			if (timeRemaining > 0)
			{
				timeRemaining -= Time.deltaTime;  // Subtract time
				//UpdateTimerUI();  // Update the UI with the remaining time
			}
			else
			{
				// Timer reaches zero
				timeRemaining = 0;
				timerIsRunning = false;
				TimerComplete();  // Call a function when the timer ends
			}
		}
	}

	void OnPlayerJoined(ulong clientId)
    {
        if (IsServer)  // Only the server can add players
        {
            playersReady.Add(clientId);
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

	void TimerComplete()
	{
		// Action to take when the timer reaches zero
		Debug.Log("Time's up!");
		// You can call any function here, like ending the game or triggering an event
	}

	public void CheckPlayersReady()
    {
        if (IsServer)
        {
            // Check if all players are ready
            foreach (var clientId in playersReady)
            {
                var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
                var playerInput = playerObject.GetComponent<PlayerInput>();

                if (!playerInput.isReady.Value)
                {
                    Debug.Log("Not all players are ready yet.");
                    return;  // At least one player is not ready
                }
            }

            // All players are ready, start the game
            Debug.Log("All players are ready! Starting the game...");
        }
    }
}
