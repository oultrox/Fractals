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


    [SerializeField] protected Initiator initiatior = new Initiator();
    [SerializeField] protected float initiatorSize;
    protected int initiatorPointAmount;
    private Vector3[] initiatorPoints;
    private Vector3 rotateVector;
    private Vector3 rotateAxis;



    private void OnDrawGizmos()
    {
        GetInitiatorPoints();
        initiatorPoints = new Vector3[initiatorPointAmount];

        rotateVector = new Vector3(0, 0, 1);
        rotateAxis = new Vector3(0, 1, 0);
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

    private void GetInitiatorPoints()
    {
        switch (initiatior)
        {
            case Initiator.Triangle:
                initiatorPointAmount = 3;
                break;
            case Initiator.Square:
                initiatorPointAmount = 4;
                break;
            case Initiator.Pentagon:
                initiatorPointAmount = 5;
                break;
            case Initiator.Hexagon:
                initiatorPointAmount = 6;
                break;
            case Initiator.Heptagon:
                initiatorPointAmount = 7;
                break;
            case Initiator.Octagon:
                initiatorPointAmount = 8;
                break;
            default:
                break;
        }
    }
    
    
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
