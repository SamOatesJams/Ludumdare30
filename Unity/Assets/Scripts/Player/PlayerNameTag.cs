using UnityEngine;
using System.Collections;

public class PlayerNameTag : MonoBehaviour 
{
    public UILabel Label = null;
    private bool m_setup = false;

    void Start()
    {

    }

	// Update is called once per frame
	void Update () 
    {
        var cam = Camera.main;
        if (cam != null)
        {
            this.transform.LookAt(cam.transform);
        }
      
        if (!m_setup)
        {
            var photon = this.transform.parent.GetComponent<PhotonView>();
            if (photon != null && photon.owner != null)
            {
                Label.text = photon.owner.name;
            }            
            m_setup = true;
        }
	}
}
