using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


/// <summary>
/// This class is used together with a UI button to fix the occulusion behavior of the TextMeshPro dropdown menu
/// </summary>
public class FixDropdownOcclusion : TMP_Dropdown
{
    private GameObject overrideButtonText;
    private TMP_Dropdown dropdown;

    protected override void Start()
    {
        base.Start();
        overrideButtonText = transform.parent.Find("OverrideDropdownButton").GetChild(0).gameObject;
    }

    /// <summary>
    /// Override parent method. This method moves the Override Button to the location of the dropdown and set the overrideSorting of the Canvas component of the child of the dropdown to false to fix occulusion
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerClick(PointerEventData eventData)
    {
        // base.OnPointerClick(eventData);
        if (GetComponent<TMP_Dropdown>().IsExpanded)
        {
            overrideButtonText.transform.parent.gameObject.SetActive(true);
            dropdown = GetComponent<TMP_Dropdown>();
            if (dropdown.options.Count > 0 && dropdown.value >= 0)
            {
                overrideButtonText.GetComponent<TextMeshProUGUI>().text = dropdown.options[dropdown.value].text;
            }
            else
            {
                overrideButtonText.GetComponent<TextMeshProUGUI>().text = "";
            }            
            Canvas canvas = transform.GetComponentInChildren<Canvas>();
            canvas.overrideSorting = false;
            overrideButtonText.transform.parent.position = transform.position;
            Debug.Log(overrideButtonText.GetComponentInParent<RectTransform>().localPosition.ToString());
        }
    }

    /// <summary>
    /// This method updates the text of the override button and disable it.
    /// </summary>
    public void UpdateText()
    {
        overrideButtonText.GetComponent<TextMeshProUGUI>().text = dropdown.options[dropdown.value].text;
        overrideButtonText.transform.parent.gameObject.SetActive(false);
    }
}
