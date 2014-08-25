using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class PlayerGameInfo : MonoBehaviour
{

    public GameObject GameInfoPanel = null;

    public GameObject PlayerPrefab = null;
    public GameObject PlayerTable = null;

    private bool m_active = false;
    private float m_lastToggle = 0.0f;

    private List<GameObject> m_items = new List<GameObject>();

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        var players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        if (GameOptions.Instance.GetWinner() != null)
        {
            return;
        }

        foreach (var p in players)
        {
            var photon = p.GetComponent<PhotonView>();
            if (photon.owner.GetScore() >= 50)
            {
                GameOptions.Instance.SetWinner(p);
                if (!m_active)
                {
                    m_active = true;
                    ShowMenu(players);
                }

                return;
            }
        }
        
        var controller = InputManager.ActiveDevice;

        if (controller.RightStickButton.IsPressed && Time.time - m_lastToggle > 0.25f)
        {
            m_active = !m_active;
            m_lastToggle = Time.time;

            if (m_active)
            {
                ShowMenu(players);
            }
            else
            {
                foreach (var p in m_items)
                {
                    GameObject.Destroy(p);
                }
            }

            this.GameInfoPanel.SetActive(m_active);
        }
	}

    private void ShowMenu(List<GameObject> players)
    {
        float yOffset = 25.0f;

        var orderedPlayers = new List<GameObject>();
        while (players.Count > 0)
        {
            GameObject bestPlayer = null;
            foreach (var p in players)
            {
                if (bestPlayer == null)
                {
                    bestPlayer = p;
                    continue;
                }

                var photon = p.GetComponent<PhotonView>();
                var bestphoton = bestPlayer.GetComponent<PhotonView>();

                if (photon.owner.GetScore() > bestphoton.owner.GetScore())
                {
                    bestPlayer = p;
                }
            }

            orderedPlayers.Add(bestPlayer);
            players.Remove(bestPlayer);
        }

        foreach (var p in orderedPlayers)
        {
            var photon = p.GetComponent<PhotonView>();

            var player = (GameObject)GameObject.Instantiate(this.PlayerPrefab);
            player.transform.parent = this.PlayerTable.transform;
            player.SetActive(true);

            player.transform.localScale = Vector3.one;
            player.transform.localPosition = new Vector3(3.0f, yOffset -= 25.0f, 0.0f);
            player.transform.localEulerAngles = Vector3.zero;

            var labelObject = player.transform.FindChild("Label - Title");
            labelObject.gameObject.SetActive(true);

            var label = labelObject.GetComponent<UILabel>();
            label.text = photon.owner.name + "  -  Score: " + photon.owner.GetScore();

            m_items.Add(player);
        }                
    }
}
