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
    [SerializeField] private GameObject heroSelectCamera;

    [SerializeField] private GameObject scoreKeepingCanvas;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject heroSelectCanvas;

    [SerializeField] private GameObject confirmButton;

    private bool spectate;
    private bool gameStarted;
    private bool heroSelected = false;

    public GameObject userPlayer; // player that is being controlled by machine running this script

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
        print("It is here UIManger0");
        // disable player in spectate mode
        // called in update so player is disabled as soon as network is connected (when there a main camera spawns
        if (!heroSelected && Camera.main && !userPlayer)
        {
            userPlayer = Camera.main.transform.parent.gameObject;
            userPlayer.SetActive(!gameStarted);
            if (!spectate)
            {
                heroSelectCanvas.SetActive(true);
                heroSelectCamera.SetActive(true);

                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
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
            flyCamera.SetActive(active);
        }
    }
    public void SetSpectateActive(bool active)
    {
        spectate = active;
    }

    public void ReactivatePlayer()
    {
        heroSelectCanvas.SetActive(false);
        heroSelectCamera.SetActive(false);

        userPlayer.SetActive(true);
        heroSelected = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetConfirmButtonActive()
    {
        confirmButton.SetActive(true);
    }
}
