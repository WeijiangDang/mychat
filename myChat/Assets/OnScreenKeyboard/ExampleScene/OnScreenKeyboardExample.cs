using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnScreenKeyboardExample : MonoBehaviour 
{
	private OnScreenKeyboard osk;
	
	private string editableString = "";
	public Text outputTextField;

	void Awake() 
    {
		osk = FindObjectOfType(typeof(OnScreenKeyboard)) as OnScreenKeyboard;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
		// You can use input from the OSK just by asking for the most recent 
		// pressed key, which will be returned to you as a string, or null if 
		// no key has been pressed since you last checked. Note that if more 
		// than one key has been pressed you will only be given the most recent.
		string keyPressed = osk.GetKeyPressed();
		if (keyPressed != "")
		{
			// Take different action depending on what key was pressed
			if (keyPressed == "Backspace" || keyPressed == "<<") 
			{
				// Remove a character
				if (editableString.Length > 0)
					editableString = editableString.Substring (0, editableString.Length - 1);
			} 
			else if (keyPressed == "Space") 
			{
				// Add a space
				editableString += " ";	
			} 
			else if (keyPressed == "Enter" || keyPressed == "Done") 
			{
				// Change screens, or do whatever you want to 
				// do when your user has finished typing :-)
				if (outputTextField.text == "") {
					outputTextField.text = "Me: " + editableString;
				} else {
					outputTextField.text += "\n";
					outputTextField.text += ("Me: " + editableString);
				}
				editableString = "";
			} 
			//@wejiand reset btn & clear btn
			else if (keyPressed == "Reset") 
			{
				editableString = "";
			}
			else if (keyPressed == "Clear") 
			{
				outputTextField.text = "";
			}
			else if (keyPressed == "Caps")
			{
				// Toggle the capslock state yourself
				osk.SetShiftState(osk.GetShiftState() == ShiftState.CapsLock ? ShiftState.Off : ShiftState.CapsLock);
			}
			else if (keyPressed == "Shift")
			{
				// Toggle shift state ourselves
				osk.SetShiftState(osk.GetShiftState() == ShiftState.Shift ? ShiftState.Off : ShiftState.Shift);
			}
			else
			{
				// Add a letter to the existing string
				editableString += keyPressed;	
			}
		}
	}

    void OnGUI()
    {
		// This displays the string. 
		// Note that it can also be edited here through Unity GUI as per usual.
		editableString = GUI.TextField(new Rect(Screen.width/4, Screen.height/2, Screen.width/2, 30), editableString);
		//outputTextField = GUI.TextField(new Rect(Screen.width/4, Screen.height, Screen.width/2, 30), editableString);
    }
}
