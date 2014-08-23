using UnityEngine;
using System;
using System.Collections;
using InControl;

public class PlayerShoot : MonoBehaviour {

    public float ShootDelay = 100;

    /// <summary>
    /// The controller for the player
    /// </summary>
    private InputDevice m_controller = null;

    /// <summary>
    /// Whether the player is currently shooting or not
    /// </summary>
    private bool m_shooting = false;

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
        m_controller = InputManager.ActiveDevice;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_shooting && Time.time > m_lastshot + 0.1)
        {
            SetEmmiting(false);
            m_shooting = false;
            m_side = (m_side == 0 ? 1 : 0);
        }
	}

    void FixedUpdate()
    {
        if (m_controller.RightTrigger.Value > 0.5f)
        {
            if (Time.time > m_lastshot + ShootDelay)
            {
                Transform weapons = this.transform.FindChild("Weapons");
                SetEmmiting(true);

                m_lastshot = Time.time;
                m_shooting = true;
            }
        }
    }

    void SetEmmiting(bool emitting)
    {
        Transform weapons = this.transform.FindChild("Weapons");
        Transform weaponSide = weapons.FindChild("Weapon" + (m_side == 0 ? "Left" : "Right"));

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
