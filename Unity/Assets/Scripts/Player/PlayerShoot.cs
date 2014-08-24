using UnityEngine;
using System;
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
    private Transform m_hit;

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
                    Transform explosive = hitInfo.transform.FindChild("Small explosion");
                    Debug.Log("Hit");
                    Hit(explosive);

                    if (network != null)
                    {
                        network.HasHit = true;
                        var photonView = explosive.GetComponent<PhotonView>();

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

    public void Hit(Transform transform)
    {
        transform.GetComponent<ParticleEmitter>().emit = true;
        m_hit = transform;
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
        if (m_hit != null && !emitting)
        {
            m_hit.GetComponent<ParticleEmitter>().emit = false;
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
