using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(BehaviourTree))]
public class BehaviourTreeEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		BehaviourTree tree = (BehaviourTree)target;
		tree.AddInspectorGUI();
		
		if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
	}
}
	