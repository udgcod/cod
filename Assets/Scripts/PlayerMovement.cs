using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    
    public float walkSpeed = 5f;
    public float RunSpeed = 10f;
    public float jumpHeight = 1.1f;
    public float gravityScale = -20f;
    public float rotationSensibility = 10f;

    public Camera cameraPlayer;

    public float cameraVerticalAngle;
    Vector3 movement = Vector3.zero;
    Vector3 cameraRotation = Vector3.zero;
    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Look();
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

    }
    
    private void Move()
    {
        if (characterController.isGrounded)
        {
            movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            movement = Vector3.ClampMagnitude(movement, 1f);

            if (Input.GetButtonDown("Sprint"))
            {
                movement = transform.TransformDirection(movement) * RunSpeed;
            }else
            {
                movement = transform.TransformDirection(movement) * walkSpeed;
            }
            
           

            if (Input.GetButtonDown("Jump"))
            {
                movement.y = Mathf.Sqrt(jumpHeight * -2f * gravityScale);
            }
        }

        movement.y += gravityScale * Time.deltaTime;
        characterController.Move(movement * Time.deltaTime);
    }

    private void Look()
    {
        cameraRotation.x = Input.GetAxis("Mouse X") * rotationSensibility * Time.deltaTime;
        cameraRotation.y = Input.GetAxis("Mouse Y") * rotationSensibility * Time.deltaTime;

        cameraVerticalAngle += cameraRotation.y;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -60, 60);

        transform.Rotate(Vector3.up * cameraRotation.x);
        cameraPlayer.transform.localRotation = Quaternion.Euler(-cameraVerticalAngle, 0f, 0f);
    }
}
