using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class GUIArray: ScriptableObject
{
	public List<TreeNode> mNodes;
	
	public GUIArray()
	{
		Debug.Log("GUIArray CTOR" );
		Initialize();
	}
	
	public void Initialize()
	{
		mNodes = new List<TreeNode>();
	}
	
	public void AddInspectorGUI()
	{
		EditorGUILayout.LabelField("size = " + mNodes.Count );
		TreeNode deleteMe = null;
		foreach( TreeNode node in mNodes )
		{
			EditorGUILayout.BeginHorizontal();
			Rect r = EditorGUILayout.BeginHorizontal ("Button",  GUILayout.MaxWidth(32));
            if (GUI.Button (r, GUIContent.none ))
			{
				deleteMe = node;
			}
			GUILayout.Label ("del");  
        	EditorGUILayout.EndHorizontal ();
			node.AddInspectorGUI();
			EditorGUILayout.EndHorizontal ();
		}
		if( null != deleteMe )
		{
			mNodes.Remove(deleteMe);
		}
	}
	
	public void Add( TreeNode node )
	{
		mNodes.Add( node );
		Debug.Log("Adding Node, count = " + mNodes.Count );
	}

}