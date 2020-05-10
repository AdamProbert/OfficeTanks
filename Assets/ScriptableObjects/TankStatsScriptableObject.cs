using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TankStatsScriptableObject", order = 1)]
public class TankStatsScriptableObject : ScriptableObject
{
    public string tankName;
    public string tankDescription;
    public float moveSpeed;
    public float jumpVelocity;
    public float jumpRate;
    public float maxSpeed;
    public float turretTurnSpeed;
    public float bodyTurnSpeed;
    public float bulletSpeed;
    public float fireRate;

    public ParticleSystem JumpFx;
    public AudioClip JumpSound;
}