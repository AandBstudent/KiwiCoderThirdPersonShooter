using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public Transform crossHairTarget;
    public UnityEngine.Animations.Rigging.Rig handIK;
    public Transform weaponParent;
    RayCastWeapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        RayCastWeapon existingWeapon = GetComponentInChildren<RayCastWeapon>();

        if(existingWeapon)
        {
            Equip(existingWeapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (weapon)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                weapon.StartFiring();
            }

            weapon.UpdateBullets(Time.deltaTime);
  
            if (Input.GetButtonUp("Fire1"))
            {
                weapon.StopFiring();
            }

            if (weapon.isFiring)
            {
                weapon.UpdateFiring(Time.deltaTime);
            }

        }
        else
        {
            handIK.weight = 0;
        }
    }

    public void Equip(RayCastWeapon newWeapon)
    {
        if(weapon)
        {
            Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        weapon.raycastEnd = crossHairTarget;
        weapon.transform.parent = weaponParent;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        handIK.weight = 1.0f;
    }
}
