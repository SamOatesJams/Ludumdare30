using UnityEngine;
using System.Collections;
using InControl;

public class PlayerShoot : MonoBehaviour {

    public float ShootDelay = 100;

    /// <summary>
    /// Whether this player is the local player
    /// </summary>
    public bool LocalPlayer = false;

    /// <summary>
    /// Whether the player is currently shooting or not
    /// </summary>
    public bool Shooting { get; private set; }

    /// <summary>
    /// The controller for the player
    /// </summary>
    private InputDevice m_controller = null;

    /// <summary>
    /// The time the player last shot their weapon
    /// </summary>
    private float m_lastshot = 0L;

    /// <summary>
    /// Side to shoot from. 0 is left, 1 is right
    /// </summary>
    private int m_side = 0;

    /// <summary>
    /// Weapons to shoot from
    /// </summary>
    private Transform[] m_weapons;

    /// <summary>
    /// Target we have hit
    /// </summary>
    private ParticleEmitter m_damageParticles;

	// Use this for initialization
	void Start ()
    {
        Transform turret = this.transform.FindChild("SK_RobotDude/SM_Turret/SM_Guns");
        m_weapons = new Transform[2];
        m_weapons[0] = turret.FindChild("WeaponLeft");
        m_weapons[1] = turret.FindChild("WeaponRight");
	}
	
	// Update is called once per frame
	void Update () 
    {
        CheckAnimation();
    }

    public void CheckAnimation()
    {
        if (Shooting && Time.time > m_lastshot + 0.1)
        {
            SetEmmiting(false);
            Shooting = false;
            m_side = (m_side == 0 ? 1 : 0);
        }
	}

    void FixedUpdate()
    {
        if (!this.LocalPlayer)
        {
            return;
        }

        m_controller = InputManager.ActiveDevice;

        if (m_controller.RightTrigger.Value > 0.5f)
        {
            if (Time.time > m_lastshot + ShootDelay)
            {

                var network = collider.gameObject.GetComponent<PlayerNetwork>();
                if (network != null)
                {
                    network.HasShot = true;
                    network.SideShot = m_side;
                }

                Shoot(m_side);

                RaycastHit hitInfo;
                Ray ray = new Ray(m_weapons[m_side].position, m_weapons[m_side].forward);
                bool hit = Physics.Raycast(ray, out hitInfo);

                if (hit && hitInfo.transform.tag == "Player")
                {
                    Hit(hitInfo.transform);

                    if (network != null)
                    {
                        network.HasHit = true;
                        var photonView = hitInfo.transform.GetComponent<PhotonView>();

                        if (photonView != null)
                        {
                            PhotonPlayer player = photonView.owner;
                            network.HitPlayer = player.ID;
                        }
                    }
                }
            }
        }
    }

    public void Hit(Transform player)
    {
        Transform explosive = player.FindChild("Small explosion");
        m_damageParticles = explosive.GetComponent<ParticleEmitter>();
        m_damageParticles.emit = true;

        var playerData = player.GetComponent<PlayerData>();
        var newHealth = playerData.Health - (10.0f + Random.Range(0.0f, 10.0f));
        if (newHealth <= 0.0f)
        {
            Kill(player);
        }
        else
        {
            playerData.Health = newHealth;
        }

        Debug.Log(player.GetComponent<PhotonView>().owner.name + " has health: " + playerData.Health);
    }

    private void Kill(Transform player)
    {
        var playerData = player.GetComponent<PlayerData>();
        playerData.Health = 100.0f;

        var spawn = SpawnManager.Instance.GetSpawm(Team.Good); //TODO:SO
        player.position = spawn.transform.position;
        player.rotation = spawn.transform.rotation;

    }

    public void Shoot(int side)
    {
        SetEmmiting(false);
        m_side = side;
        SetEmmiting(true);

        Shooting = true;
        m_lastshot = Time.time;

    }

    public void SetEmmiting(bool emitting)
    {
        if (m_damageParticles != null && !emitting)
        {
            m_damageParticles.emit = false;
        }

        foreach (Transform child in m_weapons[m_side])
        {
            ParticleEmitter script = child.GetComponent<ParticleEmitter>();
            if (script != null)
            {
                script.emit = emitting;
            }

            foreach (Transform superchild in child.transform)
            {
                ParticleEmitter emitter = superchild.GetComponent<ParticleEmitter>();
                if (emitter != null)
                {
                    emitter.emit = emitting;
                }
            }
        }

    }
}
