using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCameraMovement : MonoBehaviour
{
    // player transformation, used to calculate camera position
    public Transform playerPosition;
    public float offsetX;
    public float offsetY;
    public float offsetZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move to the position of the player with a given offset
        transform.position = new Vector3(playerPosition.position.x + offsetX, playerPosition.position.y + offsetY, playerPosition.position.z + offsetZ);
    }
}
