using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ProjectileScriptableObject", order = 1)]
public class ProjectileScriptableObject : ScriptableObject
{
    public float projectileSpeed;
    public float damage;
    public float impactForce;
    public float lifeTime;
}