using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using YG;

public class ContactLink : MonoBehaviour, IPointerClickHandler
{
    private const string LINK = "https://t.me/defleg";

    public void OnPointerClick(PointerEventData eventData)
    {
        YG2.OnURL(LINK);
#if UNITY_EDITOR
        Debug.Log("Contact link clicked");
#endif
    }
}
