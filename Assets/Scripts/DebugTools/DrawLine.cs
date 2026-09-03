using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
	private Data gameData;
	private Dictionary<string, List<VectorData>> arrowDict;
	private Dictionary<Vector3, VectorPositions> heatMap = new Dictionary<Vector3, VectorPositions>();
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
		
		//locations = gameData.locations;
		arrowDict = gameData.arrowDict;
		heatMap = gameData.heatMap;
		//occupiedPositions = gameData.occupiedPositions;
	}
	
	public void Log()
	{
		//LogData.SaveConnections(arrowConnections);
	}
}
