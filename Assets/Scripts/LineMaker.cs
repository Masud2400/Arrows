using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LineMaker : MonoBehaviour
{
	private Data gameData;
	
	[SerializeField] private PoolManager poolManager;
	
    private Dictionary<string, List<VectorData>> arrowDict;
	private Dictionary<Vector2Int, GridCell> locations;
	private Dictionary<GameObject, GameObject> gameObjectReference;
	
    private Transform parent;
    private LineRenderer lineRenderer;
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
        parent = AssetManager.Instance.SpawnParent;
		
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
			GameObject line = poolManager.poolLine.Get();
			line.transform.SetParent(parent.transform);
			
			line.name = kvp.Key;
			lineRenderer = line.GetComponent<LineRenderer>();
			
			lineRenderer.positionCount = kvp.Value.Count;
			
			lineRenderer.SetPositions(kvp.Value.Select(v => v.position).ToArray());
			
			lineRenderer.startWidth = 0.13f;
			lineRenderer.endWidth = 0.13f;
			
			GameObject head = poolManager.poolHead.Get();
			head.transform.SetParent(line.transform);
			
			VectorData val = kvp.Value[0];
			head.transform.position = val.position;
			head.transform.rotation = val.rotation;
			
			gameObjectReference[line] = head;
			
			DesignateColor(head, val);
		}
	}
}
