using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastWeapon : MonoBehaviour
{
    public bool isFiring = false;
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public Transform raycastOrigin;
    public Transform raycastEnd;

    Ray ray;
    RaycastHit hitInfo;

    public void StartFiring()
    {
        isFiring = true;
        muzzleFlash.Emit(1);

       ray.origin = raycastOrigin.position;
       ray.direction = raycastEnd.position - raycastOrigin.position;

        var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
        tracer.AddPosition(ray.origin);

       if( Physics.Raycast(ray, out hitInfo) )
        {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            tracer.transform.position = hitInfo.point;
            //Debug.Log("RayCast! " + hitInfo.transform.name);
           // Debug.DrawLine(ray.origin, hitInfo.point, Color.blue, 1.0f);
        }
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
