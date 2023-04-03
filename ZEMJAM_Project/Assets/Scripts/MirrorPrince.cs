using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPrince : MonoBehaviour
{
    public void Mirror()
    {
        Movement player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        player.takeMirror(transform);
    }
}
