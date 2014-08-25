using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerData))]
public class PlayerDamage : MonoBehaviour {

    /// <summary>
    /// Max health for the player
    /// </summary>
    public float MaxHealth = 100.0f;

    /// <summary>
    /// Level for health to be consided as low
    /// </summary>
    public float LowHealthLevel = 50.0f;

    /// <summary>
    /// Multiplier for particle emission rate
    /// </summary>
    public float ParticleEmissionMultiplier = 2;

    /// <summary>
    /// Effects to increment no matter what the health level is
    /// </summary>
    private ParticleEmitter[] m_effects;

    /// <summary>
    /// Effects to enable and increment only if the health is low
    /// </summary>
    private ParticleEmitter[] m_lowHealthEffects;

    /// <summary>
    /// The player data for this player
    /// </summary>
    private PlayerData m_data;

	// Use this for initialization
	void Start () {
        m_data = GetComponent<PlayerData>();

        m_effects = new ParticleEmitter[2];
        m_effects[0] = this.transform.FindChild("SK_RobotDude/SM_RobotBody/FX_L_Exhaust").particleEmitter;
        m_effects[1] = this.transform.FindChild("SK_RobotDude/SM_RobotBody/FX_R_Exhaust").particleEmitter;

        m_lowHealthEffects = new ParticleEmitter[2];
        m_lowHealthEffects[0] = this.transform.Find("SK_RobotDude/SM_RobotBody/FX_L_Pipe").particleEmitter;
        m_lowHealthEffects[1] = this.transform.Find("SK_RobotDude/SM_RobotBody/FX_R_Pipe").particleEmitter;
	}
	
	// Update is called once per frame
	void Update () {
        float health = 100 - m_data.Health;

        if (health > 0)
        {
            foreach (var effect in m_effects)
            {
                effect.emit = true;
                effect.minEmission = health * ParticleEmissionMultiplier;
                effect.maxEmission = 25 + (health * ParticleEmissionMultiplier);
            }
        }

        if (health >= LowHealthLevel)
        {
            foreach (var effect in m_lowHealthEffects)
            {
                effect.emit = true;
                effect.minEmission = health * ParticleEmissionMultiplier;
                effect.maxEmission = 25 + (health * ParticleEmissionMultiplier);
            }
        }
	}
}
