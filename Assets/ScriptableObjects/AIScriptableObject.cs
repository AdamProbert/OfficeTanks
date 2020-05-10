using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AIScriptableObject", order = 1)]
public class AIScriptableObject : ScriptableObject
{
    public float wanderTimer;
    public float wanderRadius;
    public float huntDistance;
    public float chaseTime;
    public float shootDistance;

    public float accuracy;
}