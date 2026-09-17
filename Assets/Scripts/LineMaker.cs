using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LineMaker : MonoBehaviour
{
	private Data gameData;
	
    private Dictionary<string, List<VectorData>> arrowDict;
	private Dictionary<Vector2Int, GridCell> locations;
	private Dictionary<GameObject, GameObject> gameObjectReference;
	
    private Transform parent;
	private GameObject line;
	private GameObject head;
    private LineRenderer lineRenderer;
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
		
        parent = AssetManager.Instance.SpawnParent;
		line = AssetManager.Instance.Line;
		head = AssetManager.Instance.Head;
		
		arrowDict = gameData.arrowDict;
		locations = gameData.locations;
		gameObjectReference = gameData.gameObjectReference;
	}
	
	private void DesignateColor(GameObject spawnedHead, VectorData val)
	{
		SpriteRenderer sprite = spawnedHead.GetComponent<SpriteRenderer>();
			
		Vector2Int index = val.index;
		int layer = locations[index].layer;
		
		float hue = ((layer - 1) * 0.61803398875f) % 1.0f;
		
		sprite.color = Color.HSVToRGB(hue, 0.5f, 1.0f);
		
		Color initialColor = Color.HSVToRGB(hue, 0.5f, 1.0f);
		Color lastColor = Color.HSVToRGB(hue, 0.5f, 1.0f);
		
		lineRenderer.startColor = initialColor;
		lineRenderer.endColor = lastColor;
	}
	
	public void DrawLine()
	{
		foreach(var kvp in arrowDict)
		{
			GameObject spawnedLine = Instantiate(line, parent);
			
			spawnedLine.name = kvp.Key;
			lineRenderer = spawnedLine.GetComponent<LineRenderer>();
			
			lineRenderer.positionCount = kvp.Value.Count;
			
			lineRenderer.SetPositions(kvp.Value.Select(v => v.position).ToArray());
			
			lineRenderer.startWidth = 0.13f;
			lineRenderer.endWidth = 0.13f;
			
			GameObject spawnedHead = Instantiate(head, spawnedLine.transform);
			
			VectorData val = kvp.Value[0];
			spawnedHead.transform.position = val.position;
			spawnedHead.transform.rotation = val.rotation;
			
			gameObjectReference[spawnedLine] = spawnedHead;
			
			DesignateColor(spawnedHead, val);
			
			gameData.lineCount += 1;
		}
	}
}
