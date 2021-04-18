using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCanvas : MonoBehaviour
{
    public AudioClip ClickButton;
    public AudioClip DeleteButton;

    public void ButtonClick()
    {
        AudioSource.PlayClipAtPoint(ClickButton, transform.position, 1);
    }

    public void DeleteLego()
    {
        AudioSource.PlayClipAtPoint(DeleteButton, transform.position, 1);
    }
}
