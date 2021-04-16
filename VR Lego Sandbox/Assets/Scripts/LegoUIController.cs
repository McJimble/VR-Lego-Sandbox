using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegoUIController : MonoBehaviour
{
    public GameObject testObj;
    public void TestSelectionEvent()
    {
        Instantiate(testObj, new Vector3(2, 4, 0), Quaternion.identity);
    }
}
