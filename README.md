# 3rd-Person-Camera
3rd person camera controller for Unity. The controller uses raytracing to ensure that the camera doesn't clip through other objects.

### Important Info:
The camera controller has the option to smoothly follow the player object. You need to create a separate transform to follow the player
with a delay so that the camera does not have issues with rotation. If you use this script you also need to turn off interpolation for the
player's Rigidbody. Otherwise, there will be issues with jerkiness.
