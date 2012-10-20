using UnityEngine;
using System.Collections;

public class CounterGUI : MonoBehaviour 
{
	public enum DisplayStyle
	{
		Amount,
		AmountAndMax,
		ReverseAmountAndMax
	}
	public string pretext = "Count :";
	public CountMe.CountType type = CountMe.CountType.None;
	public DisplayStyle style = DisplayStyle.Amount;
	private int mMaxAmount;
	
	// Use this for initialization
	void Start () 
	{
		mMaxAmount = 0;
	}
	
	void Update () 
	{
		int currAmount = GlobalCounter.GetCount(type);
		mMaxAmount = Mathf.Max( currAmount, mMaxAmount );
		
		switch(style)
		{
		case DisplayStyle.Amount:
			guiText.text = pretext + currAmount;	
			break;

		case DisplayStyle.AmountAndMax:
			guiText.text = pretext + currAmount +"/" + mMaxAmount;	
			break;
			
		case DisplayStyle.ReverseAmountAndMax:
			guiText.text = pretext + (mMaxAmount-currAmount) +"/" + mMaxAmount;	
			break;
			
		default:
			Debug.LogError("unhandled case");
			break;
		}
	}
}
