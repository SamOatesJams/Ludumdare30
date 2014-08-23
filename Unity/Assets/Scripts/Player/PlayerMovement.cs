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

    /// <summary>
    /// Player movement variables
    /// </summary>
    private Movement m_movement = default(Movement);

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

        Transform robotMesh = this.transform.FindChild("SK_RobotDude");

        Transform turret = robotMesh.transform.FindChild("SM_Turret");
        turret.Rotate(Vector3.up, m_deltaTurretRotation);

        Transform weapons = robotMesh.transform.FindChild("SM_Guns");
        weapons.RotateAround(turret.transform.position, Vector3.up, m_deltaTurretRotation);

        Transform weaponsConatainer = this.transform.FindChild("Weapons");

        Transform weaponLeft = weaponsConatainer.FindChild("WeaponLeft");
        weaponLeft.RotateAround(turret.transform.position, Vector3.up, m_deltaTurretRotation);

        Transform weaponRight = weaponsConatainer.FindChild("WeaponRight");
        weaponRight.RotateAround(turret.transform.position, Vector3.up, m_deltaTurretRotation);
    }
}
