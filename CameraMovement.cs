using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivity = 1;
    public bool hideMouse;

    float yaw = 0;
    float pitch = 0;
    public Transform target;
    public Transform playerBody;
    public float targetDistance = 5;

    public float smoothing = .10f;
    private Vector3 currentRotation;
    private Vector3 rotationVelocity;
    private Vector3 targetBodyOffset;
    public float followSpeed = 2f;

    private bool lastFrameCollision = false;

    private void Start()
    {
        // Hide and lock the mouse
        if (hideMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        targetBodyOffset = target.position - playerBody.position;
    }

    private void LateUpdate()
    {
        // Update the camera's rotation
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -23, 85);
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationVelocity, smoothing);
        transform.eulerAngles = currentRotation;

        // Create and draw a ray
        float cameraOffset = 1.2f;
        float distance = Vector3.Distance(transform.position, target.transform.position) +  cameraOffset;
        Vector3 direction = transform.position - target.transform.position;
        Ray ray = new Ray(target.transform.position, direction / direction.magnitude);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.green); // Uncomment to view ray in editor

        // If there was a collision on the last frame, increase ray distance to prevent jumping
        if (lastFrameCollision)
        {
            distance += cameraOffset;
        }

        Vector3 nextPosition;
        // Update the position of the camera, based on whether or not we had a collision
        if (Physics.Raycast(ray, out RaycastHit obstruct, distance))
        {
            lastFrameCollision = true;
            nextPosition = target.position - transform.forward * (obstruct.distance - cameraOffset);
        }
        else
        {
            lastFrameCollision = false;
            nextPosition = target.position - transform.forward * (targetDistance - cameraOffset);
        }

        // Prevent camera from jumping if too far on floor
        if (Vector3.Distance(transform.position, target.transform.position) > targetDistance - cameraOffset)
        {
            lastFrameCollision = false;
            nextPosition = target.position - transform.forward * (targetDistance - cameraOffset);
        }

        // Update position
        transform.position = nextPosition;
    }
    
    private void FixedUpdate ()
    {
        // Smoothly update the player follower's position, must be in FixedUpdate when following a RigidBody or else camera jerks
        target.position = Vector3.Lerp(target.position, playerBody.position + targetBodyOffset, Time.deltaTime * followSpeed);
    }
}
