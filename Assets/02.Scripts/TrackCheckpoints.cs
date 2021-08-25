using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{

    //public event EventHandler OnPlayerCorrectCheckpoint;
    //public event EventHandler OnPlayerWrongCheckpoint;

    //[SerializeField] private List<Transform> carTransformList;

    public List<CheckpointSingle> checkpointSingleList;
    public int nextCheckpointSingleIndex;
    //public Transform nextCheckpoint;
    public GameObject checkPointManager;

    public void InitStage()
    {
        Transform checkpointsTransform = transform.Find("CheckpointManager");

        //Destroy(GameObject.FindWithTag("CHECKPOINTMANAGER"));
        nextCheckpointSingleIndex = 0;
        //GameObject checkpointObject = Instantiate(checkPointManager);
        //checkpointObject.transform.position = transform.position;
        //GameObject.Find("Checkpoint").SetActive(true);

        //checkpointSingleList = new List<CheckpointSingle>();

        int countCheckpoint = 0;

        foreach (Transform child in checkpointsTransform)
        {
            countCheckpoint++;
        }


        for (int i = 0; i < countCheckpoint; i++)
        {
            GameObject parentGameObject = GameObject.FindGameObjectWithTag("CHECKPOINTMANAGER");
            GameObject childGameObject = parentGameObject.transform.GetChild(i).gameObject;
            if (childGameObject.activeSelf == false)
            {
                childGameObject.SetActive(true);


            }

        }

    }

    private void Awake()
    {


        Transform checkpointsTransform = transform.Find("CheckpointManager");

        //Transform child = checkpointsTransform.transform.GetChild(0).transform;

        //print(child.position);
        //foreach (Transform child in checkpointsTransform)
        //{

        //    print(child.transform.position);
        //}

        checkpointSingleList = new List<CheckpointSingle>();
        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();

            checkpointSingle.SetTrackCheckpoints(this);

            checkpointSingleList.Add(checkpointSingle);
        }

        //nextCheckpoint = checkpointsTransform.transform.GetChild(0).transform;


        nextCheckpointSingleIndex = 0;
    }

    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle)
    {
        //int nextCheckpointSingleIndex = nextCheckpointSingleIndex[carTransformList.IndexOf(carTransform)];
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            // Correct checkpoint
            //Debug.Log("Correct");
            //CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];

            nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1) % (checkpointSingleList.Count + 1);
            //OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
            //print(checkpointSingle.transform.position);
        }
        else
        {
            // Wrong checkpoint
            //Debug.Log("Wrong");
            //OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);

            //CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            //correctCheckpointSingle.Show();
        }
    }

    //public int i = 0;
    //public void GetNextCheckpointPosition(int j)
    //{

    //    Transform checkpointsTransform = transform.Find("CheckpointManager");

    //    Transform nextCheckpoint = checkpointsTransform.transform.GetChild(j).transform;
    //}


    public void GetNextCheckpoint(int i)
    {
        nextCheckpointSingleIndex = i++;
        //Transform checkpointsTransform = transform.Find("CheckpointManager");

        //Transform child = checkpointsTransform.transform.GetChild(i).transform;

        //print(child.position);
    }

}
