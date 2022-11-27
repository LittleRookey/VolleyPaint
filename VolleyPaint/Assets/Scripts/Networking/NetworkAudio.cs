using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;
using Unity.Netcode;

// this script could be adjusted for playing 2 different sounds for each team (so players know if it was an enemy ability or teammate ability)

public class NetworkAudio : NetworkBehaviour
{
    //private bool sameTeam = true;

    public void PlayNetworkedAudio(string fileName, Vector3 pos)
    {
        MasterAudio.PlaySound3DAtVector3(fileName, pos);

        PlayNetworkedAudioServerRPC(fileName, pos);
    }

    [ServerRpc] // client tells server to do something
    private void PlayNetworkedAudioServerRPC(string fileName, Vector3 pos)
    {
        PlayNetworkedAudioClientRPC(fileName, pos);
    }

    [ClientRpc] // server telling the clients to do something
    private void PlayNetworkedAudioClientRPC(string fileName, Vector3 pos)
    {
        if (IsOwner) return; // ignore client that shot ball since force was already added
        MasterAudio.PlaySound3DAtVector3(fileName, pos);
    }
}
