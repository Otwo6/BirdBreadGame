using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
	public float timeRemaining = 60f;  // Timer in seconds
	public bool timerIsRunning = false;

	public TMP_Text timerText;  // To display the countdown in UI (optional)

	private NetworkVariable<int> playersReady = new NetworkVariable<int>(0);

	void Start()
	{
		// Start the timer when the game begins
		if (timeRemaining > 0)
		{
			timerIsRunning = true;
		}
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

	[ServerRpc]
	public void AddPlayerReady()
	{
		playersReady.Value++;
		CheckPlayersReady();
	}

	void CheckPlayersReady()
	{
		if(playersReady.Value == GameObject.FindGameObjectsWithTag("Player").Length)
		{
			print("WEE READY");
		}
		else
		{
			print("Didnt work bc " + playersReady + " And not " + GameObject.FindGameObjectsWithTag("Player").Length);
		}
	}

	[ServerRpc]
	public void RemovePlayerReady()
	{
		playersReady.Value--;
	}
}
