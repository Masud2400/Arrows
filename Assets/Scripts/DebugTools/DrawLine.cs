using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{       
    private Data gameData;

	private Dictionary<Vector2Int, GridCell> locations;
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
		
		locations = gameData.locations;
	}
	
	public void MakeData()
	{
		Debug.Log(locations.Count);
		
		LogData.SaveToJsonTwo(locations);
	}
	
	/*
	public void Draw()
	{	
		GameObject spawnedObj = Instantiate(line, spawnParent);
		spawnedObj.transform.localPosition = buttonPositions[0];
	
		LineRenderer lineRenderer = spawnedObj.GetComponent<LineRenderer>();
		
		lineRenderer.startWidth = 10f;
        lineRenderer.endWidth = 10f;

		lineRenderer.useWorldSpace = false;
		lineRenderer.positionCount = 3;

		lineRenderer.SetPosition(0, buttonPositions[1]);
		lineRenderer.SetPosition(1, buttonPositions[2]);
		lineRenderer.SetPosition(2, buttonPositions[3]);
	}*/
}
