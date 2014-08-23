﻿using UnityEngine;
using System.Collections;
using InControl;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour 
{
    /// <summary>
    /// Speed scale for player
    /// </summary>
    public float Speed = 1.0f;

    /// <summary>
    /// Turret rotation sensitivity
    /// </summary>
    public float RotationSensitivity = 1.0f;

    /// <summary>
    /// The controller for the player
    /// </summary>
    private InputDevice m_controller = null;

    struct Movement 
    {
        public float DeltaSpeed { get; set; }
        public float DeltaRotation { get; set; }
    }

    public struct Weapons
    {
        public Transform Turret { get; set; }
    }

    /// <summary>
    /// Player movement variables
    /// </summary>
    private Movement m_movement = default(Movement);

    /// <summary>
    /// Player weapon variables
    /// </summary>
    private Weapons m_weapons = default(Weapons);

    /// <summary>
    /// The current rotation of the turret and weapon assembly
    /// </summary>
    private float m_deltaTurretRotation = 0f;

    /// <summary>
    /// List of all cos used on the treads
    /// </summary>
    public GameObject[] LeftCogs = null;
    public GameObject[] RightsCogs = null;

    /// <summary>
    /// The treads of the robot
    /// </summary>
    public GameObject LeftTread = null;
    public GameObject RightTread = null;

    /// <summary>
    /// 
    /// </summary>
    private Rigidbody m_body = null;
    
	// Use this for initialization
	void Start () 
    {
        m_body = this.GetComponent<Rigidbody>();

        Weapons weapons = new Weapons();
        weapons.Turret = this.transform.FindChild("SK_RobotDude/SM_Turret");
        this.m_weapons = weapons;
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_controller = InputManager.ActiveDevice;

        // Left Thumb move player
        m_movement.DeltaSpeed = m_controller.LeftStickY;
        m_movement.DeltaRotation = m_controller.LeftStickX;

        // Right Thumb aim turret
        m_deltaTurretRotation = m_controller.RightStickX * RotationSensitivity;
	}

    void FixedUpdate()
    {
        this.transform.Rotate(Vector3.up, m_movement.DeltaRotation);
        m_body.AddForce(this.transform.forward * m_movement.DeltaSpeed * this.Speed);

        RotateTreads();

        m_weapons.Turret.Rotate(Vector3.up, m_deltaTurretRotation);
    }

    private void RotateTreads()
    {
        bool isTurningLeft = m_movement.DeltaRotation < 0.0f;

        foreach (var cog in this.LeftCogs)
        {
            var rotAmount = m_movement.DeltaSpeed + (isTurningLeft ? -m_movement.DeltaRotation : m_movement.DeltaRotation);
            cog.transform.Rotate(Vector3.right, rotAmount * this.Speed);

            var treadOffset = LeftTread.renderer.material.mainTextureOffset;
            treadOffset = treadOffset - new Vector2((isTurningLeft ? -rotAmount : rotAmount) * 0.005f, 0.0f);
            if (treadOffset.x >= 1.0f)
            {
                treadOffset.x = -1.0f;
            }
            else if (treadOffset.x <= -1.0f)
            {
                treadOffset.x = 1.0f;
            }
            LeftTread.renderer.material.mainTextureOffset = treadOffset;
        }

        foreach (var cog in this.RightsCogs)
        {
            var rotAmount = m_movement.DeltaSpeed + (isTurningLeft ? m_movement.DeltaRotation : -m_movement.DeltaRotation);
            cog.transform.Rotate(Vector3.right, rotAmount * this.Speed);

            var treadOffset = RightTread.renderer.material.mainTextureOffset;
            treadOffset = treadOffset - new Vector2((isTurningLeft ? -rotAmount : rotAmount) * 0.005f, 0.0f);
            if (treadOffset.x >= 1.0f)
            {
                treadOffset.x = -1.0f;
            }
            else if (treadOffset.x <= -1.0f)
            {
                treadOffset.x = 1.0f;
            }
            RightTread.renderer.material.mainTextureOffset = treadOffset;
        }
    }

}
