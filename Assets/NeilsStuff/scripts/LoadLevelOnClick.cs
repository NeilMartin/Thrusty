using UnityEngine;
using System.Collections;

public class LoadLevelOnClick : MonoBehaviour 
{
	private Rect groupRect;
	private Rect boxRect;
	private Rect[] buttons;
	public int numLevels = 3;

	void Start () 
	{
		int buttonBuffer = 10;
		int numButtons = numLevels+1;
		int buttonHeight = 30;
		int buttonWidth = 140;
		int groupNameHeight = 10;
		int height = groupNameHeight + buttonBuffer + numButtons * (buttonHeight+buttonBuffer);
		int width = buttonWidth + 2*buttonBuffer;
		
		buttons = new Rect[numButtons];
		for( int i=0; i< numButtons;++i )
		{
			buttons[i] = new Rect( buttonBuffer, groupNameHeight + buttonBuffer + i*(buttonHeight+buttonBuffer), buttonWidth, buttonHeight );
		}
		
		groupRect = new Rect( ( Screen.width - width) / 2, (Screen.height - height) / 2, width, height );
		boxRect = new Rect( 0,0, width, height );
	}
	
	void OnGUI()
	{
		GUI.BeginGroup (groupRect);
		// All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.

		// We'll make a box so you can see where the group is on-screen.
		GUI.Box (boxRect, "Pause Menu");
		for( int i=0;i<numLevels;++i )
		{
			if( GUI.Button (buttons[i], "Level " + (i+1)) )
			{
				gameObject.SendMessageUpwards( "ResumeGame" );
				Application.LoadLevel("level"+(i+1));
			}
		}
		if( GUI.Button (buttons[numLevels], "ESC - Resume") )
		{
			gameObject.SendMessageUpwards( "ResumeGame" );
		}

		// End the group we started above. This is very important to remember!
		GUI.EndGroup ();
	}
}
