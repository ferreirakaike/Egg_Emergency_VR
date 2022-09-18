/*
*   Copyright (C) 2020 University of Central Florida, created by Dr. Ryan P. McMahan.
*
*   This program is free software: you can redistribute it and/or modify
*   it under the terms of the GNU General Public License as published by
*   the Free Software Foundation, either version 3 of the License, or
*   (at your option) any later version.
*
*   This program is distributed in the hope that it will be useful,
*   but WITHOUT ANY WARRANTY; without even the implied warranty of
*   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*   GNU General Public License for more details.
*
*   You should have received a copy of the GNU General Public License
*   along with this program.  If not, see <http://www.gnu.org/licenses/>.
*
*   Primary Author Contact:  Dr. Ryan P. McMahan <rpm@ucf.edu>
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XRST;

// Class for drawing and controlling an XRSTController component.
[CustomEditor(typeof(XRSTController))]
public class XRSTControllerEditor : Editor
{
    // Property to get a standard vertical space for a single field.
    float VerticalSpace { get { return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; } }

    // Function for drawing the inspector editor.
    public override void OnInspectorGUI()
    {
        // Start with the conventional disabled script reference.
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((XRSTController)target), typeof(XRSTController), false);
        GUI.enabled = true;

        // Disable the GUI if the application is not playing.
        GUI.enabled = Application.isPlaying;

        // Keep track of the position.
        Rect position = GUILayoutUtility.GetLastRect();
        position.y += VerticalSpace + EditorGUIUtility.standardVerticalSpacing;

        // Draw the Vive input configuration. 
        // Planning to expand this in the future to support other configurations.
        DrawViveConfiguration(ref position);

        // Reenable the GUI.
        GUI.enabled = true;

        // Apply the modified properties.
        serializedObject.ApplyModifiedProperties();
    }

    // Function for drawing the Vive configuration.
    void DrawViveConfiguration(ref Rect position)
    {
        // Draw the primary 2D axis.
        DrawHeaderField("Trackpad", ref position);
        DrawAxis2DField(ref ((XRSTController)target).m_Primary2DAxis, ref position);
        // Draw the trigger 1D axis.
        DrawHeaderField("Trigger", ref position);
        DrawAxis1DField(ref ((XRSTController)target).m_Trigger, ref position);
        // Draw the grip 1D axis.
        DrawHeaderField("Grip", ref position);
        DrawAxis1DField(ref ((XRSTController)target).m_Grip, ref position);
        // Draw the primary button.
        DrawHeaderField("Menu", ref position);
        DrawButtonField(ref ((XRSTController)target).m_PrimaryButton, ref position);
    }

    // Draws a 1D axis field.
    void DrawAxis1DField(ref Input1DAxis axis1D, ref Rect position)
    {
        // Prepare the labels and tooltips.
        GUIContent axisLabel = new GUIContent(axis1D.Name, "The 1D axis used to simulate the XR controller's " + axis1D.Name + ".");
        GUIContent buttonLabel = new GUIContent(axis1D.Name + " Button", "The binary used to simulate the XR controller's " + axis1D.Name + " Button.");

        // Create the slider for the 1D axis value.
        float axisValue = EditorGUILayout.Slider(axisLabel, axis1D.Axis, 0.0f, 1.0f);
        position.y += VerticalSpace;

        // If the button is used.
        if (axis1D.ButtonIncluded)
        {
            // Create the checkbox for the button state.
            bool buttonValue = EditorGUILayout.Toggle(buttonLabel, axis1D.Button);
            position.y += VerticalSpace;

            // Update the button and axis.
            axis1D.ButtonAxis(buttonValue, axisValue);
        }
        // If the button is not included.
        else
        {
            // Update the axis.
            axis1D.Axis = axisValue;
        }
    }

    // Draws a 2D axis field.
    void DrawAxis2DField(ref Input2DAxis axis2D, ref Rect position)
    {
        // Prepare the labels and tooltips.
        GUIContent axisLabel = new GUIContent(axis2D.Name, "The 2D axis used to simulate the XR controller's " + axis2D.Name + ".");
        GUIContent touchLabel = new GUIContent(axis2D.Name + " Touch", "The binary used to simulate touching the XR controller's " + axis2D.Name + ".");
        GUIContent clickLabel = new GUIContent(axis2D.Name + " Click", "The binary used to simulate clicking the XR controller's " + axis2D.Name + ".");

        // Size ratios.
        float size = 3.6f;
        float touchSize = 0.25f;
        float clickSize = 0.5f;

        // Get the current editor GUI label width.
        float labelWidth = EditorGUIUtility.labelWidth;

        // Set a width for the individual fields.
        float fieldWidth = labelWidth + 70.0f;

        // Calculate the space and position for the 2D axis.
        float axisSpace = EditorGUIUtility.currentViewWidth - fieldWidth - (VerticalSpace * size);
        float axisPosition = fieldWidth + (axisSpace / 2.0f);

        // Create a rectangle for the 2D axis ranges.
        Rect axis2DRange = new Rect(position);
        axis2DRange.x = axisPosition;
        axis2DRange.width = VerticalSpace * size;
        axis2DRange.height = VerticalSpace * size;
        Color axis2DRangeColor;

        // Create a rectangle for the 2D axis value.
        Rect axis2DValue = new Rect(position);
        axis2DValue.x = axis2DRange.center.x + (axis2D.Axis.x * (axis2DRange.width / 2.0f));
        axis2DValue.y = axis2DRange.center.y - (axis2D.Axis.y * (axis2DRange.height / 2.0f));
        Color axis2DValueColor;

        // Create a large green rectangle on a dark gray background if the axis is clicked.
        if (axis2D.Click)
        {
            axis2DValue.x -= (EditorGUIUtility.standardVerticalSpacing * clickSize * size);
            axis2DValue.y -= (EditorGUIUtility.standardVerticalSpacing * clickSize * size);
            axis2DValue.width = EditorGUIUtility.standardVerticalSpacing * clickSize * size * 2.0f;
            axis2DValue.height = EditorGUIUtility.standardVerticalSpacing * clickSize * size * 2.0f;
            axis2DValueColor = Color.green;
            axis2DRangeColor = Color.Lerp(Color.gray, Color.black, 0.25f);
        }
        // Create a small yellow rectangle on a light gray background if the axis is touched.
        else if (axis2D.Touch)
        {
            axis2DValue.x -= (EditorGUIUtility.standardVerticalSpacing * touchSize * size);
            axis2DValue.y -= (EditorGUIUtility.standardVerticalSpacing * touchSize * size);
            axis2DValue.width = EditorGUIUtility.standardVerticalSpacing * touchSize * size * 2.0f;
            axis2DValue.height = EditorGUIUtility.standardVerticalSpacing * touchSize * size * 2.0f;
            axis2DValueColor = Color.yellow;
            axis2DRangeColor = Color.gray;
        }
        // Otherwise, don't show the value rectangle on the background.
        else
        {
            axis2DValue.width = 0.0f;
            axis2DValue.height = 0.0f;
            axis2DValueColor = Color.gray;
            axis2DRangeColor = Color.gray;
        }

        // Draw the 2D axis.
        Handles.BeginGUI();
        Handles.DrawSolidRectangleWithOutline(axis2DRange, axis2DRangeColor, Color.black);
        Handles.DrawSolidRectangleWithOutline(axis2DValue, axis2DValueColor, axis2DValueColor);
        Handles.EndGUI();

        // Create a horizontal group for the axis label and axis x value.
        EditorGUILayout.BeginHorizontal();
        // Create the axis label.
        EditorGUILayout.LabelField(axisLabel, GUILayout.Width(labelWidth));
        // Create the float field for the 2D axis X value.
        EditorGUIUtility.labelWidth = 10;
        float axisXValue = EditorGUILayout.FloatField("X", float.Parse(axis2D.Axis.x.ToString("F4")));
        EditorGUIUtility.labelWidth = labelWidth;
        // Create a flexible space to draw the 2D axis in.
        GUILayout.FlexibleSpace();
        // End the horizontal group and update the position.
        EditorGUILayout.EndHorizontal();
        position.y += VerticalSpace;

        // Set the lower limit for the axis x value.
        if (axisXValue < -1.0f)
        {
            axisXValue = -1.0f;
        }
        // Set the upper limit for the axis x value.
        if (axisXValue > 1.0f)
        {
            axisXValue = 1.0f;
        }

        // Create a horizontal group for the axis y value.
        EditorGUILayout.BeginHorizontal();
        // Create an empty label.
        EditorGUILayout.LabelField("", GUILayout.Width(labelWidth));
        // Create the float field for the 2D axis y value.
        EditorGUIUtility.labelWidth = 10;
        float axisYValue = EditorGUILayout.FloatField("Y", float.Parse(axis2D.Axis.y.ToString("F4")));
        EditorGUIUtility.labelWidth = labelWidth;
        // Create a flexible space to draw the 2D axis in.
        GUILayout.FlexibleSpace();
        // End the horizontal group and update the position.
        EditorGUILayout.EndHorizontal();
        position.y += VerticalSpace;

        // Set the lower limit for the axis y value.
        if (axisYValue < -1.0f)
        {
            axisYValue = -1.0f;
        }
        // Set the upper limit for the axis y value.
        if (axisYValue > 1.0f)
        {
            axisYValue = 1.0f;
        }

        // Update the 2D axis if the current event is a mouse down or drag event.
        if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
        {
            // Get the current mouse position.
            Vector2 mouse2D = Event.current.mousePosition;

            // If the mouse is within the vertical 2D axis range.
            if (axis2DRange.y <= mouse2D.y && mouse2D.y <= axis2DRange.y + axis2DRange.height)
            {
                // If the mouse is within the horizontal 2D axis range.
                if (axis2DRange.x <= mouse2D.x && mouse2D.x <= axis2DRange.x + axis2DRange.width)
                {
                    // If the event is mouse down.
                    if (Event.current.type == EventType.MouseDown)
                    {
                        // Reset the click.
                        axis2D.ClickAxis(false);
                        // At least touching the axis.
                        axis2D.TouchAxis();
                    }
                    // If the 2D axis is touched.
                    if (axis2D.Touch)
                    {
                        // Update the 2D axis value.
                        Vector2 mouse2DValue = new Vector2();
                        mouse2DValue.x = (mouse2D.x - axis2DRange.center.x) / (axis2DRange.width / 2.0f);
                        mouse2DValue.y = (axis2DRange.center.y - mouse2D.y) / (axis2DRange.height / 2.0f);
                        axis2D.TouchAxis(true, mouse2DValue);

                        // If the mouse is inside the 2D axis value.
                        if ((axis2DValue.x <= mouse2D.x && mouse2D.x <= axis2DValue.x + axis2DValue.width) &&
                            (axis2DValue.y <= mouse2D.y && mouse2D.y <= axis2DValue.y + axis2DValue.height))
                        {
                            // If the event is mouse down.
                            if (Event.current.type == EventType.MouseDown)
                            {
                                // Clicking the axis.
                                axis2D.ClickAxis(true, mouse2DValue);
                            }
                        }
                    }
                }
                // If the mouse is outside the horizontal 2D axis range and after the individual fields.
                else if (fieldWidth <= mouse2D.x)
                {
                    // If the event is mouse down.
                    if (Event.current.type == EventType.MouseDown)
                    {
                        // No longer touching the axis.
                        axis2D.TouchAxis(false);
                    }
                }
            }
        }
        else
        {
            // If touching.
            if (axis2D.Touch)
            {
                // Update the 2D axis.
                axis2D.TouchAxis(true, new Vector2(axisXValue, axisYValue));
            }
        }

        // If the touch is used.
        if (axis2D.TouchIncluded)
        {
            // Create the checkbox for the touch state.
            bool touchValue = EditorGUILayout.Toggle(touchLabel, axis2D.Touch, GUILayout.MaxWidth(fieldWidth));
            position.y += VerticalSpace;

            // Check for a change in the touch value.
            if (touchValue != axis2D.Touch)
            {
                // Update touching the axis.
                axis2D.TouchAxis(touchValue, axis2D.Axis);
            }
        }

        // If the click is used.
        if (axis2D.ClickIncluded)
        {
            // Create the checkbox for the click state.
            bool clickValue = EditorGUILayout.Toggle(clickLabel, axis2D.Click, GUILayout.MaxWidth(fieldWidth));
            position.y += VerticalSpace;

            // Check for a change in the click value.
            if (clickValue != axis2D.Click)
            {
                // Update clicking the axis.
                axis2D.ClickAxis(clickValue, axis2D.Axis);
            }
        }
    }

    // Draws a touch button field.
    void DrawButtonField(ref InputButton button, ref Rect position)
    {
        // Prepare the labels and tooltips.
        GUIContent buttonLabel = new GUIContent(button.Name + " Button", "The binary used to simulate the XR controller's " + button.Name + " Button.");
        GUIContent touchLabel = new GUIContent(button.Name + " Touch", "The binary used to simulate the XR controller's " + button.Name + " Touch.");

        // Create the checkbox for the button state.
        bool buttonValue = EditorGUILayout.Toggle(buttonLabel, button.Button);
        position.y += VerticalSpace;

        // If the touch is used.
        if (button.TouchIncluded)
        {
            // Create the checkbox for the touch state.
            bool touchValue = EditorGUILayout.Toggle(touchLabel, button.Touch);
            position.y += VerticalSpace;

            // Update the touch.
            button.Touch = touchValue;
        }

        // Update the button.
        button.Button = buttonValue;
    }

    // Draws a custom header field.
    void DrawHeaderField(string header, ref Rect position)
    {
        // Create blank label field for spacing.
        EditorGUILayout.Space();
        position.y += EditorGUIUtility.standardVerticalSpacing;

        // Create the label field for the header.
        EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
        position.y += VerticalSpace;
    }
}
