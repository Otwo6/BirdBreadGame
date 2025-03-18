using UnityEngine;
using Unity.Netcode;

public class PlayerAudioManager : NetworkBehaviour
{
    public AudioClip hitWall;

    void Start()
    {
        if(IsOwner)
        {
            gameObject.AddComponent<AudioListener>();
        }
    }

    public void PlayHitWallSound(Vector3 loc)
    {
        PlayHitWallSoundServerRpc(loc);
    }

    [ClientRpc]
    void PlayHitWallSoundClientRpc(Vector3 loc)
    {
        AudioSource.PlayClipAtPoint(hitWall, loc, 1.0f);
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayHitWallSoundServerRpc(Vector3 loc)
    {
        PlayHitWallSoundClientRpc(loc);
    }
}
