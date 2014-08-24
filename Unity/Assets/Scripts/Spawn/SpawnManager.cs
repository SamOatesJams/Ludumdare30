using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

    private Dictionary<Team, List<GameObject>> m_spawnPoints = null;

    private static SpawnManager s_instance = null;

    public static SpawnManager Instance
    { 
        get { return s_instance; }
    }

    void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }        
    }

	// Use this for initialization
	void Start ()
    {
        m_spawnPoints = new Dictionary<Team, List<GameObject>>();
        m_spawnPoints[Team.Bad] = new List<GameObject>();
        m_spawnPoints[Team.Good] = new List<GameObject>();

        var spawns = GameObject.FindObjectsOfType<SpawnPoint>();
        foreach (var spawn in spawns)
        {
            if (spawn.gameObject.activeSelf)
            {
                m_spawnPoints[spawn.Team].Add(spawn.gameObject);
            }
        }

        Debug.Log("[SpawnManager] Found " + m_spawnPoints[Team.Bad].Count + " Bad spawns and " + m_spawnPoints[Team.Good].Count + " Good Spawns.");
	}
	
    public GameObject GetSpawm(Team team)
    {
        var index = Random.Range(0, m_spawnPoints[team].Count);
        return m_spawnPoints[team][index];
    }
}
