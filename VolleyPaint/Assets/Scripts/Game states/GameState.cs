using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using UnityEngine.Events;

public enum Team
{
    teamOne, teamTwo
}

public class GameState : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> teamOneScore;
    [SerializeField] private NetworkVariable<int> teamTwoScore;

    // Which team will be serving on the next round
    [SerializeField] private NetworkVariable<Team> teamServingNextRound;
    // Which team last made contact with the ball
    [SerializeField] private NetworkVariable<Team> teamWithContactWithBall;

    // Scoreboards
    [SerializeField] private TextMeshProUGUI teamOneScoreText;
    [SerializeField] private TextMeshProUGUI teamTwoScoreText;

    private NetworkVariable<bool> teamOnePresent;
    private NetworkVariable<bool> teamTwoPresent;


    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        teamOneScore.Value = 0;
        teamTwoScore.Value = 0;

        teamServingNextRound.Value = Team.teamOne;
        teamWithContactWithBall.Value = Team.teamOne;

        teamOnePresent.Value = false;
        teamTwoPresent.Value = false;
    }

    // Update is called once per frame
    void Update()
    {
        teamOneScoreText.text = teamOneScore.Value.ToString();
        teamTwoScoreText.text = teamTwoScore.Value.ToString();
    }

    public Team GetTeamServingNextRound()
    {
        return teamServingNextRound.Value;
    }

    public Team GetTeamWithBallContact()
    {
        return teamWithContactWithBall.Value;
    }

    public void UpdateScore(Team team)
    {
        if (team == Team.teamOne)
        {
            teamOneScore.Value = teamOneScore.Value + 1;
        }
        else
        {
            teamTwoScore.Value = teamTwoScore.Value + 1;
        }
    }
}
