using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls playerControls;

    public Rigidbody playerRB;
    public Camera playerCam;
    public Light flashLight;

    public GameObject bulletHitEffect;

    Vector2 moveRawValue;
    Vector2 camVelocity;
    public float mouseSensitivity = 0.5f;

    Vector3 moveVect;
    Vector3 camRotation;

    public float moveSpeed = 5f;

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Player.Fire.performed += OnFire;
        playerControls.Player.Move.performed += OnMove;
        playerControls.Player.Look.performed += OnLook;
        playerControls.Player.FlashLight.performed += OnFlash;

        flashLight.enabled = true;
    }

    

    private void OnFire(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        Debug.Log("Fire!");

        RaycastHit hit;

        //if the ray hits something in range
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 20f))
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }

            //play explosive animation at the location of the collision
            Instantiate(bulletHitEffect, hit.point, Quaternion.identity);
        }
        else
        {
            Debug.Log("No collision from raycast");
        }
    }

    private void OnFlash(InputAction.CallbackContext context)
    {
        if (flashLight.enabled)
        {
            //turn flashlight off
            flashLight.enabled = false;
        }
        else
        {
            //turn flashlight on
            flashLight.enabled = true;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        //gets normalized movement vector
        moveRawValue = context.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        //gets vector of the mouse (NOT NORMALIZED)
        camVelocity = context.ReadValue<Vector2>();

        camVelocity = camVelocity * mouseSensitivity;

        camRotation = new Vector3(camRotation.x -= camVelocity.y, camRotation.y += camVelocity.x, 0);

        //if the view goes over the top or bottom, reset to 90 degrees
        if(camRotation.x < -90)
        {
            camRotation.x = -90;
        }
        if(camRotation.x > 90)
        {
            camRotation.x = 90;
        }
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        playerCam.transform.rotation = Quaternion.Euler(camRotation);

        //rotate player around the y axis with the camera
        playerRB.transform.rotation = Quaternion.Euler(0, camRotation.y, 0);

        moveVect = this.transform.right * moveRawValue.x + this.transform.forward * moveRawValue.y;

        playerRB.position += moveVect * moveSpeed * Time.fixedDeltaTime;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}