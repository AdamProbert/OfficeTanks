using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(TankController))]
public class AgentAI : MonoBehaviour
{
    [SerializeField] AIScriptableObject aistats;
    [SerializeField] bool debugMode;

    List<GameObject> playerList = new List<GameObject>();
    GameObject targetPlayer;
    TankController tankController;
    float timer;
    Vector3 targetNavPoint = Vector3.zero;
    
    private void Awake() 
    {
        tankController = GetComponent<TankController>();    
    }
    // Start is called before the first frame update
    void Start()
    {
        timer = aistats.wanderTimer;
        GetAllPlayers();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Wander();
        if(targetPlayer == null || DistanceToTarget(targetPlayer.transform) > aistats.shootDistance)
        {
            SelectNewTarget();
        }
        if(targetPlayer != null)
        {
            Shoot();
        }   
    }

    private void GetAllPlayers()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            Debug.Log("Player: " + player.name);
            if(player != this.gameObject)
            {
                playerList.Add(player);
            }
        }
    }

    private float DistanceToTarget(Transform target)
    {
        Vector3 diff = target.position - transform.position;
        return diff.sqrMagnitude;
    }

    private void SelectNewTarget()
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject player in playerList)
        {
            float curDistance = DistanceToTarget(player.transform);
            if (curDistance < distance)
            {
                closest = player;
                distance = curDistance;
            }
        }
        targetPlayer = closest;
    }

    private void Shoot()
    {
        Vector3 approxTarget = targetPlayer.transform.position;
        approxTarget.x += Random.Range(-aistats.accuracy, aistats.accuracy);
        approxTarget.z += Random.Range(-aistats.accuracy, aistats.accuracy);
        approxTarget.y = transform.position.y;
        if(debugMode) Debug.DrawLine(transform.position, approxTarget, Color.red);
        tankController.AimAtPoint(approxTarget.x, approxTarget.z);
        tankController.Shoot();
    }

    private void Wander()
    {
        timer += Time.deltaTime;
        if(timer >= aistats.wanderTimer)
        {
            targetNavPoint = RandomNavSphere(transform.position, aistats.wanderRadius, -1);
            timer = 0;
        }

        if(targetNavPoint != Vector3.zero)
        {
            Vector3 dir = targetNavPoint - transform.position;
            dir = dir.normalized;
            if(debugMode) Debug.DrawRay(transform.position, dir, Color.green);
            tankController.Move(dir.x, dir.z);
        }
    }

    public Vector3 RandomNavSphere (Vector3 origin, float distance, int layermask) 
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;    
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition (randomDirection, out navHit, distance, layermask);
        return navHit.position;
    }

    private void RemoveDeadPlayerFromKnownPlayers(GameObject player)
    {
        playerList.Remove(player);
    }

    private void OnEnable()
    {
        EventManager.StartListening("PlayerDeath", RemoveDeadPlayerFromKnownPlayers);
    }
 
    private void OnDisable()
    {
        EventManager.StopListening("PlayerDeath", RemoveDeadPlayerFromKnownPlayers);
    }
}
