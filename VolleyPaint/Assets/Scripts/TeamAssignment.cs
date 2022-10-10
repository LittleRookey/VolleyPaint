using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TeamAssignment : NetworkBehaviour
{
    public Team assignedTeam;

    // Start is called before the first frame update
    void Start()
    {
        assignedTeam = GameObject.Find("GameManager").GetComponent<GameManagement>().GetTeamToAutoAssignTo();

        if (assignedTeam == Team.teamOne)
        {
            transform.position = GameObject.Find("Team 1 Ball Spawn").transform.position;
        }
        else if (assignedTeam == Team.teamTwo)
        {
            transform.position = GameObject.Find("Team 2 Ball Spawn").transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
