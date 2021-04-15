using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButton : MonoBehaviour
{
    public GameObject ColorSwatch;
    public Material Red;
    public Material Green;
    public Material Blue;
    public GameObject player;
    private int change = 0;
    public AudioClip Delete;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Hey Hamlin, just letting you know I made some changes and commented this out temporarily so that I could
        // link the color swapping to some multiplexed controls through a function. I hope you don't mind, just wanted
        // to show off what you took the time to implement for the second meeting!! You can change this back after it.

        /*
        if (Input.GetKeyDown(KeyCode.C))
        {

            SwapColor()
            
        }

        */

    }

    public void SwapColor()
    {
        switch (change)
        {
            case 0:
                ColorSwatch.GetComponent<MeshRenderer>().material.color = Red.color;
                change++;
                break;

            case 1:
                ColorSwatch.GetComponent<MeshRenderer>().material.color = Green.color;
                change++;
                break;

            case 2:
                ColorSwatch.GetComponent<MeshRenderer>().material.color = Blue.color;
                change = 0;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != null) { 
            switch (change) 
            {
                case 1:
                    if (other.transform.tag == "LegoGroup")
                    {
                        AudioSource.PlayClipAtPoint(Delete, transform.position, 1);
                        Destroy(other.gameObject);
                    }
                    break;
                case 2:
                    if (other.transform.tag == "LegoGroup")
                    {

                        GameObject newObj = Instantiate(other.gameObject, player.transform.position + (player.transform.forward * 3), Quaternion.identity);
                    }
                    break;
        
        
            }
        }


    }

    private void OnTriggerStay(Collider other)
    {

    }


    private void OnTriggerExit(Collider other)
    {

    }

}
