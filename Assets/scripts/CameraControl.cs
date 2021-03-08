using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float cameraspeed=60.0f;
    public bool MoveCameraflag;

    private List<Vector3> CameraPosition = new List<Vector3>();

    private ParentManager ParentManager;
    void Start()
    {
        MoveCameraflag = false;
        ParentManager = GameObject.Find("ParentManager").GetComponent<ParentManager>();
        AddPositionList();
        transform.position = CameraPosition[ParentManager.stagenum];
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveCameraflag)
        {
            NextMoveCamera();
        }
    }

    private void AddPositionList()
    {
        CameraPosition.Add(new Vector3(-114.3f, -1.1f, -10f));
        CameraPosition.Add(new Vector3(-87f, -1.33f, -10f));
        CameraPosition.Add(new Vector3(-86.5f, 14f, -10f));
    }

    private void NextMoveCamera()
    {
        float step = cameraspeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position,CameraPosition[ParentManager.stagenum], step);
        if (transform.position == CameraPosition[ParentManager.stagenum])
        {
            MoveCameraflag = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
