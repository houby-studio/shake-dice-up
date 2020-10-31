using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLink : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	private TextMeshProUGUI textMessage;

	// Open clicked link in default application for
	public void OnPointerClick(PointerEventData eventData)
	{
		int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMessage, eventData.position, eventData.pressEventCamera);
		if (linkIndex == -1)
			return;
		TMP_LinkInfo linkInfo = textMessage.textInfo.linkInfo[linkIndex];
		string selectedLink = linkInfo.GetLinkID();
		if (selectedLink != "")
		{
			Application.OpenURL(selectedLink);
		}
	}
}