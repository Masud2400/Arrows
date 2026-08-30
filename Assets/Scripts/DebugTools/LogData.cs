using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LogData
{
    [System.Serializable]
    public class ArrowEntry
    {
        public string keyObject;
        public List<Vector3> positions = new List<Vector3>();
		public List<float> rotations = new List<float>();
		public List<bool> head = new List<bool>();
		public List<Vector2Int> index = new List<Vector2Int>();
		public List<int> angle = new List<int>();
    }

    [System.Serializable]
    public class ConnectionEntry
    {
        public string groupName;
        public List<string> connections = new List<string>();
    }

    // For Dictionary<int, Dictionary<int, Vector3>>
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
	
	private static void SaveHelper(object wrapper)
	{
		string json = JsonUtility.ToJson(wrapper, true);

        string documentsPath =
            System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.MyDocuments);

        string filePath = Path.Combine(documentsPath, "logData.json");

        File.WriteAllText(filePath, json);
	}
	
    public static void SaveArrowDict(Dictionary<string, List<VectorData>> arrowDict)
    {
        ArrowDictSaver wrapper = new ArrowDictSaver();
		
        foreach (var kvp in arrowDict)
        {
            ArrowEntry entry = new ArrowEntry
            {
                keyObject = kvp.Key
            };

            foreach (VectorData go in kvp.Value)
            {
                entry.positions.Add(go.position);
				entry.rotations.Add(go.rotation.eulerAngles.z);
				entry.head.Add(go.head);
				entry.index.Add(go.index);
				entry.angle.Add(go.angle);
            }

            wrapper.arrows.Add(entry);
        }
		
		SaveHelper(wrapper);
    }
	
	public static void SaveLocations(Dictionary<Vector2Int, GridCell> locations)
    {
        LocationsSaver wrapper = new LocationsSaver();
		
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
		
		SaveHelper(wrapper);
	}
	
	public static void SaveConnections(Dictionary<string, HashSet<string>> arrowConnections)
    {
        ConnectionsSaver wrapper = new ConnectionsSaver();
		
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
		
		SaveHelper(wrapper);
	}
	
	public static void SaveOccupiedPositions(HashSet<Vector3> occupiedPositions)
    {
        OccupiedPositionsSaver wrapper = new OccupiedPositionsSaver();
		
		foreach (Vector3 position in occupiedPositions)
        {
            wrapper.occupiedPositions.Add(new OccupiedPositionEntry
            {
                position = position
            });
        }
		
		SaveHelper(wrapper);
	}
}
