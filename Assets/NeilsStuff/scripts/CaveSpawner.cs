using UnityEngine;
using System.Collections;

public class CaveSpawner : MonoBehaviour 
{
	public GameObject cavePiece;
	public GameObject turretObject;
	public GameObject crystalObject;
	public GameObject player;
	public Material material;
	public float drawRange = 30.0f;
	public int seed = 0;
	public float density = 30.0f;
	public float thickness = 10.0f;
	public float chanceOfCrystal = 0.0f;
	public float chanceOfTurret = 0.0f;
	public int gridSizeX = 30;
	public int gridSizeY = 30;
	public bool spawnOutOfReach = true;
	
	private int centreGridPosX;
	private int centreGridPosY;
	private static float cellSize = 10.0f;
	private int[,] caveGrid;
	private GameObject[,] cellObject;
	private bool[,] mFirstTime;
	private int oldCellStartX = 10;
	private int oldCellStartY = 10;
	private int oldCellEndX = -1;
	private int oldCellEndY = -1;
	private int spawnedPosX = 0;
	private int spawnedPosY = 0;
	
		
	// Use this for initialization
	void Start () 
	{
		centreGridPosX = gridSizeX/2;
		centreGridPosY = gridSizeY/2;
		GenerateCaveGrid();
		SpawnGrid( 0.0f, 0.0f, cellSize*Mathf.Max(gridSizeX,gridSizeY), transform.position );
	}
	
