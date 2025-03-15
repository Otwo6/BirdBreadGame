using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
	PlayerInventory myInv;

	void Start()
	{
		myInv = GetComponent<PlayerInventory>();
	}

	void OnCollisionEnter(Collision col)
	{
		GameObject otherPlayer = col.gameObject;
		if(otherPlayer.tag == "Player")
		{
			print("Hit a player");
			PlayerInventory hitInv = otherPlayer.GetComponentInParent<PlayerInventory>();

			if(hitInv.GetHasBread())
			{
				hitInv.SetCanUpdate();
				myInv.SetCanUpdate();
				hitInv.SetHasBread(false);
				myInv.SetHasBread(true);

				print("I " + gameObject + " take the bread");
			}
			else if(myInv.GetHasBread())
			{
				myInv.SetCanUpdate();
				hitInv.SetCanUpdate();
				myInv.SetHasBread(false);
				hitInv.SetHasBread(true);

				print("give " + otherPlayer + " the bread");
			}
		}
	}
}
