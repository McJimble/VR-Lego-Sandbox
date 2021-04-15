using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLego : MonoBehaviour
{
    public AudioClip Spawn;
    public AudioClip CollideWithLego;
    public AudioClip CollideWithOther;

    //Play Spawn Sound
    void Awake()
    {
        AudioSource.PlayClipAtPoint(Spawn, transform.position, 1);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "LegoGroup")
        {
            AudioSource.PlayClipAtPoint(CollideWithLego, transform.position, 1);
        }
        else
        {
            AudioSource.PlayClipAtPoint(CollideWithOther, transform.position, 1);
        }

    }


}
