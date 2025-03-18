using UnityEngine;
using Unity.Netcode;

public class PlayerAudioManager : NetworkBehaviour
{
    [SerializeField] AudioClip hitWall;
    [SerializeField] AudioClip hitBird;
    [SerializeField] AudioClip flapBird;

    public AudioSource windMove;
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

    public void PlayHitBirdSound(Vector3 loc)
    {
        PlayHitBirdSoundServerRpc(loc);
    }

    [ClientRpc]
    void PlayHitBirdSoundClientRpc(Vector3 loc)
    {
        AudioSource.PlayClipAtPoint(hitBird, loc, 1.0f);
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayHitBirdSoundServerRpc(Vector3 loc)
    {
        PlayHitBirdSoundClientRpc(loc);
    }

    public void PlayFlapSound(Vector3 loc)
    {
        PlayFlapSoundServerRpc(loc);
    }

    [ClientRpc]
    void PlayFlapSoundClientRpc(Vector3 loc)
    {
        AudioSource.PlayClipAtPoint(flapBird, loc, 1.0f);
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayFlapSoundServerRpc(Vector3 loc)
    {
        PlayFlapSoundClientRpc(loc);
    }
}
