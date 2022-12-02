using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamAssignment : MonoBehaviour
{
    public Team assignedTeam;

    public Vector3 teamOneSpawn;
    public Vector3 teamTwoSpawn;

    // Start is called before the first frame update
    void Start()
    {
        assignedTeam = GameObject.Find("GameManager").GetComponent<GameManagement>().GetTeamToAutoAssignTo();

        RespawnPlayer(assignedTeam);
    }

    private void Update()
    {
        if (transform.position.y < 0 || transform.position.z > 0 && assignedTeam == Team.teamOne || transform.position.z < 0 && assignedTeam == Team.teamTwo) // if player goes below map or goes on the opponents side, respawn the player
        {
            RespawnPlayer(assignedTeam);
        }
    }

    private void RespawnPlayer(Team team)
    {
        if (team == Team.teamOne)
        {
            print("Player joined Team 1");
            transform.position = teamOneSpawn;
        }
        else if (team == Team.teamTwo)
        {
            print("Player joined Team 2");
            transform.position = teamTwoSpawn;
            transform.localRotation *= Quaternion.Euler(0, 180, 0);
        }
    }
}