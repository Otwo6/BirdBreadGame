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
    public string name;

    void Start()
    {
        nameText = GameObject.FindWithTag("Respawn").GetComponent<TMP_Text>();
    }

    void Update()
    {
        nameText.text = playerName.Value.ToString();
        name = playerName.Value.ToString();
    }
}