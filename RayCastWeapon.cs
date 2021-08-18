using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 intialPostion;
        public Vector3 intialVelocity;
        public TrailRenderer tracer; 
    }
    public bool isFiring = false;
    public int fireRate = 25;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public Transform raycastOrigin;
    public Transform raycastEnd;

    Ray ray;
    RaycastHit hitInfo;
    float diffTime;
    List<Bullet> bullets = new List<Bullet>();
    float maxLife = 3.0f;

    Vector3 GetPosition(Bullet bullet)
    {
        // p + v*t + 0.5*g*t*t
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.intialPostion) + (bullet.intialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.intialPostion = position;
        bullet.intialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }
    public void StartFiring()
    {
        isFiring = true;
        diffTime = 0.0f;
        muzzleFlash.Emit(1);

        FireBullet();
    }

    public void UpdateFiring(float deltaTime)
    {
        diffTime += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while(diffTime >= 0.0f)
        {
            FireBullet();
            diffTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= maxLife);
    }
    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;

        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLife;
           // Debug.Log("RayCast! " + hitInfo.transform.name);
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.blue, 1.0f);
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }
    private void FireBullet()
    {
        Vector3 velocity = (raycastEnd.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);
        //ray.origin = raycastOrigin.position;
        //ray.direction = raycastEnd.position - raycastOrigin.position;

        //var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
        //tracer.AddPosition(ray.origin);

        //if (Physics.Raycast(ray, out hitInfo))
        //{
        //    hitEffect.transform.position = hitInfo.point;
        //    hitEffect.transform.forward = hitInfo.normal;
        //    hitEffect.Emit(1);

        //    tracer.transform.position = hitInfo.point;
        //    Debug.Log("RayCast! " + hitInfo.transform.name);
        //    Debug.DrawLine(ray.origin, hitInfo.point, Color.blue, 1.0f);
        //}
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
