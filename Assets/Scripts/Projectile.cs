using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] ProjectileScriptableObject projectileStats;
    [SerializeField] AudioClip[] defaultHitSounds;
    [SerializeField] AudioClip fireSound;
    [SerializeField] AudioClip playerHitSound;

    [SerializeField] ParticleSystem playerHitFx;
    [SerializeField] ParticleSystem defaultHitFx;

    [SerializeField] bool debugMode;

    GameObject parentTank;
    Rigidbody rb;
        
    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        Destroy(this.gameObject, projectileStats.lifeTime);
    }

    private void Start() 
    {
        SoundManager.Instance.PlayOneShot(fireSound);
        rb.AddForce(transform.forward * projectileStats.projectileSpeed, ForceMode.VelocityChange);
    }

    public void SetParentTank(GameObject parent)
    {
        parentTank = parent;
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject == parentTank)
        {
            return;
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Instantiate(playerHitFx, other.contacts[0].point, Quaternion.identity);
            if(debugMode) Debug.Log("Projectile hit player");
            SoundManager.Instance.Play(playerHitSound);
        }
        else
        {
            Instantiate(defaultHitFx, other.contacts[0].point, Quaternion.identity);
            SoundManager.Instance.RandomSoundEffect(defaultHitSounds);
        }

        if(other.gameObject.GetComponent<Rigidbody>())
        {
            other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(projectileStats.impactForce, other.contacts[0].point, 3f, 0.5f);
        }        

        Destroy(this.gameObject);
    }
}
