using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochTrail : KochGenerator {


    public class TrailObject
    {
        public GameObject GO { get; set; }
        public TrailRenderer Trail { get; set; }
        public int CurrentTargetNum { get; set; }
        public Vector3 TargetPosition { get; set; }
        public Color EmissionColor { get; set; }

    }

    [HideInInspector]
    public List<TrailObject> trails;

    [Header("Trail Properties")]
    public GameObject trailPrefab;
    public AnimationCurve trailWidthCurve;

    [Range(0, 8)]
    public int trailEndCapVertices;
    public Material trailMaterial;
    public Gradient trailColor;

    [Header("Audio")]
    [SerializeField] private AudioPeer audioPeer;
    public int[] audioBand;

	// Use this for initialization
	void Start () {
        trails = new List<TrailObject>();
        for (int i = 0; i < initiatorPointAmount; i++)
        {
            GameObject trailInstance = (GameObject)Instantiate(trailPrefab, transform.position, Quaternion.identity, this.transform);
            TrailObject trailObjectsInstance = new TrailObject();
            trailObjectsInstance.GO = trailInstance;
            trailObjectsInstance.Trail = trailInstance.GetComponent<TrailRenderer>();
            trailObjectsInstance.Trail.material = new Material(trailMaterial);
            trailObjectsInstance.EmissionColor = trailColor.Evaluate(i * (1.0f / initiatorPointAmount));
            trailObjectsInstance.Trail.numCapVertices = trailEndCapVertices;

            Vector3 instantiatePosition;
            if (generationCount > 0)
            {
                int step;
                if (isUsingBezierCurves)
                {
                    step = bezierPositions.Length / initiatorPointAmount;
                    instantiatePosition = bezierPositions[i * step];
                    trailObjectsInstance.CurrentTargetNum = (i * step) + 1;

                } 
                else
                {
                    step = positions.Length / initiatorPointAmount;
                    instantiatePosition = positions[i * step];
                    trailObjectsInstance.CurrentTargetNum = (i * step) + 1;
                }
            }
            else
            {
                instantiatePosition = positions[i];
                trailObjectsInstance.CurrentTargetNum = i + 1;
                trailObjectsInstance.TargetPosition = positions[trailObjectsInstance.CurrentTargetNum];
            }

            trailObjectsInstance.GO.transform.localPosition = instantiatePosition;
            trails.Add(trailObjectsInstance);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
