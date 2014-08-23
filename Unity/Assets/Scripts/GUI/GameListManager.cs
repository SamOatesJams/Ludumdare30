using UnityEngine;
using System.Collections;

public class GameListManager : MonoBehaviour 
{
    private bool m_getGameList = true;

    public GameObject DefaultEntry = null;

    public static GameEntryButton ActiveRoom { get; set; }

	// Use this for initialization
	void Start () 
    {
        GameListManager.ActiveRoom = null;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
	    if (m_getGameList && PhotonNetwork.connected && PhotonNetwork.insideLobby)
        {
            var rooms = PhotonNetwork.GetRoomList();

            Debug.Log("Room Count: " + rooms.Length);

            // clear existing room entries
            foreach (Transform child in this.transform)
            {
                if (child.gameObject.GetActive())
                {
                    GameObject.Destroy(child);
                }
            }

            float yOffset = 25.0f;

            // add room entries
            foreach (var room in rooms)
            {
                var roomEntry = (GameObject)GameObject.Instantiate(this.DefaultEntry);
                roomEntry.transform.parent = this.transform;
                roomEntry.transform.localScale = Vector3.one;
                roomEntry.transform.localPosition = new Vector3(3.0f, yOffset -= 25.0f, 0.0f);
                roomEntry.SetActive(true);

                var label = roomEntry.GetComponentInChildren<UILabel>();
                label.text = room.name + " - " + room.playerCount + "/" + room.maxPlayers;

                var button = roomEntry.GetComponentInChildren<GameEntryButton>();
                button.RoomName = room.name;                
            }

            m_getGameList = false;
        }
	}
}
