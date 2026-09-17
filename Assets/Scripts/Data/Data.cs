using System;
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
	
	[System.NonSerialized] public int currentLayer;
	[System.NonSerialized] public string currentArrow;
	[System.NonSerialized] public bool gameInitialized = false;
	[System.NonSerialized] public int lineCount = 0;
	
	[System.NonSerialized] public int _attempts = 3;
	
	public event Action<int> OnHealthChanged;

    public int attempts
    {
        get => _attempts;
        set
        {
            if (_attempts == value)
                return;

            _attempts = value;
            OnHealthChanged?.Invoke(_attempts);
        }
    }
	
	public float musicVolume = 0.35f;
	
	public void ResetData()
    {
        locations.Clear();
        occupiedPositions.Clear();
        heatMap.Clear();
        arrowConnections.Clear();
        arrowDict.Clear();
        gameObjectReference.Clear();

        currentLayer = 0;
        currentArrow = null;
        gameInitialized = false;
        lineCount = 0;

        attempts = 3;
    }
}
