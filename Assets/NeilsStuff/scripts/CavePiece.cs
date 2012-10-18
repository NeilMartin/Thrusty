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
	private int         mSeed;
	
	public void SetType( int type, int seed )
	{
		mType = type;
		mSeed = seed;
	}
	
	// Use this for initialization
	void Start () 
	{
		gameObject.AddComponent("MeshFilter");
	    gameObject.AddComponent("MeshRenderer");
		GenerateFromType( mType, mSeed );
		if( physicMaterial != null )
		{
			gameObject.AddComponent("MeshCollider");
			MeshCollider mc = GetComponent<MeshCollider>();
			mc.material = physicMaterial;
		}
	}
	
	void GenerateFromType( int type, int seed )
	{
		float connectorScale = Mathf.Sqrt(2.0f);
		float diagonalVariance = 1.5f;
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
		if(type<256)
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
			Random.seed = seed;
			bool bDownClear = ((type&(1<<0))==0);
			bool bUpClear = ((type&(1<<2))==0);
			bool bLeftClear = ((type&(1<<3))==0);
			bool bRightClear = ((type&(1<<1))==0);
			bool bDownRightClear = ((type&(1<<4))==0); // good
			bool bDownLeftClear = ((type&(1<<7))==0);
			bool bUpLeftClear = ((type&(1<<6))==0);
			bool bUpRightClear = ((type&(1<<5))==0);
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
			if((bUpClear)&&(bRightClear))
			{
				if(bUpRightClear)
				{
					outerRing[2] = maxRing[2] * Random.Range( fmin, fmax ) * 0.7f;
				}
				else
				{
					outerRing[0] = maxRing[0] * Random.Range( fmin, fmax ) * diagonalVariance;
					outerRing[1] = maxRing[1] * connectorScale;
					outerRing[2] = maxRing[2];
					outerRing[3] = maxRing[3] * connectorScale;
					outerRing[4] = maxRing[4] * Random.Range( fmin, fmax ) * diagonalVariance;
				}
			}
			if((bDownClear)&&(bRightClear))
			{
				if(bDownRightClear)
				{
					outerRing[6] = maxRing[6] * Random.Range( fmin, fmax ) * 0.7f;
				}
				else
				{
					outerRing[4] = maxRing[4] * Random.Range( fmin, fmax ) * diagonalVariance;
					outerRing[5] = maxRing[5] * connectorScale;
					outerRing[6] = maxRing[6];
					outerRing[7] = maxRing[7] * connectorScale;
					outerRing[8] = maxRing[8] * Random.Range( fmin, fmax ) * diagonalVariance;
				}
			}
			if((bDownClear)&&(bLeftClear))
			{
				if(bDownLeftClear)
				{
					outerRing[10] = maxRing[10] * Random.Range( fmin, fmax ) * 0.7f;
				}
				else
				{
					outerRing[8] = maxRing[8] * Random.Range( fmin, fmax ) * diagonalVariance;
					outerRing[9] = maxRing[9] * connectorScale;
					outerRing[10] = maxRing[10];
					outerRing[11] = maxRing[11] * connectorScale;
					outerRing[12] = maxRing[12] * Random.Range( fmin, fmax ) * diagonalVariance;
				}
			}
			if((bUpClear)&&(bLeftClear))
			{
				if(bUpLeftClear)
				{
					outerRing[14] = maxRing[14] * Random.Range( fmin, fmax ) * 0.7f;
				}
				else
				{
					outerRing[12] = maxRing[12] * Random.Range( fmin, fmax ) * diagonalVariance;
					outerRing[13] = maxRing[13] * connectorScale;
					outerRing[14] = maxRing[14];
					outerRing[15] = maxRing[15] * connectorScale;
					outerRing[0] = maxRing[0] * Random.Range( fmin, fmax ) * diagonalVariance;
				}
			}
			GenerateFromRings( innerRing, outerRing, size );
		}
			

	}
	
	void GenerateFromRings( float[] innerRing, float[] outerRing, float size )
	{
		int numLengths = innerRing.Length;
		int numVertsPerLength = 6;
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
			newVertices[index+2] = vec*innerRing[i]*size;
			newVertices[index+3] = vec*outerRing[i]*size;
			newVertices[index+4] = vec*outerRing[i]*size;
			newVertices[index+5] = vec*outerRing[i]*size;
			newVertices[index+1].z -= size*zscale;
			newVertices[index+2].z -= size*zscale;
			newVertices[index+3].z -= size*zscale;
			newVertices[index+4].z -= size*zscale;
			newVertices[index+0].z += size*zscale;
			newVertices[index+5].z += size*zscale;
			newUV[index+0] = new Vector2( 0.0f, 0.0f );
			newUV[index+1] = new Vector2( 0.0f, 1.0f );
			newUV[index+2] = new Vector2( 0.0f, 1.0f );
			newUV[index+3] = new Vector2( 1.0f, 1.0f );
			newUV[index+4] = new Vector2( 1.0f, 1.0f );
			newUV[index+5] = new Vector2( 1.0f, 0.0f );
			newNormals[index+0] = -vec;
			newNormals[index+1] = -vec;
			newNormals[index+2] = new Vector3(0.0f,0.0f,-1.0f);
			newNormals[index+3] = newNormals[index+2];
			newNormals[index+4] = vec;
			newNormals[index+5] = vec;
			//newNormals[index+1].z = -1.0f;
			//newNormals[index+2].z = -1.0f;
			//newNormals[index+1].Normalize();
			//newNormals[index+2].Normalize();
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
			int index4 = index0+4;
			int index5 = index0+5;
			int index6 = ((i+1)%numLengths)*numVertsPerLength;
			int index7 = index6+1;
			int index8 = index6+2;
			int index9 = index6+3;
			int index10= index6+4;
			int index11= index6+5;
			int tindex = i*numTrisPerLength*numVertsPerTri;
			newTriangles[tindex+0] = index0; newTriangles[tindex+1] = index5; newTriangles[tindex+2] = index7;
			newTriangles[tindex+3] = index0; newTriangles[tindex+4] = index7; newTriangles[tindex+5] = index6;
			newTriangles[tindex+6] = index2; newTriangles[tindex+7] = index3; newTriangles[tindex+8] = index9;
			newTriangles[tindex+9] = index2; newTriangles[tindex+10]= index9; newTriangles[tindex+11]= index8;
			newTriangles[tindex+12] = index4; newTriangles[tindex+13]= index5; newTriangles[tindex+14]= index11;
			newTriangles[tindex+15] = index4; newTriangles[tindex+16]= index11; newTriangles[tindex+17]= index10;
		}
		Mesh mesh = new Mesh();
    	GetComponent<MeshFilter>().mesh = mesh;
    	mesh.vertices = newVertices;
    	mesh.uv = newUV;
    	mesh.triangles = newTriangles;
		mesh.normals = newNormals;
		GetComponent<MeshRenderer>().material = visualMaterial;
		
	}
	
}
