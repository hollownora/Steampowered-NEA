using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestructionController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> objectsToKeep = new List<GameObject>();

    void Start()
    {
        for (int destructionIndex = 0; destructionIndex < objectsToKeep.Count; destructionIndex++)
        {
            DontDestroyOnLoad(objectsToKeep[destructionIndex]);
        }

        // once this is done, notify the game manager
        FindObjectOfType<GameManager>().LoadReady();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
