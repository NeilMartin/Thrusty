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
	}
	
	public void Initialize()
	{
		Debug.Log("GUIArray INIT" );
		mNodes = new List<TreeNode>();
	}
	
	public void AddInspectorGUI()
	{
		if(null == mNodes)
		{
			EditorGUILayout.LabelField("size = 0");
		}
		else
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
				DestroyImmediate(deleteMe);
			}
		}
	}
	
	public void Add( TreeNode node )
	{
		if(null==mNodes)
		{
			Initialize();
		}
		mNodes.Add( node );
		Debug.Log("Adding Node, count = " + mNodes.Count );
	}

}