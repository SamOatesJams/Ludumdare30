using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerNetwork : MonoBehaviour 
{
    private Transform m_turret;

    private Vector3 m_targetPosition = default(Vector3);
    private Quaternion m_targetRotation = default(Quaternion);
    private Quaternion m_targetTurretRotation = default(Quaternion);
    private Vector3 m_lastPosition = default(Vector3);
    private Quaternion m_lastRotation = default(Quaternion);
    private Quaternion m_lastTurretRotation = default(Quaternion);

    private bool m_hasPosition = false;

    private PhotonView m_photon = null;
    private float m_lerpTime = 0.0f;
    private bool m_hasData = false;

    public bool HasTeleported { get; set; }
    public bool HasShot { get; set; }
    public int SideShot { get; set; }
    public bool HasHit { get; set; }
    public string HitPlayer { get; set; }

    void Awake()
    {
        m_photon = this.GetComponent<PhotonView>();
        m_turret = this.transform.FindChild("SK_RobotDude/SM_Turret");
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)   
        {
            //We own this player: send the others our data       
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);
            stream.SendNext(m_turret.transform.rotation);
            stream.SendNext(this.HasTeleported);
            stream.SendNext(this.HasShot);
            stream.SendNext(this.SideShot == 1);
            stream.SendNext(this.HasHit);
            stream.SendNext(this.HitPlayer);

            // Reset flags
            this.HasTeleported = false;
            this.HasShot = false;
        }
        else
        {
            //Network player, receive data
            m_targetPosition = (Vector3)stream.ReceiveNext();
            m_targetRotation = (Quaternion)stream.ReceiveNext();
            m_targetTurretRotation = (Quaternion)stream.ReceiveNext();
            var didPortal = (bool)stream.ReceiveNext();
            HasShot = (bool)stream.ReceiveNext();
            SideShot = (bool)stream.ReceiveNext() ? 1 : 0;
            HasHit = (bool)stream.ReceiveNext();
            HitPlayer = (string)stream.ReceiveNext();

            if (!m_hasPosition || didPortal)
            {
                m_lastPosition = m_targetPosition;
                m_lastRotation = m_targetRotation;
                m_lastTurretRotation = m_targetRotation;
                m_hasPosition = true;
            }
            else
            {
                m_lastPosition = this.transform.position;
                m_lastRotation = this.transform.rotation;
                m_lastTurretRotation = this.m_turret.transform.rotation;
            }

            m_lerpTime = 0.0f;
            m_hasData = true;
        }
    }

    void Update()
    {
        if (!m_photon.isMine && m_hasData)
        {
            m_lerpTime += Time.deltaTime * 9;
            this.transform.position = Vector3.Lerp(m_lastPosition, m_targetPosition, m_lerpTime);
            this.transform.rotation = Quaternion.Lerp(m_lastRotation, m_targetRotation, m_lerpTime);
            this.m_turret.transform.rotation = Quaternion.Lerp(m_lastTurretRotation, m_targetTurretRotation, m_lerpTime);

            PlayerShoot playerShoot = this.GetComponent<PlayerShoot>();

            if (HasShot)
            {
                playerShoot.Shoot(SideShot);
                HasShot = false;
            }

            if (playerShoot.Shooting)
            {
                playerShoot.CheckAnimation();
            }

            if (HasHit)
            {
                //Transform hit = ; // This line n3wt. Need to get the player that was hit.
                //playerShoot.Hit(hit);
                HasHit = false;
            }
        }
    }
}
