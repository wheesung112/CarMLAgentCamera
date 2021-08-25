using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

public class CarDriverAgent : Agent
{
    private Transform tr;
    private Rigidbody rb;

    public float moveSpeed = 1.5f;
    public float turnSpeed = 200.0f;

    public int nextCheckpoint = 1;

    private Renderer floorRd;
    private Material originMt;

    //public Material goodMt, badMt;

    private List<CheckpointSingle> checkpointSingleList;



    TrackCheckpoints trackCheckpoints;
    CheckpointSingle checkpointSingle;

    public override void Initialize()
    {


        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        trackCheckpoints = tr.parent.GetComponent<TrackCheckpoints>();
        Transform checkpointsTransform = transform.Find("CheckpointManager");


        MaxStep = 10000;
    }



    public override void OnEpisodeBegin()
    {
        trackCheckpoints.InitStage();
        nextCheckpoint = 1;





        rb.velocity = rb.angularVelocity = Vector3.zero;

        tr.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
        tr.localRotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 checkpointForward = GameObject.FindGameObjectWithTag("CHECKPOINT").transform.forward;
        float dirDot = Vector3.Dot(tr.forward, checkpointForward);
        sensor.AddObservation(dirDot);
        //print(dirDot);
        //print(checkpointObservations.name);
        //Vector3 checkpointForward = trackCheckpoints.GetNextCheckpoint(trackCheckpoints.nextCheckpointSingleIndex).transform.forward;
        //float directionDot = Vector3.Dot(transform.forward, checkpointForward);
        //sensor.AddObservation(directionDot);
        //sensor.AddObservation(targetTr.localPosition);  // (x, y, z)  3
        sensor.AddObservation(tr.localPosition);        // (x, y, z)  3
        sensor.AddObservation(rb.velocity.x);           // 1
        sensor.AddObservation(rb.velocity.z);           // 1

        AddReward(+0.0003f);

    }

    bool speedLimit;

    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.DiscreteActions;

        Vector3 dir = Vector3.zero;
        Vector3 rot = Vector3.zero;

        // Branch 0
        switch (action[0])
        {
            case 1: dir = tr.forward; break;
            case 2: dir = -tr.forward; break;
        }
        // Branch 1
        switch (action[1])
        {
            case 1: rot = -tr.up; break; //왼쪽 회전
            case 2: rot = tr.up; break;  //오른쪽 회전
        }

        tr.Rotate(rot, Time.fixedDeltaTime * turnSpeed);
        if (!speedLimit)
        {
            rb.AddForce(dir * moveSpeed, ForceMode.VelocityChange);

        }

        //print(rb.velocity.magnitude);

        if (rb.velocity.magnitude > 50)
        {
            speedLimit = true;
        }
        else
        {
            speedLimit = false;
        }

        // 마이너스 패널티
        //SetReward(-1 / (float)MaxStep); // 2000 -> 0.002 
        //print(GetCumulativeReward());
        if (tr.forward == dir)
        {
            AddReward(1 / (float)MaxStep);
        }

        //print("nextCheckpoint" + nextCheckpoint);
        //print("trackCheckpoints.nextCheckpointSingleIndex" + trackCheckpoints.nextCheckpointSingleIndex);
        //print("trackCheckpoints.checkpointSingleList.Count" + trackCheckpoints.checkpointSingleList.Count);
    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;
        actions.Clear();

