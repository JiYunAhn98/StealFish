using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// 스크립트를 작성하여 사용할 때 그 대상에게 꼭 필요한 컴포넌트가 있을 것이다.
// 혹시 그 컴포넌트가 빠지더라도 반드시 붙도록 해주는 역할을 해준다.
// 만약 컴포넌트가 있다면 상관을 안쓰지만 없다면 자동으로 넣어주게 된다.
//[RequireComponent(typeof(Image))]

public class DraggableSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Vector2 _draggingOffset = Vector2.zero; // item이 어디에 붙어있을 것인지, zero라면 마우스 커서의 가운데

    GameObject _draggingObject;         // 현재 드래그 중인 오브젝트
    RectTransform _canvasRectTransform; // 캔버스의 RectTransform
    /// <summary>
    /// 처음 드래그가 일어나는 순간
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 사전 작업
        if (_draggingObject != null)
        {
            Destroy(_draggingObject);
        }
        Image srcIcon = transform.GetChild(0).GetComponent<Image>(); //현재 slot이기 때문에 안에 들어오는 item의 icon image를 가져와야 한다.

        _draggingObject = new GameObject("Dragging Object");
        _draggingObject.transform.SetParent(srcIcon.canvas.transform); // 현재 속해있는 캔버스
        _draggingObject.transform.SetAsLastSibling();       // 가장 먼저 보이도록
        _draggingObject.transform.localScale = Vector3.one;

        // 개별 캔버스를 함께 움직이도록 하는 경우에 사용하는 것이 canvasGroup이다.
        CanvasGroup _canvasGroup = _draggingObject.AddComponent<CanvasGroup>();
        _canvasGroup.blocksRaycasts = false;        // 다른 것들이 감지되도록 raycast를 꺼버린다.

        // 현재 드래그 중인 Object와 드래그를 하려고 하는 Object를 같은 모습으로 만드는 것
        Image draggingImge = _draggingObject.AddComponent<Image>();
        draggingImge.sprite = srcIcon.sprite;
        draggingImge.rectTransform.sizeDelta = srcIcon.rectTransform.sizeDelta;
        draggingImge.color = srcIcon.color;
        draggingImge.material = srcIcon.material;

        _canvasRectTransform = draggingImge.canvas.transform as RectTransform;

        // 위치갱신
        UpdateDraggingObjectPos(eventData);
    }
    /// <summary>
    /// 드래그가 일어나는 상태로 마우스의 클릭이 떨어지지 않은 상태
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        UpdateDraggingObjectPos(eventData);
    }
    /// <summary>
    /// 움직이다가 드래그하던 대상을 놓는 경우, 아이템을 드래그앤 드롭하고 난 후 발생하는 이벤트
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(_draggingObject);
    }

    // 함수
    void UpdateDraggingObjectPos(PointerEventData eventData)
    {
        if (_draggingObject != null)
        {
            // 드래그 중인 아이콘의 스크린 좌표 계산
            Vector3 _screenPos = eventData.position + _draggingOffset;

            // 스크린 좌표를 월드 좌표로 변환
            Camera _cam = eventData.pressEventCamera;
            Vector3 _newPos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvasRectTransform, _screenPos, _cam, out _newPos))
            {
                _draggingObject.transform.position = _newPos;
                _draggingObject.transform.rotation = _canvasRectTransform.rotation;
            }
        }
    }

    /* 강사님 DraggableSlot 원본
     * [SerializeField] Vector2 _draggingOffset = Vector2.zero; // item이 어디에 붙어있을 것인지, zero라면 마우스 커서의 가운데

    GameObject _draggingObject;         // 현재 드래그 중인 오브젝트
    RectTransform _canvasRectTransform; // 캔버스의 RectTransform
    /// <summary>
    /// 처음 드래그가 일어나는 순간
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 사전 작업
        if (_draggingObject != null)
        {
            Destroy(_draggingObject);
        }
        Image srcIcon = transform.GetChild(0).GetComponent<Image>(); //현재 slot이기 때문에 안에 들어오는 item의 icon image를 가져와야 한다.

        _draggingObject = new GameObject("Dragging Object");
        _draggingObject.transform.SetParent(srcIcon.canvas.transform); // 현재 속해있는 캔버스
        _draggingObject.transform.SetAsLastSibling();       // 가장 먼저 보이도록
        _draggingObject.transform.localScale = Vector3.one;

        // 개별 캔버스를 함께 움직이도록 하는 경우에 사용하는 것이 canvasGroup이다.
        CanvasGroup _canvasGroup = _draggingObject.AddComponent<CanvasGroup>();
        _canvasGroup.blocksRaycasts = false;        // 다른 것들이 감지되도록 raycast를 꺼버린다.

        // 현재 드래그 중인 Object와 드래그를 하려고 하는 Object를 같은 모습으로 만드는 것
        Image draggingImge = _draggingObject.AddComponent<Image>();
        draggingImge.sprite = srcIcon.sprite;
        draggingImge.rectTransform.sizeDelta = srcIcon.rectTransform.sizeDelta;
        draggingImge.color = srcIcon.color;
        draggingImge.material = srcIcon.material;

        _canvasRectTransform = draggingImge.canvas.transform as RectTransform;

        // 위치갱신
        UpdateDraggingObjectPos(eventData);
    }
    /// <summary>
    /// 드래그가 일어나는 상태로 마우스의 클릭이 떨어지지 않은 상태
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        UpdateDraggingObjectPos(eventData);
    }
    /// <summary>
    /// 움직이다가 드래그하던 대상을 놓는 경우, 아이템을 드래그앤 드롭하고 난 후 발생하는 이벤트
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(_draggingObject);
    }

    // 함수
    void UpdateDraggingObjectPos(PointerEventData eventData)
    {
        if (_draggingObject != null)
        {
            // 드래그 중인 아이콘의 스크린 좌표 계산
            Vector3 _screenPos = eventData.position + _draggingOffset;

            // 스크린 좌표를 월드 좌표로 변환
            Camera _cam = eventData.pressEventCamera;
            Vector3 _newPos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvasRectTransform, _screenPos, GetComponent<Camera>(), out _newPos))
            {
                _draggingObject.transform.position = _newPos;
                _draggingObject.transform.rotation = _canvasRectTransform.rotation;
                //_draggingObject.transform.LookAt(_newPos);

            }
        }
    }
     */
}
