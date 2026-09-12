using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GridCell
{
	public Vector3 position;
	public int layer;
	public string arrowName;
	public bool head = false;
	public int angle;
}

[System.Serializable]
public class VectorData
{
	public Vector3 position;
	public Quaternion rotation = Quaternion.identity;
	public bool head = false;
	public Vector2Int index;
	public int angle;
}

[System.Serializable]
public class VectorPositions
{
	public bool isOccupied = false;
	public Vector2Int vectorIndex;
}

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Data")]
public class Data : ScriptableObject
{	
	public Dictionary<Vector2Int, GridCell> locations = new Dictionary<Vector2Int, GridCell>();
	public HashSet<Vector3> occupiedPositions = new();
	public Dictionary<Vector3, VectorPositions> heatMap = new Dictionary<Vector3, VectorPositions>();
	public Dictionary<string, HashSet<string>> arrowConnections = new Dictionary<string, HashSet<string>>();
	public Dictionary<string, List<VectorData>> arrowDict = new Dictionary<string, List<VectorData>>();
	public Dictionary<GameObject, GameObject> gameObjectReference = new Dictionary<GameObject, GameObject>();
	
	public int currentLayer;
	public string currentArrow;
}
