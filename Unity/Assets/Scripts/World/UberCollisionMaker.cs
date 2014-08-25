using UnityEngine;
using System.Collections;

public class UberCollisionMaker : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

            var col = meshFilters[i].gameObject.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }

            i++;
        }
        
        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        this.GetComponent<MeshCollider>().sharedMesh = mesh;
	}
}
