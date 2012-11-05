using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeNode : ScriptableObject
{

	public virtual void AddInspectorGUI()
	{
		Debug.LogError("Please implement " + GetType().Name + "::AddInspectorGUI" );
	}
}
