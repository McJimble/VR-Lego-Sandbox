using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButton : MonoBehaviour
{
    public GameObject ColorSwatch;
    public GameObject player;
    //private int change = 0;
    public AudioClip Delete;

    [SerializeField] private Material startColorMaterial;
    [SerializeField] private GameObject currentButton;
    [SerializeField] private float moveAmount = 0.05f;

    private Color currentColor;

    private void Start()
    {
        currentColor = startColorMaterial.color;
    }

    public Color CurrentColor
    {
        get { return currentColor; }
    }

    public void SwapColor(Material colorMaterial)
    {
        currentColor = colorMaterial.color;
    }

    public void ChangeActiveButton(GameObject newButton)
    {
        if(currentButton != null)
        {
            // If we already have an active button, don't swap if new one is the same object.
            if (newButton.name == currentButton.name)
                return;

            currentButton.transform.position += currentButton.transform.forward * moveAmount;
        }

        currentButton = newButton;
        currentButton.transform.position -= currentButton.transform.forward * moveAmount;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other != null) { 
    //        switch (change) 
    //        {
    //            case 1:
    //                if (other.transform.tag == "LegoGroup")
    //                {
    //                    AudioSource.PlayClipAtPoint(Delete, transform.position, 1);
    //                    Destroy(other.gameObject);
    //                }
    //                break;
    //            case 2:
    //                if (other.transform.tag == "LegoGroup")
    //                {

    //                    GameObject newObj = Instantiate(other.gameObject, player.transform.position + (player.transform.forward * 3), Quaternion.identity);
    //                }
    //                break;
        
        
    //        }
    //    }


    //}
}
