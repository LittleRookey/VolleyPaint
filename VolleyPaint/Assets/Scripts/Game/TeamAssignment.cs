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

        if (assignedTeam == Team.teamOne)
        {
            print("Player joined Team 1");
            transform.position = teamOneSpawn;
        }
        else if (assignedTeam == Team.teamTwo)
        {
            print("Player joined Team 2");
            transform.position = teamTwoSpawn;
            transform.localRotation *= Quaternion.Euler(0, 180, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}