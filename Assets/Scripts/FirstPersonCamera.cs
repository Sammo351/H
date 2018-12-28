using UnityEngine;
using System;

public class FirstPersonCamera : MonoBehaviour
{

    private Transform _camera;
    public FirstPersonController controller;

    public float Speed = 5f;

    public bool InvertX = false;
    public bool InvertY = false;

    public float MinAngle = -90f;
    public float MaxAngle = 90f;

    public float CameraMovementSpeed = 10;
    public float StandingHeight = 1.8f;
    public float CrouchingHeight = 0.9f;


    private void Start()
    {
        _camera = transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
        }

        if (InvertX) h *= -1;
        if (InvertY) v *= -1;

        var euler = _camera.transform.localEulerAngles;

        euler.x = euler.x + (v * Speed * Time.deltaTime);

        if (euler.x > 180)
            euler.x = euler.x - 360f;

        euler.x = Mathf.Clamp(euler.x, MinAngle, MaxAngle);
        _camera.transform.localEulerAngles = euler;
        controller.Rotate(h * Speed * Time.deltaTime);


        //Move Camera Vertical height
        Vector3 pos = _camera.transform.localPosition;
        float desiredHeight = controller.states.IsCrouching ? CrouchingHeight : StandingHeight;
        pos.y = Mathf.MoveTowards(pos.y, desiredHeight, CameraMovementSpeed * Time.deltaTime);
        _camera.transform.localPosition = pos;
    }


}