using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterController character;
    public float jumpHeight;
    const float gravityAcc = -9.81f;
    public float velocity;
    public float rotationSpeed;
    public Transform playerTransformation;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // WASD movement by default
        float xAxisMovement;
        float zAxisMovement;
        float yAxisMovement;

        xAxisMovement = Input.GetAxis("Horizontal") * velocity * Time.deltaTime;
        zAxisMovement = Input.GetAxis("Vertical") * velocity * Time.deltaTime;

        yAxisMovement = gravityAcc * Time.deltaTime;

        // vector of relative movement, passed into the LookRotation method of the quaternion
        Vector3 movement = new Vector3(xAxisMovement, yAxisMovement, zAxisMovement).normalized;

        // if movement != 0.1, then calculate the appropriate angle
        if (movement.x != 0f || movement.z != 0f)
        {
            float eulerAngle = Mathf.Atan2(xAxisMovement, zAxisMovement) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(playerTransformation.rotation.eulerAngles.y, eulerAngle, ref rotationSpeed, 0.1f);
            // apply this to rotation
            playerTransformation.rotation = Quaternion.Euler(new Vector3(0f, smoothAngle, 0f));
        }
        
        character.Move(movement.normalized);
    }

}