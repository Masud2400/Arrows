using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallLogData : MonoBehaviour
{
	private Data gameData;
	private Dictionary<Vector2Int, GridCell> locations;
	private HashSet<Vector3> occupiedPositions;
	private Dictionary<string, List<VectorData>> arrowDict;
	private Dictionary<Vector3, VectorPositions> heatMap;
	private Dictionary<string, HashSet<string>> arrowConnections;
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
		
		locations = gameData.locations;
		arrowDict = gameData.arrowDict;
		heatMap = gameData.heatMap;
		occupiedPositions = gameData.occupiedPositions;
		arrowConnections = gameData.arrowConnections;
	}
	
	public void Log()
	{
		LogData.SaveConnections(arrowConnections);
		LogData.SaveArrowDict(arrowDict);
		//LogData.SaveHeatMap(heatMap);
		//LogData.SaveOccupiedPositions(occupiedPositions);
		LogData.SaveLocations(locations);
	}
}
