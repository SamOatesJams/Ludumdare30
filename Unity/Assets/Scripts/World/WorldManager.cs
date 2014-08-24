using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {

    public string[] Worlds = null;

    public bool CreateColliders = false;

	// Use this for initialization
	void Awake () 
    {
        LoadAllWorlds();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (this.CreateColliders)
        {
            var meshFilters = GameObject.FindObjectsOfType<MeshFilter>();
            Debug.Log("Found Meshes: " + meshFilters.Length);

            foreach (var mesh in meshFilters)
            {
                if (mesh.GetComponent<Collider>() == false)
                {
                    var collider = mesh.gameObject.AddComponent<MeshCollider>();
                    collider.sharedMesh = mesh.mesh;
                }
            }

            this.CreateColliders = false;
        }
	}

    private void LoadAllWorlds()
    {
        foreach (var world in this.Worlds)
        {
            Application.LoadLevelAdditive(world);
        }
    }
}