	private void GenerateCaveGrid()
	{
		Random.seed = seed;
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
			caveGrid[x,gridSizeY-1] = 1;
		}
		for(int y=0;y<gridSizeY;++y)
		{
			caveGrid[0,y] = 1;
			caveGrid[gridSizeX-1,y] = 1;
		}
		cellObject = new GameObject[gridSizeX,gridSizeY]; // how do I create an array of nulls?
		mFirstTime = new bool[gridSizeX,gridSizeY]; 
		for(int x=0;x<gridSizeX;++x)
		{
			for(int y=1;y<gridSizeY;++y)
			{
				cellObject[x,y] = null;
				mFirstTime[x,y] = true;
			}
		}
		caveGrid[centreGridPosX,centreGridPosY] = 0;
		if(false == spawnOutOfReach)
		{
			Paint(centreGridPosX,centreGridPosY,2,1);
		}
	}

	struct PaintPoint
	{
		public PaintPoint( int x, int y )
		{
			mX = x;
			mY = y;
		}
		
		public int GetX()
		{
			return mX;
		}

		public int GetY()
		{
			return mY;
		}

		int mX;
		int mY;
	};

	private void Paint( int x, int y, int paintValue, int wallValue )
	{
		ArrayList paintList = new ArrayList();
		if( caveGrid[x,y] != wallValue )
		{
			paintList.Add( new PaintPoint(x,y) );
		}
		
		while( paintList.Count > 0 )
		{
			PaintPoint pp = (PaintPoint)paintList[0];
			caveGrid[pp.GetX(),pp.GetY()] = paintValue;
			paintList.RemoveAt(0);
			if(    ( caveGrid[pp.GetX()-1,pp.GetY()+0] != wallValue )
				&& ( caveGrid[pp.GetX()-1,pp.GetY()+0] != paintValue ) )
			{
				paintList.Add( new PaintPoint( pp.GetX()-1,pp.GetY()+0 ) );
			}
			if(    ( caveGrid[pp.GetX()+1,pp.GetY()+0] != wallValue )
				&& ( caveGrid[pp.GetX()+1,pp.GetY()+0] != paintValue ) )
			{
				paintList.Add( new PaintPoint( pp.GetX()+1,pp.GetY()+0 ) );
			}
			if(    ( caveGrid[pp.GetX()+0,pp.GetY()+1] != wallValue )
				&& ( caveGrid[pp.GetX()+0,pp.GetY()+1] != paintValue ) )
			{
				paintList.Add( new PaintPoint( pp.GetX()+0,pp.GetY()+1 ) );
			}
			if(    ( caveGrid[pp.GetX()+0,pp.GetY()-1] != wallValue )
				&& ( caveGrid[pp.GetX()+0,pp.GetY()-1] != paintValue ) )
			{
				paintList.Add( new PaintPoint( pp.GetX()+0,pp.GetY()-1 ) );
			}
		}
	}
	
	private void SpawnGrid( float xpos, float ypos, float range, Vector3 origin )
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
					cellObject[x,y] = spawnCell(x,y,origin);
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
	
	GameObject spawnCell( int ix, int iy, Vector3 origin )
	{
		int iBottom			= GetCell( ix+0, iy-1 );
		int iRight 			= GetCell( ix+1, iy+0 );
		int iTop 			= GetCell( ix+0, iy+1 );
		int iLeft 			= GetCell( ix-1, iy+0 );
		int iBottomRight  	= GetCell( ix+1, iy-1 );
		int iTopRight		= GetCell( ix+1, iy+1 );
		int iTopLeft		= GetCell( ix-1, iy+1 );
		int iBottomLeft		= GetCell( ix-1, iy-1 );
		int iCentre			= GetCell( ix+0, iy+0 );
		bool bReachable = (	(iBottom!=0)
						&&	(iRight!=0)
						&&	(iTop!=0)
						&&	(iLeft!=0)
						&&	(iBottomRight!=0)
						&&	(iTopRight!=0)
						&&	(iTopLeft!=0)
						&&	(iBottomLeft!=0)
						&&	(iCentre!=0) );
		bool bBottom		= iBottom==1;
		bool bRight 		= iRight==1;
		bool bTop 			= iTop==1;
		bool bLeft 			= iLeft==1;
		bool bBottomRight  	= iBottomRight==1;
		bool bTopRight		= iTopRight==1;
		bool bTopLeft		= iTopLeft==1;
		bool bBottomLeft	= iBottomLeft==1;
		bool bCentre		= iCentre==1;
		int type = (bBottom		?(1<<0):0)
				|  (bRight		?(1<<1):0)
				|  (bTop		?(1<<2):0)
				|  (bLeft		?(1<<3):0)
				|  (bBottomRight?(1<<4):0)
				|  (bTopRight	?(1<<5):0)
				|  (bTopLeft	?(1<<6):0)
				|  (bBottomLeft	?(1<<7):0)
				|  (bCentre		?(1<<8):0);
		GameObject go = null;
		if( type > 0 )
		{
			Vector3 pos = new Vector3( (float)(ix-centreGridPosX)*cellSize, (float)(iy-centreGridPosY)*cellSize, 0.0f ) + origin;
			Quaternion rot = new Quaternion();
			go = (GameObject)Instantiate( cavePiece, pos, rot );
			CavePiece piece = go.GetComponent<CavePiece>();
			int seed = ix*256+iy;
			piece.SetType(type, seed, material, thickness);
			// if this is the first time we've spawned this cell, then let it know it must spawn turrets/crystals/whatever
			if( mFirstTime[ix,iy] )
			{
				mFirstTime[ix,iy] = false;
				bool[] bOccupied = new bool[8] {false, false, false, false, false, false, false, false };
				// only spawn if this is reachable
				if((bReachable) || (spawnOutOfReach))
				{
					piece.SetSpawners( turretObject, chanceOfTurret, ref bOccupied );
				}
				piece.SetSpawners( crystalObject, chanceOfCrystal, ref bOccupied );
			}
		}
		return go;
	}
	
	int GetCell( int ix, int iy )
	{
		int cellValue = 1;	
		if(    (ix>=0)
			&& (ix<gridSizeX)
			&& (iy>=0)
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
			SpawnGrid( xpos, ypos, drawRange, transform.position );
		}
	}

}
