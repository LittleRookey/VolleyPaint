using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLightningWithBall : MonoBehaviour
{
    public float duration;
    private Transform lightningEnd;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, duration);
        lightningEnd = transform.Find("LightningEnd");
    }

    // Update is called once per frame
    void Update()
    {
        lightningEnd.position = new Vector3(lightningEnd.position.x, 0f, lightningEnd.position.z);
    }
}
