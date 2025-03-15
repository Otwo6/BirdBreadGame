using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	public bool hasBread = false; // public for testing set to private when finished

	public GameObject breadHat;

	public void SetHasBread(bool has)
	{
		if(has)
		{
			hasBread = true;
			breadHat.SetActive(true);
		}
		else
		{
			hasBread = false;
			breadHat.SetActive(false);
		}
	}

	public bool GetHasBread()
	{
		return hasBread;
	}
}
