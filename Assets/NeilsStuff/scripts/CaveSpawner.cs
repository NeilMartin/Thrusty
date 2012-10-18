using UnityEngine;
using System.Collections;

public class CaveSpawner : MonoBehaviour 
{
	public GameObject cavePiece;
	public GameObject player;
	public float drawRange = 30.0f;
	public int seed = 0;
	
	private static int gridSizeX = 30;
	private static int gridSizeY = 30;
	private static int centreGridPosX = gridSizeX/2;
	private static int centreGridPosY = gridSizeY/2;
	private static float cellSize = 10.0f;
	private int[,] caveGrid;
	private GameObject[,] cellObject;
	private int oldCellStartX = 10;
	private int oldCellStartY = 10;
	private int oldCellEndX = -1;
	private int oldCellEndY = -1;
	private int spawnedPosX = 0;
	private int spawnedPosY = 0;
	
		
	// Use this for initialization
	void Start () 
	{
		GenerateCaveGrid();
		SpawnGrid( 0.0f, 0.0f, drawRange );
	}
	
	private void GenerateCaveGrid()
	{
		Random.seed = seed;
		float density = 30.0f;
		caveGrid = new int[gridSizeX,gridSizeY];
		for(int x=0;x<gridSizeX;++x)
		{
			for(int y=1;y<gridSizeY;++y)
			{
				caveGrid[x,y] = (Random.Range(0,100)<density)?1:0;
			}
		}
		for(int x=0;x<gridSizeX;++x)
		{
			caveGrid[x,0] = 1;
			caveGrid[0,x] = 1;
			caveGrid[x,gridSizeY-1] = 1;
			caveGrid[gridSizeX-1,x] = 1;
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
		// spawn all of the cells in active square if they are not spawned already
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
		// unspawn all of the cells in the previous square if they are not in the active square
		for(int x=oldCellStartX; x<=oldCellEndX; ++x )
		{
			for(int y=oldCellStartY; y<=oldCellEndY; ++y )
			{
				if(		( x<cellStartX )
					||	( x>cellEndX )
					||  ( y<cellStartY )
					||	( y>cellEndY ) )
				{
					unspawnCell(x,y);
				}
			}
		}
		// record the current active square so it can be deleted next time the player mo
		oldCellStartX = cellStartX;
		oldCellStartY = cellStartY;
		oldCellEndX = cellEndX;
		oldCellEndY = cellEndY;
		// record the current position of the player in cell space
		spawnedPosX = cellPosX;
		spawnedPosY = cellPosY;
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
				|  (GetCell( ix+1, iy-1 )>0?1:0)<<4
				|  (GetCell( ix+1, iy+1 )>0?1:0)<<5
				|  (GetCell( ix-1, iy+1 )>0?1:0)<<6
				|  (GetCell( ix-1, iy-1 )>0?1:0)<<7
				|  (GetCell( ix+0, iy+0 )>0?1:0)<<8;
		GameObject go = null;
		if( type > 0 )
		{
			Vector3 pos = new Vector3( (float)(ix-centreGridPosX)*cellSize, (float)(iy-centreGridPosY)*cellSize, 0.0f );
			Quaternion rot = new Quaternion();
			go = (GameObject)Instantiate( cavePiece, pos, rot );
			CavePiece piece = go.GetComponent<CavePiece>();
			int seed = ix*256+iy;
			piece.SetType(type, seed);
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
	
	void FixedUpdate() 
	{
		float xpos = 0.0f;
		float ypos = 0.0f;
		if( player != null )
		{
			xpos = player.transform.position.x;
			ypos = player.transform.position.y;
		}
		int cellPosX = centreGridPosX + (int)( xpos/cellSize );
		int cellPosY = centreGridPosY + (int)( ypos/cellSize );
		if(		( cellPosX != spawnedPosX ) 
			|| 	( cellPosY != spawnedPosY ) )
		{
			SpawnGrid( xpos, ypos, drawRange );
		}
	}

}
