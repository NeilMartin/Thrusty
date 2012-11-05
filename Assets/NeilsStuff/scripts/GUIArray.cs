using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GUIArray: ScriptableObject
{
	private List<TreeNode> mNodes;
	
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
		foreach( TreeNode node in mNodes )
		{
			node.AddInspectorGUI();
		}
	}
	
	public void Add( TreeNode node )
	{
		mNodes.Add( node );
		Debug.Log("Adding Node, count = " + mNodes.Count );
	}

}