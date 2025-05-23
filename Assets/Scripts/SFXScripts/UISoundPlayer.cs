using UnityEngine;
using UnityEngine.EventSystems;

public class UISoundPlayer : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SFXManager.Instance.PlayUI(SFXEvent.UISoundS);
    }
}
