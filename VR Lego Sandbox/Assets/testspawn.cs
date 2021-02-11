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
        if (Input.GetMouseButtonDown(0))
        {
            //Instantiate(myPrefab, new Vector3(1, 1, 1), Quaternion.identity);
            Instantiate(myPrefab, new Vector3(player.transform.position.x + 1, player.transform.position.y, player.transform.position.z + 1), transform.rotation);
        }
        
    }

}
