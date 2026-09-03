using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridManager : MonoBehaviour
{
	[Header("Main scripts")]
    [SerializeField] private GridGen gridGen;
	[SerializeField] private SetBlocks setBlocks;
	[SerializeField] private SetArrows setArrows;
	[SerializeField] private ExitChecker exitChecker;
	[SerializeField] private LineMaker lineMaker;
	
	private Data gameData;
	private Dictionary<Vector3, VectorPositions> heatMap;
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
		heatMap = gameData.heatMap;
		
		gridGen.GenerateGrid();
		//makeArrows();
	}
	
	public void makeArrows()
	{
		for(int i = 0; i < 5; i++)
		{
			setBlocks.SpawnBlock();
			setArrows.LayArrows();
			exitChecker.CheckExit();
		}
		
		lineMaker.DrawLine();
		
		/*
		int maxAttempts = 3000;
		int attempts = 0;

		while (!IsGridFull() && attempts < maxAttempts)
		{
			setBlocks.SpawnBlock();
			setArrows.LayArrows();
			exitChecker.CheckExit();
			attempts++;
		}
		
		lineMaker.DrawLine();
		
		if (attempts >= maxAttempts)
		{
			Debug.LogWarning("makeArrows stopped: maximum spawn attempts reached.");
			return;
		}*/
	}
	
	private bool IsGridFull()
	{
		foreach (VectorPositions vector in heatMap.Values)
		{
			if (!vector.isOccupied)
				return false;
		}

		return true;
	}
}
