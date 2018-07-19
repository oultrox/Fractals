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
    public Vector2 speedMinMax;


    //Private variales
    private float lerpPosSpeed;
    private float distanceSnap;


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
            trailObjectsInstance.Trail.widthCurve = trailWidthCurve;

            Vector3 instantiatePosition;
            if (generationCount > 0)
            {
                int step;
                if (isUsingBezierCurves)
                {
                    step = bezierPositions.Length / initiatorPointAmount;
                    instantiatePosition = bezierPositions[i * step];
                    trailObjectsInstance.CurrentTargetNum = (i * step) + 1;
                    trailObjectsInstance.TargetPosition = bezierPositions[trailObjectsInstance.CurrentTargetNum];

                } 
                else
                {
                    step = positions.Length / initiatorPointAmount;
                    instantiatePosition = positions[i * step];
                    trailObjectsInstance.CurrentTargetNum = (i * step) + 1;
                    trailObjectsInstance.TargetPosition = positions[trailObjectsInstance.CurrentTargetNum];
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

    private void Movement()
    {
        lerpPosSpeed = Mathf.Lerp(speedMinMax.x, speedMinMax.y, 100f);
        for (int i = 0; i < trails.Count; i++)
        {
            trails[i].GO.transform.localPosition = Vector3.MoveTowards(trails[i].GO.transform.localPosition, trails[i].TargetPosition, Time.deltaTime * lerpPosSpeed);
            distanceSnap = Vector3.Distance(trails[i].GO.transform.localPosition, trails[i].TargetPosition);

            if (distanceSnap < 0.05f)
            {
                trails[i].GO.transform.localPosition = trails[i].TargetPosition;
                if (isUsingBezierCurves && generationCount > 0)
                {
                    if (trails[i].CurrentTargetNum < bezierPositions.Length - 1)
                    {
                        trails[i].CurrentTargetNum += 1;
                    }
                    else
                    {
                        trails[i].CurrentTargetNum = 1;
                    }
                    trails[i].TargetPosition = bezierPositions[trails[i].CurrentTargetNum];
                }
                else
                {
                    if (trails[i].CurrentTargetNum < positions.Length - 1)
                    {
                        trails[i].CurrentTargetNum += 1;
                    }
                    else
                    {
                        trails[i].CurrentTargetNum = 1;
                    }
                    trails[i].TargetPosition = targetPositions[trails[i].CurrentTargetNum];
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        Movement();
	}
}
