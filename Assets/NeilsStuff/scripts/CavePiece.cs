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
		switch(type)
		{
		case 0:
			break;

		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
			break;
			
		case 16:
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
		case 22:
		case 23:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
		case 30:
		case 31:
			{
				float[] innerRing = { 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f };
				float[] outerRing = { 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f };
				GenerateFromRings( innerRing, outerRing, size );
			}
			break;			
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
