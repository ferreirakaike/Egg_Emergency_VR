using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class FixDropdownOcclusion : TMP_Dropdown
{
    private GameObject overrideButtonText;
    private TMP_Dropdown dropdown;
    protected override void Start()
    {
        base.Start();
        dropdown = GetComponent<TMP_Dropdown>();
        overrideButtonText = transform.parent.Find("OverrideDropdownButton").GetChild(0).gameObject;
        overrideButtonText.GetComponent<TextMeshProUGUI>().text = dropdown.options[dropdown.value].text;

    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        // base.OnPointerClick(eventData);
        if (GetComponent<TMP_Dropdown>().IsExpanded)
        {
            overrideButtonText.transform.parent.gameObject.SetActive(true);
            Canvas canvas = transform.GetComponentInChildren<Canvas>();
            canvas.overrideSorting = false;
        }
    }

    public void UpdateText()
    {
        overrideButtonText.GetComponent<TextMeshProUGUI>().text = dropdown.options[dropdown.value].text;
        overrideButtonText.transform.parent.gameObject.SetActive(false);
    }
}
