using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

    public float Health = 100.0f;

    public UISlider HealthSlider = null;

    public bool IsDead { get; private set; }

    private float m_dieTime = 0.0f;

    public AudioSource DieAudio = null;

    private bool m_showWinner = false;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (GameOptions.Instance.GetWinner() != null)
        {
            if (!m_showWinner)
            {
                m_showWinner = true;
                m_dieTime = Time.time;
            }
            else
            {
                var photon = GameOptions.Instance.GetWinner().GetComponent<PhotonView>();
                var healthLabel = this.HealthSlider.gameObject.GetComponentInChildren<UILabel>();
                if (healthLabel != null)
                {                    
                    int endTime = 5 - (int)(Time.time - m_dieTime);
                    healthLabel.text = photon.owner.name + " Wins!";

                    if (endTime <= 3)
                    {
                        healthLabel.text = "Game ends in " + endTime + "...";
                    }
                }

                if (Time.time - m_dieTime > 5.0f)
                {
                    GameOptions.Instance.SetWinner(null);
                    photon.owner.SetScore(0);

                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LeaveLobby();
                    Application.LoadLevel("MainMenu");
                }
            }
            return;
        }

        this.HealthSlider.sliderValue = this.Health / 100.0f;

        if (this.IsDead)
        {
            int respawnTime = 5 - (int)(Time.time - m_dieTime);
            var healthLabel = this.HealthSlider.gameObject.GetComponentInChildren<UILabel>();
            if (healthLabel != null)
            {
                healthLabel.text = "Respawning in " + respawnTime + "...";
            }

            if (Time.time - m_dieTime >= 5.0f)
            {
                this.Health = 100.0f;

                var playerNetwork = this.GetComponent<PlayerNetwork>();
                playerNetwork.HasTeleported = true;

                var spawn = SpawnManager.Instance.GetSpawm(Team.Good); //TODO:SO
                this.transform.position = spawn.transform.position;
                this.transform.rotation = spawn.transform.rotation;

                if (healthLabel != null)
                {
                    healthLabel.text = "Health";
                }                

                this.IsDead = false;

                var deadFire = this.transform.FindChild("DeadFire");
                deadFire.gameObject.SetActive(false);
            }
        }
	}

    public void Died()
    {
        if (!this.IsDead)
        {
            this.Health = 0.0f;
            this.IsDead = true;
            m_dieTime = Time.time;

            this.DieAudio.Play();

            var deadFire = this.transform.FindChild("DeadFire");
            deadFire.gameObject.SetActive(true);
        }
    }
}
