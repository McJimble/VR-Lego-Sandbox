using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testspawn : MonoBehaviour
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject myPrefab;
    public GameObject player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Instantiate(myPrefab, new Vector3(1, 1, 1), Quaternion.identity);
            SpawnObject();
        }
        
    }

    public void SpawnObject()
    {
        // Old spawn code, changed below to always spawn where player is facing.
        //Instantiate(myPrefab, new Vector3(player.transform.position.x + 1, player.transform.position.y, player.transform.position.z + 1), transform.rotation);

        // Spawn in front of player by using the forward direction of the camera's transform and offset a bit.
        Instantiate(myPrefab, player.transform.position + (player.transform.forward * 3), Quaternion.identity);
    }

}
