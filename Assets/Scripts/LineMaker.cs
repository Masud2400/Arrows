using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LineMaker : MonoBehaviour
{
	private Data gameData;
	
    private Dictionary<string, List<VectorData>> arrowDict;
	
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
			spawnedHead.transform.position = kvp.Value[0].position;
			spawnedHead.transform.rotation = kvp.Value[0].rotation;
		}
	}
}
