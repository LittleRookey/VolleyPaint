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

    [SerializeField] private TMP_Text _joinCodeText;
    [SerializeField] private TMP_InputField _joinInput;
    [SerializeField] private GameObject _buttons;
    [SerializeField] private Camera mainMenuCamera;

    private UnityTransport _transport;
    [SerializeField] private const int MaxPlayers = 5;

    private async void Awake()
    {
        _transport = FindObjectOfType<UnityTransport>();

        _buttons.SetActive(false);

        // Every unity services need authentication
        await Authenticate();

        _buttons.SetActive(true);
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
        _buttons.SetActive(false);
         // don't need to know ip addresses

        Allocation alloc = await RelayService.Instance.CreateAllocationAsync(MaxPlayers);

        _joinCodeText.text = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);

        // Relay needs to use transport
        // if unity, use unity transport. if Steam, use steam transport.

        // Make sure to set host relay data on host and Client Relay data on client
        _transport.SetHostRelayData(alloc.RelayServer.IpV4, (ushort)alloc.RelayServer.Port, alloc.AllocationIdBytes, alloc.Key, alloc.ConnectionData);

        mainMenuCamera.gameObject.SetActive(false);
        NetworkManager.Singleton.StartHost();
    }

    public async void JoinGame()
    {
        _buttons.SetActive(false);
        
        JoinAllocation alloc = await RelayService.Instance.JoinAllocationAsync(joinCode: _joinInput.text);

        _transport.SetClientRelayData(alloc.RelayServer.IpV4, (ushort)alloc.RelayServer.Port, alloc.AllocationIdBytes, alloc.Key, alloc.ConnectionData, alloc.HostConnectionData);
        mainMenuCamera.gameObject.SetActive(false);
        NetworkManager.Singleton.StartClient();
    }
}
