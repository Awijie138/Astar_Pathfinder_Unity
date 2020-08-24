using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class NavigationDebugger : MonoBehaviour {

    [SerializeField]
    private NavMeshAgent agentToDebug;
    NavMeshPath path;
    NavMeshPathStatus pathStatus;
    NavMeshData navmeshData;

    private LineRenderer line;

    // Use this for initialization
    void Start () {

        line = GetComponent<LineRenderer>();
        agentToDebug = GetComponent<NavMeshAgent>();

	}
	
	// Update is called once per frame
	void Update () {
		if (agentToDebug.hasPath)
        {
            line.positionCount = agentToDebug.path.corners.Length;
            line.SetPositions(agentToDebug.path.corners);
            line.enabled = true;
        }
        else
        {
            line.enabled = false;
        }
	}
}
