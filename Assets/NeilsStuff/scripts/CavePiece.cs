using UnityEngine;
using System.Collections;

public class CavePiece : MonoBehaviour 
{
	public Material 		visualMaterial;
	public PhysicMaterial	physicMaterial;
	private Vector3[] 	newVertices;
	private Vector3[] 	newNormals;
	private Vector2[] 	newUV;
	private int[]		newTriangles;
	private int         mType;
	
	public void SetType( int type )
	{
		mType = type;
	}
	
	// Use this for initialization
	void Start () 
	{
		gameObject.AddComponent("MeshFilter");
	    gameObject.AddComponent("MeshRenderer");
		GenerateFromType( mType );
		if( physicMaterial != null )
		{
			gameObject.AddComponent("MeshCollider");
			MeshCollider mc = GetComponent<MeshCollider>();
			mc.material = physicMaterial;
		}
	}
	
	void GenerateFromType( int type )
	{
		float size = 5.0f;
		float[] innerRing = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
		float[] outerRing = { 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f };
		float[] maxRing = { 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f };
		int numLengths = outerRing.Length;
		for(int i=0;i<numLengths;++i)
		{
			float rad = (2.0f*Mathf.PI/(float)numLengths)*(float)i;
			float vx = Mathf.Sin( rad )*2.0f;
			float vy = Mathf.Cos( rad )*2.0f;
			float vxs = Mathf.Abs( vx );
			if( vxs > 1.0f )
			{
				vx /= vxs;
				vy /= vxs;
			}
			float vys = Mathf.Abs( vy );
			if( vys > 1.0f )
			{
				vx /= vys;
				vy /= vys;
			}
			maxRing[i] = Mathf.Sqrt((vx*vx)+(vy*vy));
			outerRing[i] = maxRing[i];
		}
		if(type<16)
		{
			innerRing = new float[] { 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f };
			bool bDownClear = ((type&(1<<0))==0);
			bool bUpClear = ((type&(1<<2))==0);
			bool bLeftClear = ((type&(1<<3))==0);
			bool bRightClear = ((type&(1<<1))==0);
			if(bUpClear)
			{
				innerRing[0] = maxRing[0];
				innerRing[1] = maxRing[1];
				innerRing[15] = maxRing[15];
			}
			if(bRightClear)
			{
				innerRing[3] = maxRing[3];
				innerRing[4] = maxRing[4];
				innerRing[5] = maxRing[5];
			}
			if(bDownClear)
			{
				innerRing[7] = maxRing[7];
				innerRing[8] = maxRing[8];
				innerRing[9] = maxRing[9];
			}
			if(bLeftClear)
			{
				innerRing[11] = maxRing[11];
				innerRing[12] = maxRing[12];
				innerRing[13] = maxRing[13];
			}
		}
		else
		{
			bool bDownClear = ((type&(1<<0))==0);
			bool bUpClear = ((type&(1<<2))==0);
			bool bLeftClear = ((type&(1<<3))==0);
			bool bRightClear = ((type&(1<<1))==0);
			float fmin=0.7f;
			float fmax=0.9f;
			if(bUpClear)
			{
				outerRing[0] = maxRing[0] * Random.Range( fmin, fmax );
				outerRing[1] = maxRing[1] * Random.Range( fmin, fmax );
				outerRing[15] = maxRing[15] * Random.Range( fmin, fmax );
			}
			if(bRightClear)
			{
				outerRing[3] = maxRing[3] * Random.Range( fmin, fmax );
				outerRing[4] = maxRing[4] * Random.Range( fmin, fmax );
				outerRing[5] = maxRing[5] * Random.Range( fmin, fmax );
			}
			if(bDownClear)
			{
				outerRing[7] = maxRing[7] * Random.Range( fmin, fmax );
				outerRing[8] = maxRing[8] * Random.Range( fmin, fmax );
				outerRing[9] = maxRing[9] * Random.Range( fmin, fmax );
			}
			if(bLeftClear)
			{
				outerRing[11] = maxRing[11] * Random.Range( fmin, fmax );
				outerRing[12] = maxRing[12] * Random.Range( fmin, fmax );
				outerRing[13] = maxRing[13] * Random.Range( fmin, fmax );
			}
			GenerateFromRings( innerRing, outerRing, size );
		}
			

	}
	
