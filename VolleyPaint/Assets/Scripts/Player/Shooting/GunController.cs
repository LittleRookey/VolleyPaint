using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using DarkTonic.MasterAudio;

public class GunController : NetworkBehaviour
{
    public List<GameObject> guns;
    public float gunChangeSpeed = 3f;

    [SerializeField] private int activeGunIndex; // gun slot 
    Vector3 originGunPos = new Vector3(0.665f, -0.26f, 1.435f);
    Vector3 holsterGunPos = new Vector3(0.665f, -2f, 1.435f);

    [Header("Debug")]
    public bool isTestingWithoutNetwork;

    private void Awake()
    {
        
        if (guns.Count == 0)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Debug.Log(transform.GetChild(i).gameObject.name);
                guns.Add(transform.GetChild(i).gameObject);
            }
        }
        foreach (GameObject gun in guns)
        {
            gun.gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        guns[activeGunIndex].gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isTestingWithoutNetwork)
            if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeGunMode(eShootType.Normal);
            ChangeGunModeServerRPC(eShootType.Normal);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeGunMode(eShootType.Jump);
            ChangeGunModeServerRPC(eShootType.Jump);
        }
    }

    public void ChangeGunMode(eShootType shootType)
    {
        // set player shoot type in PlayerShoot script
        transform.parent.parent.GetComponent<PlayerShoot>().shootType = shootType;

        guns[activeGunIndex].gameObject.SetActive(false);
        activeGunIndex = (int)shootType;
        guns[activeGunIndex].transform.localPosition = holsterGunPos;
        guns[activeGunIndex].gameObject.SetActive(true);
        MasterAudio.PlaySound3DAtVector3("Pistol Holster", transform.position);
        StartCoroutine(GunChange(guns[activeGunIndex]));
    }

    public void ReloadGunAnimation(float lapsedTime, float cooldownTime)
    {
        // If it's in the first half of the cooldown, bring the gun towards the player
        // Otherwise, bring it away from the player
        if (lapsedTime <= cooldownTime / 2)
        {
            guns[activeGunIndex].transform.localPosition += Vector3.back * 0.001f * gunChangeSpeed;
        }
        else if (lapsedTime > cooldownTime / 2)
        {
            guns[activeGunIndex].transform.localPosition += Vector3.forward * 0.001f * gunChangeSpeed;
        }
    }

    IEnumerator GunChange(GameObject go)
    {
        WaitForSeconds sec = new WaitForSeconds(0.01f);
        while (go.transform.localPosition.y  < originGunPos.y)
        {
            go.transform.localPosition += Vector3.up * 0.001f * gunChangeSpeed;
            yield return sec;
        }
    }


    [ServerRpc]
    private void ChangeGunModeServerRPC(eShootType shootType)
    {
        ChangeGunModeClientRPC(shootType);
    }

    [ClientRpc] // server telling the clients to do something
    private void ChangeGunModeClientRPC(eShootType shootType)
    {
        if (IsOwner) return; // ignore client that shot ball since force was already added
        ChangeGunMode(shootType);
    }
}



