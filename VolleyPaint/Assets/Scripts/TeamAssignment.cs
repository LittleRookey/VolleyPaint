using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TeamAssignment : NetworkBehaviour
{
    public Team assignedTeam;

    public Vector3 teamOneSpawn;
    public Vector3 teamTwoSpawn;

    // Start is called before the first frame update
    void Start()
    {
        assignedTeam = GameObject.Find("GameManager").GetComponent<GameManagement>().GetTeamToAutoAssignTo();

        if (assignedTeam == Team.teamOne)
        {
            transform.position = teamOneSpawn;
        }
        else if (assignedTeam == Team.teamTwo)
        {
            transform.position = teamTwoSpawn;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}