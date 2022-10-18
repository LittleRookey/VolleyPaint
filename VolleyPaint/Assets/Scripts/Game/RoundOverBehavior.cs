using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundOverBehavior : MonoBehaviour
{
    [SerializeField] private Team owningTeam;
    [SerializeField] private GameManagement manager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            if (owningTeam == Team.teamOne)
            {
                manager.EndRoundServerRpc(Team.teamTwo);
            }
            else if (owningTeam == Team.teamTwo)
            {
                manager.EndRoundServerRpc(Team.teamOne);
            }
        }
    }
}
