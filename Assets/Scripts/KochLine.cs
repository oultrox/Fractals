using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochLine : KochGenerator {

    //[Range(0,1)]
    //public float lerpAmount;
    public float generateMultiplier = 1;

    [Header("Audio")]
    public AudioPeer audioPeer;
    public int[] audioBand;

    Vector3[] lerpPositions;
    LineRenderer lineRenderer;
    private float[] lerpAudio;


    // Use this for initialization
    void Start () {
        lerpAudio = new float[initiatorPointAmount];
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
            int count = 0;
            for (int i = 0; i < initiatorPointAmount; i++)
            {
                lerpAudio[i] = audioPeer.audiobandBuffer[audioBand[i]];
                Debug.Log((positions.Length - 1) / initiatorPointAmount);
                for (int j = 0; j < (positions.Length - 1) / initiatorPointAmount; j++)
                {
                    lerpPositions[count] = Vector3.Lerp(positions[count], targetPositions[count], lerpAudio[i]);
                    count++;
                }
            }

            lerpPositions[count] = Vector3.Lerp(positions[count], targetPositions[count], lerpAudio[initiatorPointAmount-1]);

            //for (int i = 0; i < positions.Length; i++)
            //{
            //    //CHECKING because of an AABB transform error.
            //    if (audioPeer.audiobandBuffer[audioBand] <= 1 && audioPeer.audiobandBuffer[audioBand] > 0)
            //    {
            //        lerpPositions[i] = Vector3.Lerp(positions[i], targetPositions[i], audioPeer.audiobandBuffer[audioBand]);
            //    }
            //}

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
