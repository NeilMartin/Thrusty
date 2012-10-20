using UnityEngine;
using System.Collections;

public class CounterGUI : MonoBehaviour 
{
	public string pretext = "Count :";
	public CountMe.CountType type = CountMe.CountType.None;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	void Update () 
	{
		guiText.text = pretext+GlobalCounter.GetCount(type);
	}
}
