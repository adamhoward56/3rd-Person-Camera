using System.Collections;
    public float mouseSensitivity = 1;
        // Smoothly update the player follower's position
        target.position = Vector3.Lerp(target.position, playerBody.position + targetBodyOffset, Time.deltaTime * followSpeed);