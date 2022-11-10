using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : Photon.Pun.MonoBehaviourPun
{


    float yVelocity = 0f;
    [Range(5f, 25f)]
    public float gravity = 15f;
    [Range(5f, 15f)]
    public float movementSpeed = 10f;
    [Range(5f, 15f)]
    public float jumpSpeed = 10f;

    Transform cameraTransform;
    float pitch = 0f;
    [Range(1f, 90f)]
    public float maxPitch = 85f;
    [Range(-1f, -90f)]
    public float minPitch = -85f;
    [Range(0.5f, 5f)]
    public float mouseSensitivity = 2f;

    CharacterController cc;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        Look();
        Move();
    }

    void Look()
    {
        float xInput = Input.GetAxis("Mouse X") * mouseSensitivity;
        float yInput = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(0, xInput, 0);
        pitch -= yInput;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        Quaternion rot = Quaternion.Euler(pitch, 0, 0);
        cameraTransform.localRotation = rot;
    }

    void Move()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input = Vector3.ClampMagnitude(input, 1f);
        Vector3 move = transform.TransformVector(input) * movementSpeed;
        if (cc.isGrounded)
        {
            yVelocity = -gravity * Time.deltaTime;
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpSpeed;
            }
        }
        yVelocity -= gravity * Time.deltaTime;
        move.y = yVelocity;
        cc.Move(move * Time.deltaTime);
    }

}
