using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text teamOneText;
    [SerializeField] private TMP_Text teamTwoText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        teamOneText.text = GameObject.Find("GameManager").GetComponent<GameManagement>().GetTeamOneScore().ToString();
        teamTwoText.text = GameObject.Find("GameManager").GetComponent<GameManagement>().GetTeamTwoScore().ToString();
    }
}
