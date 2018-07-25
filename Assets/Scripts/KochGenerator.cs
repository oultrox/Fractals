using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochGenerator : MonoBehaviour {

    protected enum Initiator
    {
        Triangle,
        Square,
        Pentagon,
        Hexagon,
        Heptagon,
        Octagon
    }

    protected enum Axis
    {
        XAxis,
        YAxis, 
        ZAxis
    }

    protected struct LineSegment
    {
        public Vector3 StartPosition { get; set; }
        public Vector3 EndPosition { get; set; }
        public Vector3 Direction { get; set; }
        public float Length { get; set; }
    }

    [System.Serializable]
    protected struct StartGen
    {
        public bool outwards;
        public float scale;
    }

    [SerializeField] private StartGen[] startGens;
    [SerializeField] protected Axis axis = new Axis();
    [SerializeField] protected Initiator initiatior = new Initiator();
    [SerializeField] protected float initiatorSize;
    [SerializeField] protected AnimationCurve generator;

    [Range(8,24)]
    [SerializeField] protected int bezierVertexCount = 8;
    [SerializeField] protected bool isUsingBezierCurves;

    protected int generationCount;
    protected int initiatorPointAmount;

    protected Vector3[] positions;
    protected Vector3[] targetPositions;
    protected Vector3[] bezierPositions;
    protected Keyframe[] keys;

    private float initialRotation;
    private Vector3[] initiatorPoints;
    private Vector3 rotateVector;
    private Vector3 rotateAxis;
    private List<LineSegment> lineSegments;


    private void Awake()
    {
        GetInitiatorPoints();
        positions = new Vector3[initiatorPointAmount + 1];
        targetPositions = new Vector3[initiatorPointAmount + 1];
        keys = generator.keys;
        lineSegments = new List<LineSegment>();

        rotateVector = Quaternion.AngleAxis(initialRotation, rotateAxis) * rotateVector;
        for (int i = 0; i < initiatorPointAmount; i++)
        {
            positions[i] = rotateVector * initiatorSize;
            rotateVector = Quaternion.AngleAxis(360 / initiatorPointAmount, rotateAxis) * rotateVector;
        }

        positions[initiatorPointAmount] = positions[0];
        targetPositions = positions;

        for (int i = 0; i < startGens.Length; i++)
        {
            KochGenerate(targetPositions, startGens[i].outwards, startGens[i].scale);
        }
    }

    protected void KochGenerate(Vector3[] _positionsAux, bool outwards, float generatorMultiplier)
    {
        lineSegments.Clear();
        for (int i = 0; i < _positionsAux.Length - 1; i++)
        {
            LineSegment line = new LineSegment();
            line.StartPosition = _positionsAux[i];
            if (i == _positionsAux.Length - 1)
            {
                line.EndPosition = _positionsAux[0];
            }
            else
            {
                line.EndPosition = _positionsAux[i + 1];
            }
            line.Direction = (line.EndPosition - line.StartPosition).normalized;
            line.Length = Vector3.Distance(line.EndPosition, line.StartPosition);
            lineSegments.Add(line);
        }

        // add the line segment points to a point array.
        List<Vector3> newPos = new List<Vector3>();
        List<Vector3> targetPos = new List<Vector3>();

        for (int i = 0; i < lineSegments.Count; i++)
        {
            newPos.Add(lineSegments[i].StartPosition);
            targetPos.Add(lineSegments[i].StartPosition);

            for (int j = 1; j < keys.Length - 1; j++)
            {
                float moveAmount = lineSegments[i].Length * keys[j].time;
                float heightAmount = (lineSegments[i].Length * keys[j].value) * generatorMultiplier;
                Vector3 movePos = lineSegments[i].StartPosition + (lineSegments[i].Direction * moveAmount);
                Vector3 dir;
                if (outwards)
                {
                    dir = Quaternion.AngleAxis(-90, rotateAxis) * lineSegments[i].Direction;
                }
                else
                {
                    dir = Quaternion.AngleAxis(90, rotateAxis) * lineSegments[i].Direction;
                }
                newPos.Add(movePos);
                targetPos.Add(movePos + (dir * heightAmount));
            }
        }
        newPos.Add(lineSegments[0].StartPosition);
        targetPos.Add(lineSegments[0].StartPosition);

        positions = new Vector3[newPos.Count];
        positions = newPos.ToArray();

        targetPositions = new Vector3[targetPos.Count];
        targetPositions = targetPos.ToArray();
        bezierPositions = BezierCurve(targetPositions, bezierVertexCount);
        generationCount++;
    }

    protected Vector3[] BezierCurve(Vector3[] points, int vertexCount)
    {
        var pointList = new List<Vector3>();
        for (int i = 0; i < points.Length; i += 2)
        {
            if (i + 2 <= points.Length - 1)
            {
                for (float ratio = 0f; ratio <= 1f; ratio += 1.0f / vertexCount)
                {
                    var tangentLineVertex1 = Vector3.Lerp(points[i], points[i + 1], ratio);
                    var tangentLineVertex2 = Vector3.Lerp(points[i + 1], points[i + 2], ratio);
                    var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
                    pointList.Add(bezierPoint);
                }
            }
        }
        return pointList.ToArray();
    }

    private void GetInitiatorPoints()
    {
        switch (initiatior)
        {
            case Initiator.Triangle:
                initiatorPointAmount = 3;
                initialRotation = 0;
                break;
            case Initiator.Square:
                initiatorPointAmount = 4;
                initialRotation = 45;
                break;
            case Initiator.Pentagon:
                initiatorPointAmount = 5;
                initialRotation = 36;
                break;
            case Initiator.Hexagon:
                initiatorPointAmount = 6;
                initialRotation = 30;
                break;
            case Initiator.Heptagon:
                initiatorPointAmount = 7;
                initialRotation = 25.71428f;
                break;
            case Initiator.Octagon:
                initiatorPointAmount = 8;
                initialRotation = 22.5f;
                break;
            default:
                initiatorPointAmount = 3;
                initialRotation = 0;
                break;
        }

        switch (axis)
        {
            case Axis.XAxis:
                rotateVector = new Vector3(1, 0, 0);
                rotateAxis = new Vector3(0, 0, 1);
                break;
            case Axis.YAxis:
                rotateVector = new Vector3(0, 1, 0);
                rotateAxis = new Vector3(1, 0, 0);
                break;
            case Axis.ZAxis:
                rotateVector = new Vector3(0, 0, 1);
                rotateAxis = new Vector3(0, 1, 0);
                break;
            default:
                rotateVector = new Vector3(0, 1, 0);
                rotateAxis = new Vector3(1, 0, 0);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        GetInitiatorPoints();
        initiatorPoints = new Vector3[initiatorPointAmount];

        rotateVector = Quaternion.AngleAxis(initialRotation, rotateAxis) * rotateVector;
        for (int i = 0; i < initiatorPointAmount; i++)
        {
            initiatorPoints[i] = rotateVector * initiatorSize;
            rotateVector = Quaternion.AngleAxis(360 / initiatorPointAmount, rotateAxis) * rotateVector;
        }

        for (int i = 0; i < initiatorPointAmount; i++)
        {
            Gizmos.color = Color.white;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = rotationMatrix;
            if (i < initiatorPointAmount - 1)
            {
                Gizmos.DrawLine(initiatorPoints[i], initiatorPoints[i + 1]);
            }
            else
            {
                Gizmos.DrawLine(initiatorPoints[i], initiatorPoints[0]);
            }
        }
    }

}
