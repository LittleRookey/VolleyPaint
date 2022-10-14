using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject lobbyCanvas;
    [SerializeField] private GameObject scoreKeepingCanvas;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private Camera mainMenuCamera;
    // Start is called before the first frame update
    private void Start()
    {
        mainMenuCanvas.SetActive(true);
        lobbyCanvas.SetActive(false);
        scoreKeepingCanvas.SetActive(false);
        mainMenuCamera.gameObject.SetActive(true);
    }
    public void StartButton()
    {
        mainMenuCanvas.SetActive(false);
        lobbyCanvas.SetActive(true);
        scoreKeepingCanvas.SetActive(true);
    }

    public void DoRotateGO(GameObject go)
    {
        //go.transform.DOLocalRotate(new Vector3(0,0,90), 1f, RotateMode.Fast).SetEase(Ease.OutBack).
    }
}
