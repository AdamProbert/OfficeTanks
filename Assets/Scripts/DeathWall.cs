using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    [SerializeField] AudioClip disintegrateSound;
    [SerializeField] ParticleSystem disintegrateEffect;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EventManager.TriggerEvent("PlayerDeath", other.gameObject);
            Destroy(other.gameObject);
            SoundManager.Instance.PlayOneShot(disintegrateSound);
            Instantiate(disintegrateEffect, other.transform.position, other.transform.rotation);
        }
    }
}
