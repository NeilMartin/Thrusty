using UnityEditor;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class State : TreeNode
{
	public string mName;
	
	public State( string name )
	{
		mName = name;
	}
	
	public string GetName()
	{
		return mName;
	}

	public void SetName(string name)
	{
		mName = name;
	}
	
	public override void AddInspectorGUI()
	{
		EditorGUILayout.LabelField(mName);
	}

}
