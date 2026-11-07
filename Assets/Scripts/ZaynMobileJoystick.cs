using UnityEngine; using UnityEngine.EventSystems;
public class ZaynMobileJoystick:MonoBehaviour,IPointerDownHandler,IDragHandler,IPointerUpHandler{
 public RectTransform background,handle; public float radius=100f;
 public void OnPointerDown(PointerEventData e){OnDrag(e);}
 public void OnDrag(PointerEventData e){if(!background||!handle||!ZaynCompleteGame.Instance)return;
  RectTransformUtility.ScreenPointToLocalPointInRectangle(background,e.position,e.pressEventCamera,out Vector2 p);
  Vector2 input=new Vector2(p.x/(background.sizeDelta.x/2f),p.y/(background.sizeDelta.y/2f));if(input.magnitude>1)input.Normalize();
  handle.anchoredPosition=input*radius;ZaynCompleteGame.Instance.SetMove(input);}
 public void OnPointerUp(PointerEventData e){if(handle)handle.anchoredPosition=Vector2.zero;if(ZaynCompleteGame.Instance)ZaynCompleteGame.Instance.SetMove(Vector2.zero);}
}
