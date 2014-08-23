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
    /// 
    /// </summary>
    private Rigidbody m_body = null;
    
	// Use this for initialization
	void Start () 
    {
        m_controller = InputManager.ActiveDevice;
        m_body = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
    {	
        // Left Thumb move player
        m_movement.DeltaSpeed = m_controller.LeftStickY;
        m_movement.DeltaRotation = m_controller.LeftStickX;

        // Right Thumb aim turret
	}

    void FixedUpdate()
    {
        var forward = this.transform.forward;
        m_body.AddForce(forward * m_movement.DeltaSpeed * this.Speed);
    }
}
