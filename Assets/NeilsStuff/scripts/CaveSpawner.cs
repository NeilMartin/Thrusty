using UnityEngine;
using System.Collections;

public class CaveSpawner : MonoBehaviour 
{
	public GameObject cavePiece;
	
	private int gridSizeX = 10;
	private int gridSizeY = 10;
	private int[,] caveGrid;
	private GameObject[,] cellObject;
	private int centreGridPosX = 5;
	private int centreGridPosY = 5;
	private float cellSize = 10.0f;
	private int oldCellStartX = 10;
	private int oldCellStartY = 10;
	private int oldCellEndX = -1;
	private int oldCellEndY = -1;
		
	// Use this for initialization
	void Start () 
	{
		GenerateCaveGrid();
		SpawnGrid( 0.0f, 0.0f, 50.0f );
	}
	
	private void GenerateCaveGrid()
	{
		caveGrid = new int[gridSizeX,gridSizeY];
		for(int x=0;x<gridSizeX;++x)
		{
			for(int y=1;y<gridSizeY;++y)
			{
				caveGrid[x,y] = Random.Range(0,100)&1;
			}
		}
		cellObject = new GameObject[gridSizeX,gridSizeY]; // how do I create an array of nulls?
		for(int x=0;x<gridSizeX;++x)
		{
			for(int y=1;y<gridSizeY;++y)
			{
				cellObject[x,y] = null;
			}
		}
		caveGrid[centreGridPosX,centreGridPosY] = 0;
	}
	
	private void SpawnGrid( float xpos, float ypos, float range )
	{
		int cellRange = (int)Mathf.Max( range/cellSize, 1.0f );
		int cellPosX = centreGridPosX + (int)( xpos/cellSize );
		int cellPosY = centreGridPosY + (int)( ypos/cellSize );
		int cellStartX = Mathf.Max( cellPosX-cellRange, 0 );
		int cellStartY = Mathf.Max( cellPosY-cellRange, 0 );
		int cellEndX = Mathf.Min( cellPosX+cellRange, gridSizeX-1 );
		int cellEndY = Mathf.Min( cellPosY+cellRange, gridSizeY-1 );
		for(int x=cellStartX; x<=cellEndX; ++x )
		{
			for(int y=cellStartY; y<=cellEndY; ++y )
			{
				if( cellObject[x,y] == null )
				{
					cellObject[x,y] = spawnCell(x,y);
				}
			}
		}
		for(int x=oldCellStartX; x<=oldCellEndX; ++x )
		{
			for(int y=oldCellStartY; y<=oldCellEndY; ++y )
			{
				if(		( x<cellStartX )
					||	( x>cellEndX )
					||  ( x<cellStartX )
					||	( x>cellEndX ) )
				{
					unspawnCell(x,y);
				}
			}
		}
		oldCellStartX = cellStartX;
		oldCellStartY = cellStartY;
		oldCellEndX = cellEndX;
		oldCellEndY = cellEndY;
	}
	
	void unspawnCell( int ix, int iy )
	{
		if( null != cellObject[ix,iy] )
		{
			Destroy( cellObject[ix,iy] );
			cellObject[ix,iy] = null;
		}
	}
	
	GameObject spawnCell( int ix, int iy )
	{
		int type = (GetCell( ix+0, iy-1 )>0?1:0)<<0
				|  (GetCell( ix+1, iy+0 )>0?1:0)<<1
				|  (GetCell( ix+0, iy+1 )>0?1:0)<<2
				|  (GetCell( ix-1, iy+0 )>0?1:0)<<3
				|  (GetCell( ix+0, iy+0 )>0?1:0)<<4;
		GameObject go = null;
		if( type > 0 )
		{
			Vector3 pos = new Vector3( (float)(ix-centreGridPosX)*cellSize, (float)(iy-centreGridPosY)*cellSize, 0.0f );
			Quaternion rot = new Quaternion();
			go = (GameObject)Instantiate( cavePiece, pos, rot );
			CavePiece piece = go.GetComponent<CavePiece>();
			piece.SetType(type);
		}
		return go;
	}
	
	int GetCell( int ix, int iy )
	{
		int cellValue = 1;	
		if(    (ix>0)
			&& (ix<gridSizeX)
			&& (iy>0)
			&& (iy<gridSizeY))
		{
			cellValue = caveGrid[ix,iy];
		}
		return cellValue;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
