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

    struct TurretRotation
    {
        public float DeltaX { get; set; }
        public float DeltaY { get; set; }
    }

    public struct Weapons
    {
        public Transform Turret { get; set; }
        public Transform WeaponSystem { get; set; }
    }

    /// <summary>
    /// Player movement variables
    /// </summary>
    private Movement m_movement = default(Movement);

    /// <summary>
    /// Player turret rotation variables
    /// </summary>
    private TurretRotation m_turretRotation = default(TurretRotation);

    /// <summary>
    /// Player weapon variables
    /// </summary>
    private Weapons m_weapons = default(Weapons);

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

    public AudioSource EngineAudio = null;

	// Use this for initialization
	void Start () 
    {
        m_body = this.GetComponent<Rigidbody>();

        Weapons weapons = new Weapons();
        weapons.Turret = this.transform.FindChild("SK_RobotDude/SM_Turret");
        weapons.WeaponSystem = this.transform.FindChild("SK_RobotDude/SM_Turret/SM_TurretArm");
        this.m_weapons = weapons;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (GameOptions.Instance.GetWinner() != null)
        {
            return;
        }

        var playerData = this.GetComponent<PlayerData>();
        if (playerData.IsDead)
        {
            m_movement.DeltaSpeed = 0.0f;
            m_movement.DeltaRotation = 0.0f;
            m_turretRotation.DeltaX = 0.0f;
            m_turretRotation.DeltaY = 0.0f;
            return;
        }

        m_controller = InputManager.ActiveDevice;

        if (m_controller.Name == "Keyboard/Mouse")
        {
            Screen.lockCursor = true;
        }

        // Left Thumb move player
        m_movement.DeltaSpeed = m_controller.LeftStickY;
        m_movement.DeltaRotation = m_movement.DeltaSpeed < 0.0f ? -m_controller.LeftStickX : m_controller.LeftStickX;

        // Right Thumb aim turret
        m_turretRotation.DeltaX = m_controller.RightStickX * RotationSensitivity;
        m_turretRotation.DeltaY = m_controller.RightStickY * RotationSensitivity;
	}

    void FixedUpdate()
    {
        this.transform.Rotate(Vector3.up, m_movement.DeltaRotation);
        m_body.AddForce(this.transform.forward * m_movement.DeltaSpeed * this.Speed);

        RotateTreads();

        m_weapons.Turret.Rotate(Vector3.up, m_turretRotation.DeltaX);
        m_weapons.WeaponSystem.Rotate(Vector3.left, m_turretRotation.DeltaY);

        Quaternion localRotation = m_weapons.WeaponSystem.localRotation;
        if (localRotation.eulerAngles.x >= 19f && localRotation.eulerAngles.x <= 180f)
        {
            m_weapons.WeaponSystem.localRotation = Quaternion.Euler(new Vector3(19, 0, 0));
        }
        if (localRotation.eulerAngles.x <= 300f && localRotation.eulerAngles.x >= 180f)
        {
            m_weapons.WeaponSystem.localRotation = Quaternion.Euler(new Vector3(300f, 0, 0));
        }

        this.EngineAudio.pitch = 0.5f + Mathf.Min(m_body.velocity.magnitude * 0.01f, 2.5f);
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
            treadOffset = treadOffset - new Vector2((isTurningLeft ? rotAmount : -rotAmount) * 0.005f, 0.0f);
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
