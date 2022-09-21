using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRST
{
    // Public struct for 1D axis input.
    [System.Serializable]
    public class Input1DAxis
    {
        // The name of the 1D axis.
        [SerializeField]
        string m_Name;
        public string Name { get { return m_Name; } set { m_Name = value; } }

        // The press threshold for the 1D axis.
        [SerializeField]
        float m_Threshold = 0.1f;
        public float Threshold { get { return m_Threshold; } set { m_Threshold = value; } }

        // The value of the 1D axis.
        [SerializeField]
        float m_Axis;
        public float Axis
        {
            // Get the value of the 1D axis.
            get { return m_Axis; }

            // Set the value of the 1D axis.
            set
            {
                // Clamp the new value.
                float clampedValue = value;
                // Clamp lower bound.
                if (clampedValue < 0.0f) clampedValue = 0.0f;
                // Clamp upper bound.
                else if (clampedValue > 1.0f) clampedValue = 1.0f;
                // Set the clamped value.
                m_Axis = clampedValue;

                // If the value is less than the press threshold.
                if (clampedValue < Threshold)
                {
                    // Reset the button value.
                    m_Button = false;
                }
                // If the value is greater than the press threshold.
                else
                {
                    // Set the button value.
                    m_Button = true;
                }
            }
        }

        // The button state of the 1D axis.
        [SerializeField]
        bool m_Button;
        public bool Button
        {
            // Get the button state of the 1D axis.
            get { return m_Button; }

            // Set the button state of the 1D axis.
            set
            {
                // If the button is down but the axis is up.
                if (value && Axis < Threshold)
                {
                    // Set a random axis value.
                    Axis = Random.Range(Threshold + Mathf.Epsilon, 1.0f);
                }
                // If the button is up but the axis is down.
                else if (!value && Axis >= Threshold)
                {
                    // Set a random axis value. 
                    Axis = Random.Range(0.0f, Threshold - Mathf.Epsilon);
                }
                // Update the button value.
                m_Button = value;
            }
        }

        // Whether the button is included.
        [SerializeField]
        bool m_ButtonIncluded;
        public bool ButtonIncluded { get { return m_ButtonIncluded; } set { m_ButtonIncluded = value; } }

        // Function for changing the axis down.
        public void ChangeAxisDown(float change = 0.0f)
        {
            // Create a random change by default.
            if (change == 0.0f)
            {
                change = Random.Range(0.0f, 0.25f);
            }
            // Update the axis value.
            Axis += change;
        }

        // Function for changing the axis up.
        public void ChangeAxisUp(float change = 0.0f)
        {
            // Create a random change by default.
            if (change == 0.0f)
            {
                change = Random.Range(0.0f, 0.25f);
            }
            // Update the axis value.
            Axis -= change;
        }

        // Function for pressing the button.
        public void ButtonAxis(bool button = true, float axis = 0.0f)
        {
            // If there is a button change.
            if (button != Button)
            {
                // Update the button value.
                Button = button;
            }
            // If there is not a button change.
            else
            {
                // Update the axis value.
                Axis = axis;
            }
        }
    }

    // Public struct for 2D axis input.
    [System.Serializable]
    public class Input2DAxis
    {
        // The name of the 2D axis.
        [SerializeField]
        string m_Name;
        public string Name { get { return m_Name; } set { m_Name = value; } }

        // The value of the 2D axis.
        Vector2 m_Axis;
        public Vector2 Axis
        {
            // Get the value of the 2D axis.
            get { return m_Axis; }

            // Set the value of the 2D axis.
            set
            {
                // Clamp the incoming value.
                Vector2 clampedValue = value;

                // Clamp lower x bound.
                if (clampedValue.x < -1.0f) clampedValue.x = -1.0f;
                // Clamp upper x bound.
                else if (clampedValue.x > 1.0f) clampedValue.x = 1.0f;

                // Clamp lower y bound.
                if (clampedValue.y < -1.0f) clampedValue.y = -1.0f;
                // Clamp upper y bound.
                else if (clampedValue.y > 1.0f) clampedValue.y = 1.0f;

                // Set the value.
                m_Axis = clampedValue;
            }
        }

        // The touch state of the 2D axis.
        [SerializeField]
        bool m_Touch;
        public bool Touch
        {
            // Get the touch state of the 2D axis.
            get { return m_Touch; }

            // Set the touch state of the 2D axis.
            set
            {
                // Handling a touch.
                if (value)
                {
                    // Set the touch value.
                    m_Touch = true;
                    // Generate a random axis value, if it is default.
                    if (Axis == Vector2.zero)
                    {
                        Axis = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                    }
                }
                // Handling a release.
                else
                {
                    // Reset the touch value.
                    m_Touch = false;
                    // Reset the axis value.
                    Axis = Vector2.zero;
                    // Reset the click value.
                    m_Click = false;
                }
            }
        }

        // Whether the touch is included.
        [SerializeField]
        bool m_TouchIncluded;
        public bool TouchIncluded { get { return m_TouchIncluded; } set { m_TouchIncluded = value; } }

        // The click state of the 2D axis.
        [SerializeField]
        bool m_Click;
        public bool Click
        {
            // Get the click state of the 2D axis.
            get { return m_Click; }

            // Set the click state of the 2D axis.
            set
            {
                // Handling a click.
                if (value)
                {
                    // Set the click value.
                    m_Click = true;

                    // Set the touch value.
                    m_Touch = true;

                    // Generate a random axis value, if it is default.
                    if (Axis == Vector2.zero)
                    {
                        Axis = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                    }
                }
                // Handling a release.
                else
                {
                    // Reset the click value.
                    m_Click = false;

                    // If touch is not used.
                    if (!TouchIncluded)
                    {
                        // Reset the touch value.
                        m_Touch = false;
                        // Reset the axis vlaue.
                        Axis = Vector2.zero;
                    }
                }
            }
        }

        // Whether the click is included.
        [SerializeField]
        bool m_ClickIncluded;
        public bool ClickIncluded { get { return m_ClickIncluded; } set { m_ClickIncluded = value; } }

        // Function for touching the axis.
        public void TouchAxis(bool touch = true, Vector2 axis = new Vector2())
        {
            // Set the axis first.
            Axis = axis;
            // Then set the touch.
            Touch = touch;
        }

        // Function for touching the axis up.
        public void TouchAxisUp(bool touch = true)
        {
            // Set a random up axis value.
            Vector2 up = new Vector2(Random.Range(-0.25f, 0.25f), Random.Range(0.50f, 1.00f));
            TouchAxis(touch, up);
        }

        // Function for touching the axis down.
        public void TouchAxisDown(bool touch = true)
        {
            // Set a random down axis value.
            Vector2 down = new Vector2(Random.Range(-0.25f, 0.25f), Random.Range(-0.50f, -1.00f));
            TouchAxis(touch, down);
        }

        // Function for touching the axis left.
        public void TouchAxisLeft(bool touch = true)
        {
            // Set a random left axis value.
            Vector2 left = new Vector2(Random.Range(-0.50f, -1.00f), Random.Range(-0.25f, 0.25f));
            TouchAxis(touch, left);
        }

        // Function for touching the axis right.
        public void TouchAxisRight(bool touch = true)
        {
            // Set a random right axis value.
            Vector2 right = new Vector2(Random.Range(0.50f, 1.00f), Random.Range(-0.25f, 0.25f));
            TouchAxis(touch, right);
        }

        // Function for clicking the axis.
        public void ClickAxis(bool click = true, Vector2 axis = new Vector2())
        {
            // Set the axis first.
            Axis = axis;
            // Then set the click.
            Click = click;
        }
    }

    // Public struct for button input.
    [System.Serializable]
    public class InputButton
    {
        // The name of the button.
        [SerializeField]
        string m_Name;
        public string Name { get { return m_Name; } set { m_Name = value; } }

        // The button state of the button.
        [SerializeField]
        bool m_Button;
        public bool Button
        {
            // Get the button state of the button.
            get { return m_Button; }

            // Set the button state of the button.
            set
            {
                // If the touch is used.
                if (TouchIncluded)
                {
                    // If the value is set.
                    if (value)
                    {
                        // Set both the button and the touch to the new value.
                        m_Button = value;
                        m_Touch = value;
                    }
                    // Otherwise, only set the button.
                    else
                    {
                        m_Button = value;
                    }
                }
                // If the touch is not used.
                else
                {
                    // Set both the button and the touch to the new value.
                    m_Button = value;
                    m_Touch = value;
                }
            }
        }

        // The touch state of the button.
        [SerializeField]
        bool m_Touch;
        public bool Touch
        {
            // Get the touch state of the button.
            get { return m_Touch; }

            // Set the touch state of the button.
            set
            {
                // If the value is reset.
                if (!value)
                {
                    // Set both the button and the touch to the new value.
                    m_Button = value;
                    m_Touch = value;
                }
                // Otherwise, only set the touch.
                else
                {
                    m_Touch = value;
                }
            }
        }

        // Whether the touch is included.
        [SerializeField]
        bool m_TouchIncluded;
        public bool TouchIncluded { get { return m_TouchIncluded; } set { m_TouchIncluded = value; } }
    }
}