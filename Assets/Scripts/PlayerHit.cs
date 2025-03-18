using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    PlayerInventory myInv;
    PlayerAudioManager myAud;

    void Start()
    {
        myInv = GetComponent<PlayerInventory>();
        myAud = GetComponent<PlayerAudioManager>();
    }

    void OnCollisionEnter(Collision col)
    {
        GameObject otherPlayer = col.gameObject;
        myAud.PlayHitWallSound(transform.position);
        if (otherPlayer.CompareTag("Player"))
        {
            Debug.Log("Hit a player");

            PlayerInventory hitInv = otherPlayer.GetComponentInParent<PlayerInventory>();
            myAud.PlayHitBirdSound(transform.position);

            if (hitInv.GetHasBread())
            {
                // Transfer bread from hit player to this player
                hitInv.SetHasBreadServerRpc(false); // Update on the server
                myInv.SetHasBreadServerRpc(true);   // Update on the server

                Debug.Log($"I {gameObject.name} take the bread");
            }
            else if (myInv.GetHasBread())
            {
                // Transfer bread from this player to hit player
                myInv.SetHasBreadServerRpc(false); // Update on the server
                hitInv.SetHasBreadServerRpc(true);  // Update on the server

                Debug.Log($"Give {otherPlayer.name} the bread");
            }
        }
		else
		{
            // Not a player
			print("Hit " + otherPlayer);
            GetComponent<PlayerMovement>().StopVelocity();
		}
    }
}
