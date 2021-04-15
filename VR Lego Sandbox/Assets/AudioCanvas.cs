using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCanvas : MonoBehaviour
{
    public AudioClip ClickButton;


    public void ButtonClick()
    {
        AudioSource.PlayClipAtPoint(ClickButton, transform.position, 1);

    }





}
