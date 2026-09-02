using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LogData
{
    [System.Serializable]
    public class ArrowEntry
    {
        public string keyObject;
        public List<VectorData> vectorData = new List<VectorData>();
    }

    [System.Serializable]
    public class ConnectionEntry
    {
        public string groupName;
        public List<string> connections = new List<string>();
    }

    [System.Serializable]
    public class LocationEntry
    {
        public int x;
        public int y;
        public Vector3 position;
		public int layer;
    }

    [System.Serializable]
    public class OccupiedPositionEntry
    {
        public Vector3 position;
    }
	
	[System.Serializable]
    public class HeatMapEntry
    {
        public Vector3 keyObject;
        public List<VectorPositions> vector = new List<VectorPositions>();
    }
	
	// Wrappers
    [System.Serializable]
    public class ArrowDictSaver
    {
        public List<ArrowEntry> arrows = new List<ArrowEntry>();
    }
	
	[System.Serializable]
    public class ConnectionsSaver
    {
		public List<ConnectionEntry> allConnections = new List<ConnectionEntry>();
	}
	
	[System.Serializable]
    public class LocationsSaver
    {
		public List<LocationEntry> locations = new List<LocationEntry>();
	}
	
	[System.Serializable]
    public class OccupiedPositionsSaver
    {
		public List<OccupiedPositionEntry> occupiedPositions = new List<OccupiedPositionEntry>();
	}
	
	[System.Serializable]
    public class HeatMapSaver
    {
        public List<HeatMapEntry> vectors = new List<HeatMapEntry>();
    }
	
	private static void SaveHelper(object wrapper, string fileName)
	{
		string json = JsonUtility.ToJson(wrapper, true);

        string documentsPath =
            System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.MyDocuments);

        string filePath = Path.Combine(documentsPath, $"{fileName}.json");

        File.WriteAllText(filePath, json);
	}
	
    public static void SaveArrowDict(Dictionary<string, List<VectorData>> arrowDict)
    {
        ArrowDictSaver wrapper = new ArrowDictSaver();
		string fileName = "ArrowDict";
		
        foreach (var kvp in arrowDict)
        {
            ArrowEntry entry = new ArrowEntry
            {
                keyObject = kvp.Key
            };

            foreach (VectorData x in kvp.Value)
            {
                entry.vectorData.Add(x);
            }

            wrapper.arrows.Add(entry);
        }
		
		SaveHelper(wrapper, fileName);
    }
	
	public static void SaveLocations(Dictionary<Vector2Int, GridCell> locations)
    {
        LocationsSaver wrapper = new LocationsSaver();
		string fileName = "Locations";
		
		foreach (var kvp in locations)
        {
            Vector2Int index = kvp.Key;

            GridCell cell = kvp.Value;

			wrapper.locations.Add(new LocationEntry
			{
				x = index.x,
				y = index.y,
				position = cell.position,
				layer = cell.layer
			});
        }
		
		SaveHelper(wrapper, fileName);
	}
	
	public static void SaveConnections(Dictionary<string, HashSet<string>> arrowConnections)
    {
        ConnectionsSaver wrapper = new ConnectionsSaver();
		string fileName = "Connections";
		
		foreach (var kvp in arrowConnections)
        {
            ConnectionEntry entry = new ConnectionEntry
            {
                groupName = kvp.Key
            };

            foreach (var go in kvp.Value)
            {
                entry.connections.Add(go != null ? go : null);
            }

            wrapper.allConnections.Add(entry);
        }
		
		SaveHelper(wrapper, fileName);
	}
	
	public static void SaveOccupiedPositions(HashSet<Vector3> occupiedPositions)
    {
        OccupiedPositionsSaver wrapper = new OccupiedPositionsSaver();
		string fileName = "OccupiedPositions";
		
		foreach (Vector3 position in occupiedPositions)
        {
            wrapper.occupiedPositions.Add(new OccupiedPositionEntry
            {
                position = position
            });
        }
		
		SaveHelper(wrapper, fileName);
	}
	
	public static void SaveHeatMap(Dictionary<Vector3, VectorPositions> heatMap)
    {
        HeatMapSaver wrapper = new HeatMapSaver();
		string fileName = "HeatMap";
		
        foreach (var kvp in heatMap)
        {
            HeatMapEntry entry = new HeatMapEntry
            {
                keyObject = kvp.Key
            };
			
			entry.vector.Add(kvp.Value);

            wrapper.vectors.Add(entry);
        }
		
		SaveHelper(wrapper, fileName);
    }
}
