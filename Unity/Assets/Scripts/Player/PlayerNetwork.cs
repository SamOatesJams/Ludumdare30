using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerNetwork : MonoBehaviour 
{
    private Vector3 m_targetPosition = default(Vector3);
    private Quaternion m_targetRotation = default(Quaternion);
    private Vector3 m_lastPosition = default(Vector3);
    private Quaternion m_lastRotation = default(Quaternion);

    private bool m_hasPosition = false;

    private PhotonView m_photon = null;
    private float m_lerpTime = 0.0f;
    private bool m_hasData = false;

    public bool HasTeleported { get; set; }

    void Start()
    {
        m_photon = this.GetComponent<PhotonView>();
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)   
        {       
            //We own this player: send the others our data       
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);
            stream.SendNext(this.HasTeleported);

            // Reset flags
            this.HasTeleported = false;
        }   
        else   
        {       
            //Network player, receive data        
            m_targetPosition = (Vector3)stream.ReceiveNext();
            m_targetRotation = (Quaternion)stream.ReceiveNext();
            var didPortal = (bool)stream.ReceiveNext();

            if (!m_hasPosition || didPortal)
            {
                m_lastPosition = m_targetPosition;
                m_lastRotation = m_targetRotation;
                m_hasPosition = true;
            }
            else
            {
                m_lastPosition = this.transform.position;
                m_lastRotation = this.transform.rotation;
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
        }
    }
}
