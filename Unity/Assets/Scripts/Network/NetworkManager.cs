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

        Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");

        var roomOptions = new RoomOptions() { isOpen = true, isVisible = true, maxPlayers = 10 };
        PhotonNetwork.JoinOrCreateRoom("RobotsTest", roomOptions, TypedLobby.Default);
    }

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). Use a GUI to show existing rooms available in PhotonNetwork.GetRoomList().");
    }
}
