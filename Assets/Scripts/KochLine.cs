using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochLine : KochGenerator {

    LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
