using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

public class MoveArrow : MonoBehaviour
{	
	private Data gameData;
	private Dictionary<GameObject, GameObject> gameObjectReference;
	private Dictionary<string, HashSet<string>> arrowConnections;
	
	private Camera cam;
	private Coroutine _myCoroutine;
	private Coroutine wrongArrowCoroutine;
	private LineRenderer lineRenderer;
	
	private Vector3 startPosition;
    private Vector3 endPosition = Vector3.zero;
	private float rot;
    [SerializeField] private float duration = 2f;
	
	public static event Action OnCorrectMoveChanged;
	public static event Action OnWrongMoveChanged;
    
    void Start()
    {
		gameData = AssetManager.Instance.GameData;
		
		gameObjectReference = gameData.gameObjectReference;
		arrowConnections = gameData.arrowConnections;
		
		cam = AssetManager.Instance.Cam;
    }
    
    void Update()
    {	
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

			foreach(var kvp in gameObjectReference)
			{
				if(hit.collider != null && hit.collider.gameObject == kvp.Value)
				{
					if (_myCoroutine != null || wrongArrowCoroutine != null)
					{
						return; 
					}
					
					GetEndPos(kvp.Value);

					if (!isFree(kvp.Key))
					{
						wrongArrowCoroutine = StartCoroutine(MoveWrongArrow(kvp.Key, kvp.Value));
						gameData.attempts -= 1;
						OnWrongMoveChanged?.Invoke();
					}
					else
					{
						_myCoroutine = StartCoroutine(MoveLine(kvp.Key, kvp.Value));
						OnCorrectMoveChanged?.Invoke();
					}
				}
			}
        }
    }
	
	private void GetEndPos(GameObject head)
	{
		Vector3 screen = cam.ScreenToWorldPoint(
			new Vector3(Screen.width, Screen.height, 10)
		);

		Vector3 pos = head.transform.position;
		rot = head.transform.eulerAngles.z;
		
		float offset = 0.10f * screen.x;
		
		switch(rot)
		{
			case 0:
				endPosition = new Vector3(-screen.x + -offset, pos.y, 0);
				break;
			case 180:
				endPosition = new Vector3(screen.x + offset, pos.y, 0);
				break;
			case 270:
				endPosition = new Vector3(pos.x, screen.y + offset, 0);
				break;
			case 90:
				endPosition = new Vector3(pos.x, -screen.y + -offset, 0);
				break;
		}
	}
	
	private bool isFree(GameObject line)
	{
		string key = null;
			
		foreach(var kvp in arrowConnections)
		{
			if(line.name == kvp.Key)
			{
				key = kvp.Key;
			}
		}
		
		if(arrowConnections[key].Count == 0)
			return true;
		
		return false;
	}
	
	private void RemoveAndRelease(GameObject line, GameObject head)
	{
		foreach(var kvp in arrowConnections)
		{
			kvp.Value.Remove(line.name);
		}
		
		Destroy(line);
		Destroy(head);
		
		gameData.lineCount -= 1;
	}
	
	private IEnumerator MoveWrongArrow(GameObject line, GameObject head)
	{
		lineRenderer = line.GetComponent<LineRenderer>();
		startPosition = lineRenderer.GetPosition(0);
		
		Vector3 forwardPos = Vector3.zero;
		float offset = 1f;
		
		forwardPos = startPosition + rot switch
		{
			270 => new Vector3(0, offset, 0),
			90  => new Vector3(0, -offset, 0),
			0   => new Vector3(-offset, 0, 0),
			180 => new Vector3(offset, 0, 0),
			_   => Vector3.zero
		};

		yield return SetWrongArrowPos(startPosition, forwardPos, head);
		yield return SetWrongArrowPos(forwardPos, startPosition, head);
		
		wrongArrowCoroutine = null;
	}

	private IEnumerator SetWrongArrowPos(Vector3 from, Vector3 to, GameObject head)
	{
		float timeElapsed = 0f;

		while (timeElapsed < duration)
		{
			float t = timeElapsed / duration;

			Vector3 middle = Vector3.Lerp(from, to, t);
			head.transform.position = middle;
			lineRenderer.SetPosition(0, middle);

			timeElapsed += Time.deltaTime;

			yield return null;
		}

		head.transform.position = to;
	}
	
	private IEnumerator MoveLine(GameObject line, GameObject head)
	{	
		lineRenderer = line.GetComponent<LineRenderer>();
		startPosition = lineRenderer.GetPosition(0);
		int posCount = lineRenderer.positionCount;
		
		List<Vector3> posList = new List<Vector3>();
		
		for(int i = 0; i < posCount; i++)
		{
			Vector3 pos = lineRenderer.GetPosition(i);
			posList.Add(pos);
		}
		
		float timeElapsed = 0;

        while (timeElapsed < duration)
        {	
            float t = timeElapsed / duration;
            
			Vector3 middlePosition = Vector3.Lerp(startPosition, endPosition, t);
			posList[0] = middlePosition;
			
			for(int i = 1; i < posCount; i++)
			{
				middlePosition = Vector3.Lerp(posList[i], posList[i - 1], t);
				posList[i] = middlePosition;
			}
			
			lineRenderer.positionCount = posList.Count;
			lineRenderer.SetPositions(posList.ToArray());
			
			head.transform.position = posList[0];
            
            timeElapsed += Time.deltaTime;
			
			yield return null;
        }
		
		lineRenderer.SetPosition(0, endPosition);
		
		RemoveAndRelease(line, head);
		
		_myCoroutine = null;
	}
}
