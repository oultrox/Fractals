using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochLine : KochGenerator {

    [Range(0,1)]
    public float lerpAmount;
    public float generateMultiplier = 1;
    Vector3[] lerpPositions;
    LineRenderer lineRenderer;

    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
        lerpPositions = new Vector3[positions.Length];
    }

    private void Update()
    {
        if (generationCount != 0)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                lerpPositions[i] = Vector3.Lerp(positions[i], targetPositions[i], lerpAmount);
            }
            

            if (isUsingBezierCurves)
            {
                bezierPositions = BezierCurve(lerpPositions, bezierVertexCount);
                lineRenderer.positionCount = bezierPositions.Length;
                lineRenderer.SetPositions(bezierPositions);
            }
            else
            {
                lineRenderer.positionCount = lerpPositions.Length;
                lineRenderer.SetPositions(lerpPositions);
            }
        }



    }
}
