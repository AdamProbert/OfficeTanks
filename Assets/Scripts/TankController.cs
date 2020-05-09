using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField] GameObject tankBody;
    [SerializeField] GameObject tankTurret;

    [SerializeField] Transform projectileSpawn;
    [SerializeField] GameObject projectile;

    [SerializeField] bool debugMode;
    private Rigidbody rb;
    public TankStatsScriptableObject tankStats;

    private float nextFireTime;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        nextFireTime = Time.time;
    }

    public void Move(float x, float z)
    {
        Vector3 movementDir = new Vector3(x, 0, z);
        Quaternion newrot = Quaternion.LookRotation(movementDir);
        tankBody.transform.rotation = Quaternion.Lerp(tankBody.transform.rotation, newrot, tankStats.bodyTurnSpeed);

        float currentSpeed = rb.velocity.magnitude;
        if(currentSpeed < tankStats.maxSpeed)
        {
            rb.AddForce(movementDir * tankStats.moveSpeed);
        }
        else // If > maxspeed, add accurate force to opposite direction.
        {
            float breakSpeed = currentSpeed - tankStats.maxSpeed;
            Vector3 normalizedVelocity = rb.velocity.normalized;
            Vector3 breakVelocity = normalizedVelocity * breakSpeed;
            rb.AddForce(-breakVelocity);
        }        
    }

    public void AimAtPoint(float x, float z)
    {   
        float y = tankTurret.transform.position.y;
        Vector3 aimPoint = new Vector3(x, y, z);
        tankTurret.transform.LookAt(aimPoint);
    }

    public void AimAtDirection(float x, float z)
    {
        float y = tankTurret.transform.position.y;
        // Vector3 aimDir = new Vector3(x, y, z);
        tankTurret.transform.eulerAngles = new Vector3( 0, Mathf.Atan2(x, z) * 180 / Mathf.PI, 0 );
    }

    public void Shoot()
    {
        if(Time.time > nextFireTime)
        {
            if(debugMode) Debug.Log("Shooting projectile");
            GameObject p = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
            p.GetComponent<Projectile>().SetParentTank(this.gameObject);    
            nextFireTime = Time.time + tankStats.fireRate;
        }        
    }
}
