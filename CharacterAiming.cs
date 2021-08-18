using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15f;
    public float aimDuration = 1f;
    Camera mainCamera;
    
    public Rig aimLayer;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);

        if(aimLayer)
        {
            //if (Input.GetButton("Fire2"))
            //{
            //    Debug.Log("Aiming");
            //    aimLayer.weight += Time.deltaTime / aimDuration;
            //}
            //else
            //{
            //    if (aimLayer.weight >= 0)
            //    {
            //        aimLayer.weight -= Time.deltaTime / aimDuration;
            //    }

            //}
            aimLayer.weight = 1.0f;
        }
    }
}
