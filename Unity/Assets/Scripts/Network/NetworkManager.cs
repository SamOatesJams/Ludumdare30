using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    private bool m_isConnectedToLobby = false;

    public GameObject LocalPlayer = null;

	// Use this for initialization
	void Start () 
    {
        PhotonNetwork.autoJoinLobby = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_isConnectedToLobby && !PhotonNetwork.connected)
        {
            Debug.Log("Connecting to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
            m_isConnectedToLobby = true;
            PhotonNetwork.ConnectUsingSettings("2." + Application.loadedLevel);
        }
	}

    /// <summary>
    /// Called when connected to the master serber
    /// </summary>
    public virtual void OnConnectedToMaster()
    {
        if (PhotonNetwork.networkingPeer.AvailableRegions != null)
        {
            Debug.LogWarning("List of available regions counts " + PhotonNetwork.networkingPeer.AvailableRegions.Count + ". First: " + PhotonNetwork.networkingPeer.AvailableRegions[0] + " \t Current Region: " + PhotonNetwork.networkingPeer.CloudRegion);
        }

        var roomOptions = new RoomOptions() { isOpen = true, isVisible = true, maxPlayers = 10 };
        PhotonNetwork.JoinOrCreateRoom("RobotsTestv2", roomOptions, TypedLobby.Default);
    }

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    public void OnJoinedRoom()
    {
        var spawn = SpawnManager.Instance.GetSpawm((Team)Random.Range(0, 1));                
        var player = PhotonNetwork.Instantiate("Player", spawn.transform.position, spawn.transform.rotation, 0);
        player.name = "LocalPlayer";
        PhotonNetwork.playerName = "Player-" + (PhotonNetwork.playerList.Length + 1);

        var camera = player.transform.Find("SK_RobotDude/SM_Turret/Camera").gameObject;
        camera.SetActive(true);

        var collider = player.GetComponent<Collider>();
        collider.enabled = true;

        var body = player.GetComponent<Rigidbody>();
        body.isKinematic = false;

        var movement = player.GetComponent<PlayerMovement>();
        movement.enabled = true;

        var shoot = player.GetComponent<PlayerShoot>();
        shoot.enabled = true;
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby().");
    }
}
