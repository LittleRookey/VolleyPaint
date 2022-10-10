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
    [SerializeField] private int teamOneScore;
    [SerializeField] private int teamTwoScore;

    [SerializeField] private TMP_Text teamOneScoreText;
    [SerializeField] private TMP_Text teamTwoScoreText;

    [SerializeField] private Transform teamOneBallSpawn;
    [SerializeField] private Transform teamTwoBallSpawn;

    [SerializeField] private Team mostRecentlyShootingTeam;
    [SerializeField] private Team servingTeam;

    [SerializeField] private float roundCooldownDuration;
    [SerializeField] private float currentRoundCooldown;

    private bool roundOver;


    // Start is called before the first frame update
    void Start()
    {
        teamOneScore = 0;
        teamTwoScore = 0;

        currentRoundCooldown = 0.0f;

        roundOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        teamOneScoreText.text = teamOneScore.ToString();
        teamTwoScoreText.text = teamTwoScore.ToString();

        if (roundOver)
        {
            currentRoundCooldown += Time.deltaTime;

            if (currentRoundCooldown >= roundCooldownDuration)
            {
                currentRoundCooldown = 0.0f;
                roundOver = false;
            }
        }
    }

    // Change the tracker for the team that most recently shot
    public void UpdateMostRecentlyShootingTeam(Team team)
    {
        if (team != mostRecentlyShootingTeam)
        {
            mostRecentlyShootingTeam = team;
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                player.GetComponent<PlayerShoot>().ReplenishAmmo(); // Currently a stub, will implement after scorekeeping done
            }
        }
    }

    // Assign a point to winning team and start round cooldown
    public void EndRound(Team winningTeam)
    {
        if (!roundOver)
        {
            if (winningTeam == Team.teamOne)
            {
                teamOneScore += 1;
            }
            else if (winningTeam == Team.teamTwo)
            {
                teamTwoScore += 1;
            }
            roundOver = true;
        }
    }
}
