using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    ConstantForce cf;
    // Start is called before the first frame update
    
    private void Awake() 
    {
        cf = GetComponent<ConstantForce>();    
    }

    private void OnCollisionEnter(Collision other) 
    {
        cf.enabled = false;    
    }
}
