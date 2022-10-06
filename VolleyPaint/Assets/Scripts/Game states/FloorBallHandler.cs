using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBallHandler : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private Team owningTeam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            gameState.UpdateScore(Team.teamTwo);
        }
    }
}
