using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public PlayerMovement player;
    public Texture2D crosshair;

    public Vector3 relativePos = new Vector3(0, 1.5f, 0);
    private Vector2 crosshairPos = new Vector2(8, 8);

    private void Start()
    {
        

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(crosshair, crosshairPos, CursorMode.Auto);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = player.transform.position + relativePos;
    }
}
