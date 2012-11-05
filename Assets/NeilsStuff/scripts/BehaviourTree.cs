using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class BehaviourTree : MonoBehaviour 
{
	public GameObject states = null;
	
	private GameObject mCurrState;
	
	private GUIArray mNodeArray;
	private string mNewStateName;
	private bool mEditingNewStateString;
	
	private ActionTypes actionType = ActionTypes.None;
	
	public BehaviourTree()
	{
		//mNodes.Initialize();
		mEditingNewStateString = false;
	}
	
	public enum ActionTypes
	{
		None,
		Action,
	}
	
	public void Awake () 
	{
		mCurrState = null;
	}
	
	void Update () 
	{
		if(( null == mCurrState ) && ( null != states ))
		{
			Transform firstTransform = states.GetComponentInChildren<Transform>();
			if( null == firstTransform )
			{
				Debug.Log("BehaviourTree in " + name + " does not have a child Transform" );
			}
			else
			{
				mCurrState = firstTransform.gameObject;
				EndCurrentActions();
				PopulateActions( mCurrState );
			}
		}
		if( null != mCurrState )
		{
			UpdateActions();
		}
	}
	
	void EndCurrentActions()
	{
		
	}
	
	void PopulateActions( GameObject state )
	{
		Debug.Log( "newstate="+state.name );
	}
	
	void UpdateActions()
	{
		// do anything?
	}

	private void AddNewState( string newStateName )
	{
		//string statePrefabName = "/GameStuff/Prefabs/PlayerPrefab";
		//GameObject statePrefab = ObjectLibrary.GetComponent("State");
		State newState = (State)ScriptableObject.CreateInstance( "State" );
		newState.SetName(newStateName);
		if(null==mNodeArray)
		{
			Debug.Log("creating GUIArray" );
			mNodeArray = (GUIArray)ScriptableObject.CreateInstance( "GUIArray" );
		}
		if(null!=mNodeArray)
		{
			Debug.Log("GUIArray CREATED" );
		}
		mNodeArray.Add( newState );
	}

	public void AddInspectorGUI()
	{
		if(null != mNodeArray)
		{
			mNodeArray.AddInspectorGUI();
		}
		else
		{
			EditorGUILayout.LabelField("No states, please add one below");
		}
		if( null == mNewStateName )
		{
			mNewStateName = "";
		}
		EditorGUILayout.Separator();
		GUI.SetNextControlName("NewStateField");
		mNewStateName = EditorGUILayout.TextField("add state", mNewStateName);
		
		
		actionType = (ActionTypes)EditorGUILayout.EnumPopup( "new action", actionType ); 
/*		TreeNode newTreeNode = new TreeNode();
		newTreeNode = (TreeNode)EditorGUILayout.ObjectField( "new node", 
			newTreeNode, 
			typeof(TreeNode),
			true );*/
		if (Event.current.type == EventType.KeyUp)
	    {
	        if ((Event.current.keyCode == KeyCode.Return) 
				&& mEditingNewStateString
				&& (mNewStateName.Length > 0) )
	        {
				Debug.Log("adding new state " + mNewStateName );
				AddNewState(mNewStateName);
				EditorUtility.SetDirty(this);
	        }
	        mEditingNewStateString = (GUI.GetNameOfFocusedControl() == "NewStateField");
	    }
	}
}
