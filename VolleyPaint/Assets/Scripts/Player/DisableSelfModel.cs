using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DisableSelfModel : NetworkBehaviour
{
    public GameObject model;
    public GameObject meshRoot;
    public GameObject gunHandler;

    private void Start()
    {
        if (!IsOwner) return;

        model.SetActive(false);
        meshRoot.SetActive(false);
        gunHandler.SetActive(true);
    }
}
