using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    public List<Vector3> patrolPositions;
    public NavMeshAgent enemyAgent;
    private int positionIndex = 0;
    private int frameTimer = 0;
    private NavigationStates navState = new NavigationStates();

    private void Start()
    {
        navState = NavigationStates.patrol;
        // check navigation mode
        if (navState == NavigationStates.patrol)
        {
            // set position of navmesh agent
            enemyAgent.SetDestination(patrolPositions[positionIndex]);
        }
    }

    private void Update()
    {
        // check nav mode
        if (navState == NavigationStates.patrol)
        {
            if (enemyAgent.remainingDistance < 1f)
            {
                // move to next point
                if (positionIndex == patrolPositions.Count - 1) positionIndex = 0;
                else positionIndex++;
                
                // set the new position
                enemyAgent.SetDestination(patrolPositions[positionIndex]);
            }

            /*if (frameTimer % 30 == 0)
            {
                // get direction of movement
                const float rayLength = 5f;
                float eulerAngleBase = transform.rotation.eulerAngles.y;
                float eulerAngleCalc = 0f;
                float zComponent = 0f;
                float xComponent = 0f;
                RaycastHit hit;
                // check quadrant that the object is facing
                if ((eulerAngleBase >= 0f && eulerAngleBase < 90f) || eulerAngleBase == 360f)
                {
                    // first quadrant, don't modify value
                    eulerAngleCalc = eulerAngleBase;
                    xComponent = Mathf.Sin(eulerAngleCalc * Mathf.Deg2Rad) * Mathf.Rad2Deg * rayLength;
                    zComponent = Mathf.Cos(eulerAngleBase * Mathf.Deg2Rad) * Mathf.Rad2Deg * rayLength;
                }
                else if (eulerAngleBase >= 90f && eulerAngleBase <= 180f)
                {
                    eulerAngleCalc = eulerAngleBase - 90f;
                    xComponent = Mathf.Cos(eulerAngleCalc * Mathf.Deg2Rad) * Mathf.Rad2Deg * rayLength;
                    zComponent = Mathf.Sin(eulerAngleCalc * Mathf.Deg2Rad) * Mathf.Rad2Deg * rayLength * -1f;
                }
                else if (eulerAngleBase >= 180f && eulerAngleBase <= 270f)
                {
                    eulerAngleCalc = eulerAngleBase - 180f;
                    xComponent = Mathf.Cos(eulerAngleCalc * Mathf.Deg2Rad) * Mathf.Rad2Deg * rayLength * -1f;
                    zComponent = Mathf.Sin(eulerAngleCalc * Mathf.Deg2Rad) * Mathf.Rad2Deg * rayLength * -1f;
                }
                else if (eulerAngleBase >= 270f && eulerAngleBase <= 360f)
                {
                    eulerAngleCalc = eulerAngleBase - 270f;
                    xComponent = Mathf.Sin(eulerAngleCalc * Mathf.Deg2Rad) * Mathf.Rad2Deg * rayLength * -1f;
                    zComponent = Mathf.Cos(eulerAngleCalc * Mathf.Deg2Rad) * Mathf.Rad2Deg * rayLength;
                }

                // create a raycast with the calculated direction

                Physics.Raycast(gameObject.transform.position, new Vector3(xComponent, 0f, zComponent), out hit, rayLength);



                // compass directions
                
                NORTH =
                EAST = (1, 0, 0)
                SOUTH = (0, 0, -1)
                WEST = (-1, 0, 0)
                 
            }*/
        }
    }

    private enum NavigationStates
    {
        patrol,
        tracking,
        idle
    }
}