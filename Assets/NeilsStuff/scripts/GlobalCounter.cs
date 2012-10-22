using UnityEngine;
using System.Collections;

public class GlobalCounter 
{
	private int[] mCounters;

	protected GlobalCounter()
	{
		mCounters = new int[(int)CountMe.CountType.NumTypes];
		for( int i=0; i<mCounters.Length; ++i )
		{
			mCounters[i] = 0;
		}
	}

	private sealed class SingletonCreator
	{
	    private static readonly GlobalCounter instance = new GlobalCounter();
	    public static GlobalCounter Instance { get { return instance; } }
	}

	public static GlobalCounter Instance { get { return SingletonCreator.Instance; } }
	
	
	public static void Increment( CountMe.CountType type, int incValue )
	{
		int iType = (int)type;
		if((iType>=0) && (iType<Instance.mCounters.Length))
		{
			Instance.mCounters[iType] += incValue;
			if( Instance.mCounters[iType] < 0 )
			{
				Instance.mCounters[iType] = 0;
			}
		}
		else
		{
			Debug.LogError("Type "+iType+" out of range");
		}
	}
	
	public static void Increment( CountMe.CountType type )
	{
		Increment( type, 1 );
	}

	public static void Decrement( CountMe.CountType type )
	{
		Increment( type, -1 );
	}

	public static int GetCount( CountMe.CountType type )
	{
		int iType = (int)type;
		int count = 0;
		if((iType>=0) && (iType<Instance.mCounters.Length))
		{
			count = Instance.mCounters[iType];
		}
		else
		{
			Debug.LogError("Type "+iType+" out of range");
		}
		return count;
	}
	
	public static void ResetCounters()
	{
		for( int i=0; i<Instance.mCounters.Length; ++i )
		{
			Instance.mCounters[i] = 0;
		}
	}

}
