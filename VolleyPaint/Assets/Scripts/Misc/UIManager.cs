using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text teamOneText;
    [SerializeField] private TMP_Text teamTwoText;

    [SerializeField] private Camera mainMenuCamera;
    [SerializeField] private GameObject flyCamera;

    [SerializeField] private GameObject scoreKeepingCanvas;
    [SerializeField] private GameObject mainMenuCanvas;

    private bool spectate;

    private bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            teamOneText.text = GameObject.Find("GameManager").GetComponent<GameManagement>().GetTeamOneScore().ToString();
            teamTwoText.text = GameObject.Find("GameManager").GetComponent<GameManagement>().GetTeamTwoScore().ToString();
        }

        // disable player in spectate mode
        if (spectate && Camera.main != null)
        {
            Camera.main.transform.parent.gameObject.SetActive(!gameStarted);
        }
    }

    public void SetGameActive(bool active)
    {
        gameStarted = active;

        // disable main menu if active
        mainMenuCanvas.SetActive(!active);
        mainMenuCamera.gameObject.SetActive(!active);

        // enable score keeping UI if active
        scoreKeepingCanvas.SetActive(active);

        if (spectate)
        {
            flyCamera.SetActive(true);
        }

       
    }
    public void SetSpectateActive(bool active)
    {
        spectate = active;
    }
}
