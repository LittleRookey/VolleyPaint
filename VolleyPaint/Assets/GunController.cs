using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class GunController : MonoBehaviour
{
    public List<GameObject> guns;
    public float gunChangeSpeed = 3f;

    [SerializeField] private int activeGunIndex; // gun slot 
    Vector3 originGunPos = new Vector3(0.665f, -0.26f, 1.435f);
    Vector3 holsterGunPos = new Vector3(0.665f, -2f, 1.435f);

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

    public void ChangeGunMode(eBulletType bulletType)
    {
        guns[activeGunIndex].gameObject.SetActive(false);
        activeGunIndex = (int)bulletType;
        guns[activeGunIndex].transform.localPosition = holsterGunPos;
        guns[activeGunIndex].gameObject.SetActive(true);
        MasterAudio.PlaySound3DAtVector3("Pistol Holster", transform.position);
        StartCoroutine(GunChange(guns[activeGunIndex]));
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
}



