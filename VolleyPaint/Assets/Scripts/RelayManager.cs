using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System;

public class RelayManager : MonoBehaviour
{
    /// <summary>
    /// RelayHostData represents the necessary information
    /// for a Host to host a game on a Relay
    /// </summary>
    public struct RelayHostData
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDBytes;
        public byte[] ConnectionData;
        public byte[] Key;
    }

    [SerializeField] private TMP_Text _joinCodeOutput;
    [SerializeField] private TMP_InputField _joinCodeInput;
    [SerializeField] private Camera mainMenuCamera;
    [SerializeField] private GameObject scoreKeepingCanvas;
    [SerializeField] private GameObject mainMenuCanvas;

    private UnityTransport _transport;
    [SerializeField] private const int MaxPlayers = 5;

    private async void Awake()
    {
        _transport = FindObjectOfType<UnityTransport>();

        mainMenuCanvas.SetActive(false);

        // Every unity services need authentication
        await Authenticate();

        mainMenuCanvas.SetActive(true);
    }

    private static async Task Authenticate()
    {
        // Initialize unity services engine
        await UnityServices.InitializeAsync();

        // don't have to force player to login, 
        // let player start play right away
        
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateGame()
    {
        OnGameStart();

        // don't need to know ip addresses

        Allocation alloc = await RelayService.Instance.CreateAllocationAsync(MaxPlayers);

        _joinCodeOutput.text = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);

        // Relay needs to use transport
        // if unity, use unity transport. if Steam, use steam transport.

        // Make sure to set host relay data on host and Client Relay data on client
        _transport.SetHostRelayData(alloc.RelayServer.IpV4, (ushort)alloc.RelayServer.Port, alloc.AllocationIdBytes, alloc.Key, alloc.ConnectionData);
        NetworkManager.Singleton.StartHost();
    }

    public async void JoinGame()
    {
        OnGameStart();

        JoinAllocation alloc = await RelayService.Instance.JoinAllocationAsync(joinCode: _joinCodeInput.text);

        _transport.SetClientRelayData(alloc.RelayServer.IpV4, (ushort)alloc.RelayServer.Port, alloc.AllocationIdBytes, alloc.Key, alloc.ConnectionData, alloc.HostConnectionData);
        NetworkManager.Singleton.StartClient();
    }

    private void OnGameStart()
    {
        // disable main menu
        mainMenuCanvas.SetActive(false);
        mainMenuCamera.gameObject.SetActive(false);

        // enable score keeping UI
        scoreKeepingCanvas.SetActive(true);
    }
}
