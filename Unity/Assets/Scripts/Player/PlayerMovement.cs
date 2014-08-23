using UnityEngine;
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

    struct Weapons
    {
        public Transform Turret { get; set; }
        public Transform WeaponSystem { get; set; }
        public Transform WeaponLeft { get; set; }
        public Transform WeaponRight { get; set; }
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
    /// 
    /// </summary>
    private Rigidbody m_body = null;
    
	// Use this for initialization
	void Start () 
    {
        m_body = this.GetComponent<Rigidbody>();

        Transform robotMesh = this.transform.FindChild("SK_RobotDude");
        Transform weaponsConatainer = this.transform.FindChild("Weapons");

        m_weapons.Turret = robotMesh.transform.FindChild("SM_Turret");
        m_weapons.WeaponSystem = robotMesh.transform.FindChild("SM_Guns");
        m_weapons.WeaponLeft = weaponsConatainer.FindChild("WeaponLeft");
        m_weapons.WeaponRight = weaponsConatainer.FindChild("WeaponRight");
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
        
        var forward = this.transform.forward;
        m_body.AddForce(forward * m_movement.DeltaSpeed * this.Speed);

        m_weapons.Turret.Rotate(Vector3.up, m_deltaTurretRotation);
        m_weapons.WeaponSystem.RotateAround(m_weapons.Turret.transform.position, Vector3.up, m_deltaTurretRotation);

        m_weapons.WeaponLeft.RotateAround(m_weapons.Turret.transform.position, Vector3.up, m_deltaTurretRotation);
        m_weapons.WeaponRight.RotateAround(m_weapons.Turret.transform.position, Vector3.up, m_deltaTurretRotation);
    }
}
