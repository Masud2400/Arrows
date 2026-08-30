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
	
	void Start()
	{
		gridGen.GenerateGrid();
	}
	
	public void makeArrows()
	{	
		for(int i = 0; i < 50; i++)
		{
			setBlocks.SpawnBlock();
		
			setArrows.LayArrows();
			
			//exitChecker.CheckExit();
		}
		
		lineMaker.DrawLine();
	}
}
