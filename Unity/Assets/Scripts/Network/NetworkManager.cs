using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    private bool m_isConnectedToLobby = false;

    public GameObject LocalPlayer = null;

    public bool AutoCreateAndJoin = true;

    public static string ActiveRoom = null;

    void Awake()
    {
        if (GameOptions.Instance == null)
        {
            var optionsObject = new GameObject("GameOptions");
            optionsObject.AddComponent<GameOptions>();
            GameObject.DontDestroyOnLoad(optionsObject);
        }
    }
    
	// Use this for initialization
	void Start () 
    {
        PhotonNetwork.autoJoinLobby = !this.AutoCreateAndJoin;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_isConnectedToLobby && !PhotonNetwork.connected)
        {
            m_isConnectedToLobby = true;
            PhotonNetwork.ConnectUsingSettings("2." + Application.loadedLevel);
        }

        if (NetworkManager.ActiveRoom != null && PhotonNetwork.connected && PhotonNetwork.insideLobby && !PhotonNetwork.inRoom)
        {
            var name = NetworkManager.ActiveRoom;
            NetworkManager.ActiveRoom = null;

            var roomOptions = new RoomOptions() { isOpen = true, isVisible = true, maxPlayers = 10 };
            PhotonNetwork.JoinOrCreateRoom(name, roomOptions, TypedLobby.Default);
        }
	}

    public static void JoinGame(string gameName)
    {
        Application.LoadLevel("Default");
        NetworkManager.ActiveRoom = gameName;
    }

    /// <summary>
    /// Called when connected to the master server
    /// </summary>
    public virtual void OnConnectedToMaster()
    {
        if (PhotonNetwork.networkingPeer.AvailableRegions != null)
        {
            Debug.LogWarning("List of available regions counts " + PhotonNetwork.networkingPeer.AvailableRegions.Count + ". First: " + PhotonNetwork.networkingPeer.AvailableRegions[0] + " \t Current Region: " + PhotonNetwork.networkingPeer.CloudRegion);
        }

        if (this.AutoCreateAndJoin)
        {
            var roomOptions = new RoomOptions() { isOpen = true, isVisible = false, maxPlayers = 10 };
            PhotonNetwork.JoinOrCreateRoom("RobotsTestv5", roomOptions, TypedLobby.Default);
        }
    }

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    public void OnJoinedRoom()
    {
        if (SpawnManager.Instance.HasNoSpawns())
        {
            return;
        }

        //var spawn = SpawnManager.Instance.GetSpawm((Team)Random.Range(0, 2));         TODO:SO
        var spawn = SpawnManager.Instance.GetSpawm(Team.Good); 
        var player = PhotonNetwork.Instantiate("Player", spawn.transform.position, spawn.transform.rotation, 0);
        player.name = "LocalPlayer";

        PhotonNetwork.playerName = PlayerNameInput.GetPlayerName();

        var camera = player.transform.Find("SK_RobotDude/SM_Turret/UI Root (2D)").gameObject;
        camera.SetActive(true);

        var body = player.GetComponent<Rigidbody>();
        body.isKinematic = false;

        var movement = player.GetComponent<PlayerMovement>();
        movement.enabled = true;

        var shoot = player.GetComponent<PlayerShoot>();
        shoot.LocalPlayer = true;

        var nameTag = player.transform.FindChild("UI Root (3D)");
        nameTag.gameObject.SetActive(false);
    }

    public virtual void OnJoinedLobby()
    {
    }
}
