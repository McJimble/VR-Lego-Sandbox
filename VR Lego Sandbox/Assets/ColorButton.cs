using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButton : MonoBehaviour
{
    public GameObject ColorSwatch;
    public Material Red;
    public Material Green;
    public Material Blue;
    private int change = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {

            switch (change)
            {
                case 0:
                    ColorSwatch.GetComponent<MeshRenderer>().material = Red;
                    change++;
                    break;

                case 1:
                    ColorSwatch.GetComponent<MeshRenderer>().material = Green;
                    change++;
                    break;
            
                case 2:
                    ColorSwatch.GetComponent<MeshRenderer>().material = Blue;
                    change = 0;
                    break;
            }
            
        }



    }
}
