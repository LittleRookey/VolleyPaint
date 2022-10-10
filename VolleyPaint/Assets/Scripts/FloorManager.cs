using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
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
                manager.EndRound(Team.teamTwo);
            }
            else if (owningTeam == Team.teamTwo)
            {
                manager.EndRound(Team.teamOne);
            }
        }
    }
}
