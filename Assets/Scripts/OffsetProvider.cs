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
using UnityEngine.XR.Interaction.Toolkit;

namespace UnityEngine.XR.Interaction.Toolkit
{
    // The Offset Provider works with the XR Interactor to provide a dynamic offset for interactors.
    [AddComponentMenu("XRST/Interaction/OffsetProvider")]
    public class OffsetProvider : MonoBehaviour
    {
        // The XR Interactor to provide a dynamic offset for.
        [SerializeField]
        [Tooltip("The XR Interactor to provide a dynamic offset for.")]
        XRBaseInteractor m_Interactor;
        public XRBaseInteractor Interactor { get { return m_Interactor; } set { m_Interactor = value; } }

        // The default time it takes for interactables to ease into their attachment. 
        [SerializeField]
        [Tooltip("The default time it takes for interactables to ease into their attachment.")]
        float m_AttachEaseInTime = 0.0f;
        public float AttachEaseInTime { get { return m_AttachEaseInTime; } set { m_AttachEaseInTime = value; } }

        // The default time it takes for interactables to ease into their attachment. 
        [SerializeField]
        [Tooltip("The default type of movement applied to the interactable.")]
        XRBaseInteractable.MovementType m_MovementType = XRBaseInteractable.MovementType.Instantaneous;
        public XRBaseInteractable.MovementType MovementType { get { return m_MovementType; } set { m_MovementType = value; } }

        // Whether the listeners have been added.
        bool listenersAdded;

        // Listener called by the interactor's OnSelectEnter interactor event.
        public void SetOffset(XRBaseInteractable interactable)
        {
            // If a local interactor exists.
            if (Interactor != null)
            {
                // Treat the base interactable as a grab interactable.
                XRGrabInteractable grabInteractable = interactable as XRGrabInteractable;
                // If it is a grab interactable.
                if (grabInteractable != null)
                {
                    // Set the grab interactable's attach transform to the interactor's transform (i.e., the offset).
                    grabInteractable.attachTransform = Interactor.transform;
                    // Set the grab interactable's ease in time. 
                    grabInteractable.attachEaseInTime = AttachEaseInTime;
                    // Set the grab interactable's movement type.
                    grabInteractable.movementType = MovementType;
                }
            }
        }

        // Listener called by the interactor's OnSelectExit interactor event.
        public void ResetOffset(XRBaseInteractable interactable)
        {
            // If a local interactor exists.
            if (Interactor != null)
            {
                // Treat the base interactable as a grab interactable.
                XRGrabInteractable grabInteractable = interactable as XRGrabInteractable;
                // If it is a grab interactable.
                if (grabInteractable != null)
                {
                    // Set the grab interactable's attach transform to null.
                    grabInteractable.attachTransform = null;
                }
            }
        }

        // Reset function for initializing the offset provider.
        void Reset()
        {
            // Attempt to fetch a local interactor.
            m_Interactor = GetComponent<XRBaseInteractor>();

            // Did not find a local interactor.
            if (m_Interactor == null)
            {
                // Warn the developer.
                Debug.LogWarning("[" + gameObject.name + "][OffsetProvider]: Did not find a local XR Interactor attached to the same game object.");

                // Attepmt to fetch any interactor.
                m_Interactor = FindObjectOfType<XRBaseInteractor>();

                // Did not find one.
                if (m_Interactor == null)
                {
                    Debug.LogWarning("[" + gameObject.name + "][OffsetProvider]: Did not find an XR Interactor in the scene.");
                }
                // Found one.
                else
                {
                    Debug.LogWarning("[" + gameObject.name + "][OffsetProvider]: Found an XR Interactor attached to " + m_Interactor.gameObject.name + ".");
                }
            }
        }

        // This function is called when the behaviour becomes disabled.
        void OnDisable()
        {
            // Attempt to remove the ResetOffset and SetOffset listeners from the events of the interactor.
            if (m_Interactor != null && listenersAdded)
            {
                // Remove the SetOffset function as a listener.
                m_Interactor.onSelectEnter.RemoveListener(SetOffset);
                // Remove the ResetOffset function as a listener.
                m_Interactor.onSelectExit.RemoveListener(ResetOffset);
                // Keep track of removing the listeners.
                listenersAdded = false;
            }
        }

        // This function is called when the object becomes enabled and active.
        void OnEnable()
        {
            // Attempt to add the ResetOffset and SetOffset listeners to the events of the interactor.
            if (m_Interactor != null && !listenersAdded)
            {
                // Add the SetOffset function as a listener.
                m_Interactor.onSelectEnter.AddListener(SetOffset);
                // Add the ResetOffset function as a listener.
                m_Interactor.onSelectExit.AddListener(ResetOffset);
                // Keep track of adding the listeners.
                listenersAdded = true;
            }
        }
    }
}
