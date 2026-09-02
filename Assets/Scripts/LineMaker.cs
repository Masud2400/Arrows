using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LineMaker : MonoBehaviour
{
	private Data gameData;
	
    private Dictionary<string, List<VectorData>> arrowDict;
	private Dictionary<Vector2Int, GridCell> locations;
	private Dictionary<GameObject, List<GameObject>> gameObjectReference;
	
	private GameObject line;
    private Transform parent;
    private LineRenderer lineRenderer;
	
	private GameObject head;
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
		line = AssetManager.Instance.Line;
        parent = AssetManager.Instance.SpawnParent;
		head = AssetManager.Instance.Head;
		
		arrowDict = gameData.arrowDict;
		locations = gameData.locations;
		gameObjectReference = gameData.gameObjectReference;
	}
	
	public void DrawLine()
	{
		foreach(var kvp in arrowDict)
		{
			GameObject spawnedLine = Instantiate(line, parent);
			spawnedLine.name = kvp.Key;
			lineRenderer = spawnedLine.GetComponent<LineRenderer>();
			
			gameObjectReference[spawnedLine] = new List<GameObject>();
			
			lineRenderer.positionCount = kvp.Value.Count;
			
			lineRenderer.SetPositions(kvp.Value.Select(v => v.position).ToArray());
			
			lineRenderer.startWidth = 0.13f;
			lineRenderer.endWidth = 0.13f;
			
			GameObject spawnedHead = Instantiate(head, spawnedLine.transform);
			spawnedHead.transform.position = kvp.Value[0].position;
			spawnedHead.transform.rotation = kvp.Value[0].rotation;
			
			gameObjectReference[spawnedLine].Add(spawnedHead);
			
			SpriteRenderer sprite = spawnedHead.GetComponent<SpriteRenderer>();
			
			Vector2Int index = kvp.Value[0].index;
			int layer = locations[index].layer;
			
			float hue = ((layer - 1) * 0.61803398875f) % 1.0f;
			
			sprite.color = Color.HSVToRGB(hue, 0.5f, 1.0f);
			
			Color initialColor = Color.HSVToRGB(hue, 0.5f, 1.0f);
			Color lastColor = Color.HSVToRGB(hue, 0.5f, 1.0f);
			
			lineRenderer.startColor = initialColor;
			lineRenderer.endColor = lastColor;
		}
	}
}
