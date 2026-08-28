using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridManager : MonoBehaviour
{
	[Header("Main scripts")]
    [SerializeField] private GridGen gridGen;
	
	void Start()
	{
		gridGen.GenerateGrid();
	}
	
	public void makeArrows()
	{	
		
	}
}
