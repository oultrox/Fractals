using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochLine : KochGenerator {

    [Header("Audio")]
    [SerializeField] private AudioPeer audioPeer;
    [SerializeField] private int audioBand;

    [Header("Color")]
    [SerializeField] private Material material;
    [SerializeField] private Color color;
    [SerializeField] private int audioBandMaterial;
    [SerializeField] private float emissionMultiplier = 4;

    private Vector3[] lerpPositions;
    private LineRenderer lineRenderer;
    private Material materialInstance;
    //private float[] lerpAudio;

    // Use this for initialization
    void Start () {
        //lerpAudio = new float[initiatorPointAmount];
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
        lerpPositions = new Vector3[positions.Length];
        //apply material
        materialInstance = new Material(material);
        lineRenderer.material = materialInstance;
    }

    private void Update()
    {
        materialInstance.SetColor("_EmissionColor", color * audioPeer.audiobandBuffer[audioBandMaterial] * emissionMultiplier);
        if (generationCount != 0)
        { 
            //Multiple lerps por cada lado en frecuencia: usar esto.
            //int count = 0;
            //for (int i = 0; i < initiatorPointAmount; i++)
            //{
            //    lerpAudio[i] = audioPeer.audiobandBuffer[audioBand[i]];
            //    for (int j = 0; j < (positions.Length - 1) / initiatorPointAmount; j++)
            //    {
            //        lerpPositions[count] = Vector3.Lerp(positions[count], targetPositions[count], lerpAudio[i]);
            //        count++;
            //    }
            //}
            //lerpPositions[count] = Vector3.Lerp(positions[count], targetPositions[count], lerpAudio[initiatorPointAmount-1]);

            for (int i = 0; i < positions.Length; i++)
            {
                //CHECKING because of an AABB transform error.
                if (audioPeer.audiobandBuffer[audioBand] <= 1 && audioPeer.audiobandBuffer[audioBand] > 0)
                {
                    lerpPositions[i] = Vector3.Lerp(positions[i], targetPositions[i], audioPeer.audiobandBuffer[audioBand]);
                }
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