	void GenerateFromRings( float[] innerRing, float[] outerRing, float size )
	{
		int numLengths = innerRing.Length;
		int numVertsPerLength = 4;
		newVertices = new Vector3[numLengths*numVertsPerLength];
		newNormals = new Vector3[numLengths*numVertsPerLength];
		newUV = new Vector2[numLengths*numVertsPerLength];
		float zscale = 0.2f;
		Vector3 vec = new Vector3();
		for(int i=0;i<numLengths;++i)
		{
			float rad = (2.0f*Mathf.PI/(float)numLengths)*(float)i;
			vec.x = Mathf.Sin( rad );
			vec.y = Mathf.Cos( rad );
			vec.z = 0.0f;
			int index = i*numVertsPerLength;
			newVertices[index+0] = vec*innerRing[i]*size;
			newVertices[index+1] = vec*innerRing[i]*size;
			newVertices[index+2] = vec*outerRing[i]*size;
			newVertices[index+3] = vec*outerRing[i]*size;
			newVertices[index+1].z -= size*zscale;
			newVertices[index+2].z -= size*zscale;
			newVertices[index+0].z += size*zscale;
			newVertices[index+3].z += size*zscale;
			newUV[index+0] = new Vector2( 0.0f, 0.0f );
			newUV[index+1] = new Vector2( 0.0f, 1.0f );
			newUV[index+2] = new Vector2( 1.0f, 1.0f );
			newUV[index+3] = new Vector2( 1.0f, 0.0f );
			newNormals[index+0] = -vec;
			newNormals[index+1] = -vec;
			newNormals[index+2] = vec;
			newNormals[index+3] = vec;
			newNormals[index+1].z = -1.0f;
			newNormals[index+2].z = -1.0f;
			newNormals[index+1].Normalize();
			newNormals[index+2].Normalize();
		}
		int numVertsPerTri = 3;
		int numTrisPerLength = 6;
		newTriangles = new int[numTrisPerLength*numLengths*numVertsPerTri];
		for(int i=0;i<numLengths;++i)
		{
			int index0 = i*numVertsPerLength;
			int index1 = index0+1;
			int index2 = index0+2;
			int index3 = index0+3;
			int index4 = ((i+1)%numLengths)*numVertsPerLength;
			int index5 = index4+1;
			int index6 = index4+2;
			int index7 = index4+3;
			int tindex = i*numTrisPerLength*numVertsPerTri;
			newTriangles[tindex+0] = index0; newTriangles[tindex+1] = index5; newTriangles[tindex+2] = index4;
			newTriangles[tindex+3] = index0; newTriangles[tindex+4] = index1; newTriangles[tindex+5] = index5;
			newTriangles[tindex+6] = index1; newTriangles[tindex+7] = index6; newTriangles[tindex+8] = index5;
			newTriangles[tindex+9] = index1; newTriangles[tindex+10]= index2; newTriangles[tindex+11]= index6;
			newTriangles[tindex+12] = index2; newTriangles[tindex+13]= index7; newTriangles[tindex+14]= index6;
			newTriangles[tindex+15] = index2; newTriangles[tindex+16]= index3; newTriangles[tindex+17]= index7;
		}
		Mesh mesh = new Mesh();
    	GetComponent<MeshFilter>().mesh = mesh;
    	mesh.vertices = newVertices;
    	mesh.uv = newUV;
    	mesh.triangles = newTriangles;
		mesh.normals = newNormals;
		GetComponent<MeshRenderer>().material = visualMaterial;
		
	}
	
	void GenerateFromHeights( float[] heights )
	{
		int numHeights = heights.Length;
		float min = heights[0];
		float max = heights[0];
		for(int i=1;i<numHeights;++i)
		{
			min = Mathf.Min( min, heights[i] );
			max = Mathf.Max( max, heights[i] );
		}
		int numVertsPerTri = 3;
		int numVertsPerHeight = 3;
		newVertices = new Vector3[numHeights*numVertsPerHeight];
		newUV = new Vector2[numHeights*numVertsPerHeight];
		float xstep = 0.1f;
		float ybase = min-30.0f;
		float zmin = -0.2f;
		float zmax = 0.2f;
		for(int i=0;i<numHeights;++i)
		{
			float xpos = i*xstep;
			int index = i*numVertsPerHeight;
			newVertices[index+0] = new Vector3( xpos, ybase, zmin );
			newVertices[index+1] = new Vector3( xpos, heights[i], zmin );
			newVertices[index+2] = new Vector3( xpos, heights[i], zmax );
			newUV[index+0] = new Vector2( 0.0f, 0.0f );
			newUV[index+1] = new Vector2( 0.0f, 1.0f );
			newUV[index+2] = new Vector2( 1.0f, 1.0f );
		}
		int numTrisPerHeight = 4;
		newTriangles = new int[numTrisPerHeight*(numHeights-1)*numVertsPerTri];
		for(int i=1;i<numHeights;++i)
		{
			int index0 = (i-1)*numVertsPerHeight;
			int index1 = index0+1;
			int index2 = index0+2;
			int index3 = i*numVertsPerHeight;
			int index4 = index3+1;
			int index5 = index3+2;
			int tindex = (i-1)*numTrisPerHeight*numVertsPerTri;
			newTriangles[tindex+0] = index0; newTriangles[tindex+1] = index1; newTriangles[tindex+2] = index4;
			newTriangles[tindex+3] = index0; newTriangles[tindex+4] = index4; newTriangles[tindex+5] = index3;
			newTriangles[tindex+6] = index1; newTriangles[tindex+7] = index2; newTriangles[tindex+8] = index5;
			newTriangles[tindex+9] = index1; newTriangles[tindex+10]= index5; newTriangles[tindex+11]= index4;
		}
		Mesh mesh = new Mesh();
    	GetComponent<MeshFilter>().mesh = mesh;
    	mesh.vertices = newVertices;
    	mesh.uv = newUV;
    	mesh.triangles = newTriangles;
		GetComponent<MeshRenderer>().material = visualMaterial;
	}
	
	float[] GenerateHeights()
	{
		float[] heights = new float[100];
		for(int i=0;i<100;++i)
		{
			heights[i] = (i*-0.02f) + Random.Range(-0.03f,0.0f);
		}
		return heights;
	}
}
