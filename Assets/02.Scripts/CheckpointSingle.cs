using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    private CarDriverAgent carDriverAgent;
    private TrackCheckpoints trackCheckpoints;
    //private MeshRenderer meshRenderer;

    private void Awake()
    {
        //meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            trackCheckpoints.CarThroughCheckpoint(this);
        }
    }

    //private void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (collider.gameObject.CompareTag("Player"))
    //    {
    //        trackCheckpoints.CarThroughCheckpoint(this, collider.transform);
    //    }
    //}

    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;
    }

    

}
