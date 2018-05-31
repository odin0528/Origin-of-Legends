using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcEffect : OneShotEffect {
	[Header("Prefabs")]
	public GameObject lineRendererPrefab;
	public GameObject lightRendererPrefab;

	[Header("Config")]
	public int chainLength;
	public int lightnings;

	private float nextRefresh;
	private float segmentLength = 0.2f;

	private List<LightningBolt> LightningBolts = new List<LightningBolt>();
	private List<Vector2> Targets = new List<Vector2>();

	void Start() {
		ShowDamage();
		Remove(0.5f);
	}

	public void SetTarget(Vector2 pos) {
		Targets.Add(pos);

		LightningBolt tmpLightningBolt = new LightningBolt(segmentLength, LightningBolts.Count, transform);
		tmpLightningBolt.Init(lightnings, lineRendererPrefab, lightRendererPrefab);
		LightningBolts.Add(tmpLightningBolt);
		LightningBolts[Targets.Count - 1].Activate();
	}

	void Update() {
		//Refresh the LightningBolts
		if(Time.time > nextRefresh) {
			//BuildChain();
			for(int i = 0;i < Targets.Count;i++) {
				if(i == 0) {
					LightningBolts[i].DrawLightning(transform.position, Targets[i]);
				} else {
					LightningBolts[i].DrawLightning(Targets[i - 1], Targets[i]);
				}
			}
			nextRefresh = Time.time + 0.03f;
		}
	}

	public class LightningBolt {
		public Transform root;
		public LineRenderer[] lineRenderer { get; set; }
		public LineRenderer lightRenderer { get; set; }

		public float SegmentLength { get; set; }
		public int Index { get; private set; }
		public bool IsActive { get; private set; }

		public LightningBolt(float segmentLength, int index, Transform _root) {
			root = _root;
			SegmentLength = segmentLength;
			Index = index;
		}

		public void Init(int lineRendererCount, GameObject lineRendererPrefab, GameObject lightRendererPrefab) {
			//Create the needed LineRenderer instances
			lineRenderer = new LineRenderer[lineRendererCount];
			for(int i = 0;i < lineRendererCount;i++) {
				lineRenderer[i] = (GameObject.Instantiate(lineRendererPrefab) as GameObject).GetComponent<LineRenderer>();
				lineRenderer[i].transform.SetParent(root);
				lineRenderer[i].enabled = false;
			}
			lightRenderer = (GameObject.Instantiate(lightRendererPrefab) as GameObject).GetComponent<LineRenderer>();
			lightRenderer.transform.SetParent(root);
			IsActive = false;
		}

		public void Activate() {
			//Active this LightningBolt with all of its LineRenderers
			for(int i = 0;i < lineRenderer.Length;i++) {
				lineRenderer[i].enabled = true;
			}
			lightRenderer.enabled = true;
			IsActive = true;
		}

		public void DrawLightning(Vector2 source, Vector2 target) {
			//Calculated amount of Segments
			float distance = Vector2.Distance(source, target);
			int segments = 5;
			if(distance > SegmentLength) {
				segments = Mathf.FloorToInt(distance / SegmentLength) + 2;
			} else {
				segments = 4;
			}

			for(int i = 0;i < lineRenderer.Length;i++) {
				// Set the amount of points to the calculated value
				lineRenderer[i].positionCount = segments;
				lineRenderer[i].SetPosition(0, source);
				Vector3 lastPosition = source;
				for(int j = 1;j < segments - 1;j++) {
					//Go linear from source to target
					Vector2 tmp = Vector2.Lerp(source, target, (float) j / (float) segments);
					//Add randomness
					lastPosition = new Vector3(tmp.x + Random.Range(-0.15f, 0.15f), tmp.y + Random.Range(-0.15f, 0.15f), 99f);
					//Debug.Log(lastPosition);
					//Set the calculated position
					lineRenderer[i].SetPosition(j, lastPosition);
				}
				lineRenderer[i].SetPosition(segments - 1, target);
			}
			//Set the points for the light
			lightRenderer.SetPosition(0, source);
			lightRenderer.SetPosition(1, target);
			//Set the color of the light
			Color lightColor = new Color(0.5647f, 0.58823f, 1f, Random.Range(0.2f, 1f));
			lightRenderer.startColor = lightColor;
			lightRenderer.endColor = lightColor;
		}
	}
}
