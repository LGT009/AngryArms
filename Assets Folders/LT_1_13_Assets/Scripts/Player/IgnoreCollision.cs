using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    public Collider[] colliderToIgnore;
    public Collider myCollider;

    void Start()
    {
        
        if (myCollider != null && colliderToIgnore != null)
        {
            for(int i = 0; i < colliderToIgnore.Length; i++){
                Physics.IgnoreCollision(myCollider, colliderToIgnore[i], true);   
            }
            
        }
    }
}