using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class FixDropdownOcclusion : TMP_Dropdown
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (GetComponent<TMP_Dropdown>().IsExpanded)
        {
            Canvas canvas = transform.GetComponentInChildren<Canvas>();
            canvas.overrideSorting = false;
        }
    }
}
