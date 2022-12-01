using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// this script could be adjusted for playing 2 different sounds for each team (so players know if it was an enemy ability or teammate ability)

public class NetworkSpawnObject : NetworkBehaviour
{
    public GameObject prefab;
    public Transform parent;

    public void SpawnNetworkedObject()
    {
        Instantiate(prefab, parent);
        SpawnObjectServerRPC();
    }

    [ServerRpc] // client tells server to do something
    private void SpawnObjectServerRPC()
    {
        SpawnObjectClientRPC();
    }

    [ClientRpc] // server telling the clients to do something
    private void SpawnObjectClientRPC()
    {
        if (IsOwner) return; // ignore client that shot ball since force was already added
        Instantiate(prefab, parent);
    }
}
