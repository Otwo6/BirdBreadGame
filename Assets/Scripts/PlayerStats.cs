using System.Collections;
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerStats : NetworkBehaviour
{
    public NetworkVariable<FixedString128Bytes> playerName = new NetworkVariable<FixedString128Bytes>("Player");
    public TMP_Text nameText;
    public string characterName;

    void Update()
    {
        characterName = playerName.Value.ToString();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerNameServerRpc(string newName)
    {
        playerName.Value = newName;
    }
}