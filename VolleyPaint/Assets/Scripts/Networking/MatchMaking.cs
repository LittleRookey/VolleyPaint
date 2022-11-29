using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Authentication;
using TMPro;

#if UNITY_EDITOR
using ParrelSync;
#endif

public class MatchMaking : MonoBehaviour
{
    [SerializeField] private GameObject _buttons;

    [SerializeField] private TMP_Text _joinCodeOutput;
    [SerializeField] private TMP_InputField _joinCodeInput;
    [SerializeField] private GameObject uiManager;
    [SerializeField] private const int maxPlayers = 11;

    private Lobby _connectedLobby;
    private QueryResponse _lobbies;
    private UnityTransport _transport;
    private const string JoinCodeKey = "j";
    private string _playerID;

    private async void Awake()
    {
        _transport = FindObjectOfType<UnityTransport>();

        // Every unity services need authentication
        await Authenticate();

        // enables main menu, so we can keep the game canvas disabled until it connects to Unity
        uiManager.GetComponent<UIManager>().SetGameActive(false);
    }    
    
    private async Task Authenticate()
    {
        var options = new InitializationOptions();
#if UNITY_EDITOR
        // Remove this if you don't have ParrelSynbc installed
        // it's used to differentiate the clients, otherwise, lobby will count them as the same
        options.SetProfile(ClonesManager.IsClone() ? ClonesManager.GetArgument() : "Primary");
#endif

        await UnityServices.InitializeAsync(options);

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        _playerID = AuthenticationService.Instance.PlayerId;
    }

    // quick play - join lobby if available or create one if not
    public async void CreateOrJoinLobby()
    {
        // await Authenticate();

        _connectedLobby = await QuickJoinLobby() ?? await CreateLobby();
    }

    private async Task<Lobby> QuickJoinLobby()
    {
        try 
        {
            // Attempt to join lobby in progress
            var lobby = await Lobbies.Instance.QuickJoinLobbyAsync();

            // If we found one, grab the relay allocation details
            await JoinLobby(lobby.Data[JoinCodeKey].Value);

            return lobby;
        }
        catch (Exception)
        {
            print("No lobbies available via quick join");
            return null;
        }
    }

    public async void JoinGame()
    {
        print(_joinCodeInput.text);
        await JoinLobby(_joinCodeInput.text);
    }

    private async Task JoinLobby(string code)
    {
        JoinAllocation alloc = await RelayService.Instance.JoinAllocationAsync(joinCode: code);

        _transport.SetClientRelayData(alloc.RelayServer.IpV4, (ushort)alloc.RelayServer.Port, alloc.AllocationIdBytes, alloc.Key, alloc.ConnectionData, alloc.HostConnectionData);
        NetworkManager.Singleton.StartClient();
        
        uiManager.GetComponent<UIManager>().SetGameActive(true);
    }

    public async void CreateGame()
    {
        await CreateLobby();
    }

    // Lobby will automatically shut down when there is inactivity for more than 30 secs
    public async Task<Lobby> CreateLobby()
    {
        try 
        {
            uiManager.GetComponent<UIManager>().SetGameActive(true);

            // create a relay allocation and generate a join code to share with the lobby
            var a = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            _joinCodeOutput.text = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

            // Create a lobby, adding the relay join code to the lobby data
            var options = new CreateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> { { JoinCodeKey, new DataObject(DataObject.VisibilityOptions.Public, _joinCodeOutput.text) } }
            };
            var lobby = await Lobbies.Instance.CreateLobbyAsync("Lobby Name 1", maxPlayers, options);

            // Send a heartbeat every 15 seconds to keep room alive
            // StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));

            // Set the game room to use the relay allocation
            _transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

            // Start the room. I'm doing this immediately, but maybe you wait for the lobby fto fill up
            NetworkManager.Singleton.StartHost();
            return lobby;
        } 
        catch(Exception e)
        {
            Debug.LogFormat("Failed creating a lobby: {0}", e);
            return null;
        }
    }

    private void OnDestroy()
    {
        try
        {
            StopAllCoroutines();
            // TODO: add a check to see if you're a host
            if (_connectedLobby != null)
            {
                if (_connectedLobby.HostId == _playerID) Lobbies.Instance.DeleteLobbyAsync(_connectedLobby.Id);
                else Lobbies.Instance.RemovePlayerAsync(_connectedLobby.Id, _playerID);
            }
        } catch (Exception e)
        {
            Debug.Log($"Error shutting down lobby: {e}");
        }
    }

    private static IEnumerator HeartbeatLobbyCoroutine(string lobbyID, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyID);
            yield return delay;
        }
    }
}
