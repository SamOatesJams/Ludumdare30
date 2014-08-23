using UnityEngine;
using System;
using System.Collections;
using InControl;

public class PlayerShoot : MonoBehaviour {

    public float ShootDelay = 100;

    /// <summary>
    /// Whether the player is currently shooting or not
    /// </summary>
    public bool Shooting { get; set; }

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

	// Use this for initialization
	void Start () 
    {
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
        m_controller = InputManager.ActiveDevice;

        if (m_controller.RightTrigger.Value > 0.5f)
        {
            if (Time.time > m_lastshot + ShootDelay)
            {
                SetEmmiting(true);

                var network = collider.gameObject.GetComponent<PlayerNetwork>();
                if (network != null)
                {
                    network.HasShot = true;
                    network.SideShot = m_side;
                }

                m_lastshot = Time.time;
                Shooting = true;
            }
        }
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
        Transform turret = this.transform.FindChild("SK_RobotDude/SM_Turret/SM_Guns");
        Transform weaponSide = turret.FindChild("Weapon" + (m_side == 0 ? "Left" : "Right"));

        foreach (Transform child in weaponSide)
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
