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
    private float nextJumpTime;
    private float landingTimeCheck = 0;
    private float distanceToGround;
    private bool jumping = false;
    private float lastHitTime = 0;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        distanceToGround = GetComponent<Collider>().bounds.extents.y;
        nextFireTime = Time.time;
        nextJumpTime = Time.time;
    }

    private void Update() 
    {   
        // Check if landed from purposeful jump
        if(landingTimeCheck < Time.time && jumping && IsGrounded())
        {
            jumping = false;
        }    
    }

    public void Move(float x, float z)
    {
        // Only move is purposefully jumped, or on ground.
        // Not when knocked into air
        if((jumping || IsGrounded()) && lastHitTime + .1f < Time.time)
        {
            if(debugMode) Debug.Log("Player is jumping or grounded. Move Enabled.");
            Vector3 movementDir = new Vector3(x, 0, z);
            Quaternion newrot = Quaternion.LookRotation(movementDir);
            tankBody.transform.rotation = Quaternion.Lerp(tankBody.transform.rotation, newrot, tankStats.bodyTurnSpeed);

            float currentSpeed = rb.velocity.magnitude;
            if(currentSpeed < tankStats.maxSpeed)
            {
                rb.AddForce(movementDir * tankStats.moveSpeed);
            }
            // If > maxspeed and we haven't just been hit, add accurate force to opposite direction to cap speed.
            else
            {
                float breakSpeed = currentSpeed - tankStats.maxSpeed;
                Vector3 normalizedVelocity = rb.velocity.normalized;
                Vector3 breakVelocity = normalizedVelocity * breakSpeed;
                rb.AddForce(-breakVelocity);
            }        
        }
    }

    public void Jump()
    {
        if(Time.time > nextJumpTime && IsGrounded())
        {
            SoundManager.Instance.PlayOneShot(tankStats.JumpSound);
            Instantiate(tankStats.JumpFx, transform.position + (Vector3.up * 0.3f), Quaternion.LookRotation(transform.up));
            rb.AddForce(new Vector3(0, tankStats.jumpVelocity, 0), ForceMode.Impulse);
            nextJumpTime = Time.time + tankStats.jumpRate;
            landingTimeCheck = Time.time + 1;
            jumping = true;
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

    public bool IsGrounded()
    {
        if(debugMode) Debug.DrawRay(transform.position, -Vector3.up, Color.red);
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            lastHitTime = Time.time;
        }    
    }
}