        // Branch 0 - 이동 (정지/전진/후진) 0, 1, 2 : Size 3
        if (Input.GetKey(KeyCode.W))
        {
            actions[0] = 1; //전진
        }
        if (Input.GetKey(KeyCode.S))
        {
            actions[0] = 2; //후진
        }
        // Branch 1 - 회전 (정지/왼쪽회전/오른쪽회전) 0, 1, 2 : Size 3
        if (Input.GetKey(KeyCode.A))
        {
            actions[1] = 1; //왼쪽 회전
        }
        if (Input.GetKey(KeyCode.D))
        {
            actions[1] = 2; //오른쪽 회전
        }
    }


    //[SerializeField] private TrackCheckpoints trackCheckpoints;
    //[SerializeField] private Transform spawnPosition;

    //private CarDriver carDriver;

    //private void Awake()
    //{
    //    carDriver = GetComponent<CarDriver>();
    //}

    //private void Start()
    //{
    //    trackCheckpoints.OnCarCorrectCheckpoint += TrackCheckpoints_OnCarCorrectCheckpoint;
    //    trackCheckpoints.OnCarWrongCheckpoint += TrackCheckpoints_OnCarWrongCheckpoint;
    //}

    //private void TrackCheckpoints_OnCarWrongCheckpoint(object sender, TrackCheckpoints.CarCheckpointEventArgs e)
    //{
    //    if (e.carTransform == transform)
    //    {
    //        AddReward(-1f);
    //    }
    //}

    //private void TrackCheckpoints_OnCarCorrectCheckpoint(object sender, TrackCheckpoints.CarCheckpointEventArgs e)
    //{
    //    if (e.carTransform == transform)
    //    {
    //        AddReward(1f);
    //    }
    //}

    //public override void OnEpisodeBegin()
    //{
    //    //transform.position = spawnPosition.position + new Vector3(Random.Range(-5f, +5f), 0, Random.Range(-5f, +5f));
    //    //transform.forward = spawnPosition.forward;
    //    //trackCheckpoints.ResetCheckpoint(transform);
    //    carDriver.StopCompletely();
    //}

    //public override void CollectObservations(VectorSensor sensor)
    //{
    //    Vector3 checkpointForward = /*trackCheckpoints.GetNextCheckpoint(transform).*/transform.forward;
    //    float directionDot = Vector3.Dot(transform.forward, checkpointForward);
    //    sensor.AddObservation(directionDot);
    //}

    //public override void OnActionReceived(ActionBuffers actions)
    //{
    //    float forwardAmount = 0f;
    //    float turnAmount = 0f;

    //    switch (actions.DiscreteActions[0])
    //    {
    //        case 0: forwardAmount = 0f; break;
    //        case 1: forwardAmount = +1f; break;
    //        case 2: forwardAmount = -1f; break;
    //    }
    //    switch (actions.DiscreteActions[1])
    //    {
    //        case 0: turnAmount = 0f; break;
    //        case 1: turnAmount = +1f; break;
    //        case 2: turnAmount = -1f; break;
    //    }

    //    carDriver.SetInputs(forwardAmount, turnAmount);
    //}

    //public override void Heuristic(in ActionBuffers actionsOut)
    //{
    //    int forwardAction = 0;
    //    if (Input.GetKey(KeyCode.UpArrow)) forwardAction = 1;
    //    if (Input.GetKey(KeyCode.DownArrow)) forwardAction = 2;

    //    int turnAction = 0;
    //    if (Input.GetKey(KeyCode.RightArrow)) turnAction = 1;
    //    if (Input.GetKey(KeyCode.LeftArrow)) turnAction = 2;

    //    ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
    //    discreteActions[0] = forwardAction;
    //    discreteActions[1] = turnAction;
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            AddReward(-0.05f);
        }
        if (collision.gameObject.CompareTag("WALL"))
        {
            // Hit a Wall
            AddReward(-1.0f);
            //EndEpisode();
        }
        if (collision.gameObject.CompareTag("DEADLINE"))
        {
            AddReward(-0.5f);
            EndEpisode();
        }
        if (collision.gameObject.CompareTag("FINISHLINE"))
        {
            AddReward(+0.5f);
            EndEpisode();
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("WALL"))
        {
            // Hit a Wall
            AddReward(-0.1f);
            //EndEpisode();
        }

    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("CHECKPOINT") && trackCheckpoints.nextCheckpointSingleIndex == nextCheckpoint)
    //    {
    //        AddReward(+0.5f);
    //        //print("gogo");
    //        //print(trackCheckpoints.nextCheckpointSingleIndex);
    //        //print(nextCheckpoint);

    //        //Debug.Log("dd" + trackCheckpoints.nextCheckpointSingleIndex);
    //        //Debug.Log("dds" + nextCheckpoint);

    //        trackCheckpoints.GetNextCheckpoint(trackCheckpoints.nextCheckpointSingleIndex);
    //        nextCheckpoint++;

    //    }
    //    else
    //    {
    //        AddReward(-0.3f);
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CHECKPOINT") && trackCheckpoints.nextCheckpointSingleIndex == nextCheckpoint)
        {

            AddReward(+0.5f);
            //print("gogo");
            //print(trackCheckpoints.nextCheckpointSingleIndex);
            //print(nextCheckpoint);

            //Debug.Log("dd" + trackCheckpoints.nextCheckpointSingleIndex);
            //Debug.Log("dds" + nextCheckpoint);
            other.gameObject.SetActive(false);



            trackCheckpoints.GetNextCheckpoint(trackCheckpoints.nextCheckpointSingleIndex);
            nextCheckpoint++;

            if (trackCheckpoints.nextCheckpointSingleIndex == trackCheckpoints.checkpointSingleList.Count)
            {
                AddReward(+0.5f);
                EndEpisode();
            }

        }

    }





}
