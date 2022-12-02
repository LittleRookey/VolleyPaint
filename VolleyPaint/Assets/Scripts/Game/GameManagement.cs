using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public enum Team
{
    teamOne, teamTwo, none
}

public class GameManagement : NetworkBehaviour
{
    [SerializeField] private TMP_Text roundOverText;

    private NetworkVariable<int> teamOneScore;
    private NetworkVariable<int> teamTwoScore;

    private Team mostRecentlyShootingTeam;
    private Team servingTeam;

    [SerializeField] private float roundCooldownDuration;
    private float currentRoundCooldown;

    private bool roundOver;

    private Transform player;


    // Start is called before the first frame update
    void Start()
    {
        teamOneScore = new NetworkVariable<int>(0);
        teamTwoScore = new NetworkVariable<int>(0);

        currentRoundCooldown = 0.0f;

        mostRecentlyShootingTeam = Team.none;
        servingTeam = Team.teamOne;

        roundOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (roundOver)
        {
            currentRoundCooldown += Time.deltaTime;

            // round restarts after time limit
            if (currentRoundCooldown >= roundCooldownDuration)
            {
                // allows players to shoot again
                player.GetComponent<PlayerShoot>().enabled = true;
                foreach (AbilityHolder ability in player.GetComponents<AbilityHolder>())
                {
                    ability.enabled = true;
                }


                // hide round won/lost text
                roundOverText.gameObject.SetActive(false);

                currentRoundCooldown = 0.0f;
                roundOver = false;

                GameObject.Find("Spawn Points").GetComponent<BallRespawn>().RespawnBallClientRpc(servingTeam);
            }
        }
    }

    // Change the tracker for the team that most recently shot
    [ServerRpc(RequireOwnership = false)]
    public void UpdateMostRecentlyShootingTeamServerRpc(Team team)
    {
        if (team != mostRecentlyShootingTeam)
        {
            mostRecentlyShootingTeam = team;
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                player.GetComponent<PlayerShoot>().ReplenishAmmoClientRpc(); // Currently a stub, will implement after scorekeeping done
            }
        }
    }

    // Assign a point to winning team and start round cooldown
    [ServerRpc(RequireOwnership=false)]
    public void EndRoundServerRpc(Team winningTeam)
    {
        if (!roundOver)
        {
            if (winningTeam == Team.teamOne)
            {
                teamOneScore.Value += 1;
            }
            else if (winningTeam == Team.teamTwo)
            {
                teamTwoScore.Value += 1;
            }
            servingTeam = winningTeam;
            mostRecentlyShootingTeam = Team.none;
            roundOver = true;

            EndRoundClientRpc(winningTeam);
        }
    }

    // display round over text and prevent shooting on all clients
    [ClientRpc]
    public void EndRoundClientRpc(Team winningTeam)
    {
        // displays round won/lost text
        roundOverText.gameObject.SetActive(true);
        player = Camera.main.transform.parent;
        if (player.GetComponent<TeamAssignment>().assignedTeam == winningTeam) // finds main camera to find team of player its assigned to
        {
            roundOverText.text = "ROUND WON";
        }
        else
        {
            roundOverText.text = "ROUND LOST";
        }

        // prevents players from shooting
        player.GetComponent<PlayerShoot>().enabled = false;
        foreach (AbilityHolder ability in player.GetComponents<AbilityHolder>())
        {
            ability.enabled = false;
        }
    }

    public int GetTeamOneScore()
    {
        return teamOneScore.Value;
    }

    public int GetTeamTwoScore()
    {
        return teamTwoScore.Value;
    }

    public Team GetTeamToAutoAssignTo()
    {
        int totalPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
        //print(totalPlayers);
        if (totalPlayers % 2 == 1)
        {
            return Team.teamOne;
        }
        else
        {
            return Team.teamTwo;
        }
    }
}
