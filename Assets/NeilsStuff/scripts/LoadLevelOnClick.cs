using UnityEngine;
using System.Collections;

public class LoadLevelOnClick : MonoBehaviour 
{
	private Rect groupRect;
	private Rect boxRect;
	private Rect[] buttons;
	public int numLevels = 3;
	private Rect[] helpBoxes;
	private Rect helpGroup;
	private Rect helpRect;

	void Start () 
	{
		int buttonBuffer = 10;
		int numButtons = numLevels+1;
		int buttonHeight = 30;
		int buttonWidth = 140;
		int groupNameHeight = 20;
		int height = groupNameHeight + buttonBuffer + numButtons * (buttonHeight+buttonBuffer);
		int width = buttonWidth + 2*buttonBuffer;
		
		buttons = new Rect[numButtons];
		for( int i=0; i< numButtons;++i )
		{
			buttons[i] = new Rect( buttonBuffer, groupNameHeight + buttonBuffer + i*(buttonHeight+buttonBuffer), buttonWidth, buttonHeight );
		}
		
		groupRect = new Rect( ( Screen.width - width) / 2, (Screen.height - height) / 2, width, height );
		boxRect = new Rect( 0,0, width, height );
		
		
		int helpLineWidth = 140;
		int numHelpLines = 5;
		int helpWidth = helpLineWidth + 2*buttonBuffer;
		int helpLineHeight = 12;
		int helpHeight = groupNameHeight + buttonBuffer+(helpLineHeight+buttonBuffer)*numHelpLines;
		
		helpGroup = new Rect( ( Screen.width - helpWidth) / 2, ((Screen.height + height) / 2)+buttonBuffer, helpWidth, helpHeight );
		helpRect = new Rect( 0,0, helpWidth, helpHeight );
		
		helpBoxes = new Rect[numHelpLines];
		for( int i=0; i< numHelpLines;++i )
		{
			helpBoxes[i] = new Rect( buttonBuffer, groupNameHeight + buttonBuffer + i*(helpLineHeight+buttonBuffer), helpLineWidth, helpLineHeight );
		}
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
				GlobalCounter.ResetCounters();
				Application.LoadLevel("level"+(i+1));
			}
		}
		if( GUI.Button (buttons[numLevels], "ESC - Resume") )
		{
			gameObject.SendMessageUpwards( "ResumeGame" );
		}

		// End the group we started above. This is very important to remember!
		GUI.EndGroup ();

		GUI.BeginGroup (helpGroup);
		// All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.
		GUIStyle center = new GUIStyle();
		center.alignment = TextAnchor.MiddleCenter;
		center.normal.textColor = new Color(1.0f, 1.0f, 1.0f);
		GUI.Label( helpBoxes[0], "Up - Thrust", center );
		GUI.Label( helpBoxes[1], "Down - Shield", center );
		GUI.Label( helpBoxes[2], "Left - Rotate Left", center );
		GUI.Label( helpBoxes[3], "Right - Rotate Right", center );
		GUI.Label( helpBoxes[4], "Space - Shoot", center );
		// We'll make a box so you can see where the group is on-screen.
		GUI.Box (helpRect, "Controls");

		// End the group we started above. This is very important to remember!
		GUI.EndGroup ();

	}
}
