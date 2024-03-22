using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// ��ũ��Ʈ�� �ۼ��Ͽ� ����� �� �� ��󿡰� �� �ʿ��� ������Ʈ�� ���� ���̴�.
// Ȥ�� �� ������Ʈ�� �������� �ݵ�� �ٵ��� ���ִ� ������ ���ش�.
// ���� ������Ʈ�� �ִٸ� ����� �Ⱦ����� ���ٸ� �ڵ����� �־��ְ� �ȴ�.
//[RequireComponent(typeof(Image))]

public class DraggableSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Vector2 _draggingOffset = Vector2.zero; // item�� ��� �پ����� ������, zero��� ���콺 Ŀ���� ���

    GameObject _draggingObject;         // ���� �巡�� ���� ������Ʈ
    RectTransform _canvasRectTransform; // ĵ������ RectTransform
    /// <summary>
    /// ó�� �巡�װ� �Ͼ�� ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ���� �۾�
        if (_draggingObject != null)
        {
            Destroy(_draggingObject);
        }
        Image srcIcon = transform.GetChild(0).GetComponent<Image>(); //���� slot�̱� ������ �ȿ� ������ item�� icon image�� �����;� �Ѵ�.

        _draggingObject = new GameObject("Dragging Object");
        _draggingObject.transform.SetParent(srcIcon.canvas.transform); // ���� �����ִ� ĵ����
        _draggingObject.transform.SetAsLastSibling();       // ���� ���� ���̵���
        _draggingObject.transform.localScale = Vector3.one;

        // ���� ĵ������ �Բ� �����̵��� �ϴ� ��쿡 ����ϴ� ���� canvasGroup�̴�.
        CanvasGroup _canvasGroup = _draggingObject.AddComponent<CanvasGroup>();
        _canvasGroup.blocksRaycasts = false;        // �ٸ� �͵��� �����ǵ��� raycast�� ��������.

        // ���� �巡�� ���� Object�� �巡�׸� �Ϸ��� �ϴ� Object�� ���� ������� ����� ��
        Image draggingImge = _draggingObject.AddComponent<Image>();
        draggingImge.sprite = srcIcon.sprite;
        draggingImge.rectTransform.sizeDelta = srcIcon.rectTransform.sizeDelta;
        draggingImge.color = srcIcon.color;
        draggingImge.material = srcIcon.material;

        _canvasRectTransform = draggingImge.canvas.transform as RectTransform;

        // ��ġ����
        UpdateDraggingObjectPos(eventData);
    }
    /// <summary>
    /// �巡�װ� �Ͼ�� ���·� ���콺�� Ŭ���� �������� ���� ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        UpdateDraggingObjectPos(eventData);
    }
    /// <summary>
    /// �����̴ٰ� �巡���ϴ� ����� ���� ���, �������� �巡�׾� ����ϰ� �� �� �߻��ϴ� �̺�Ʈ
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(_draggingObject);
    }

    // �Լ�
    void UpdateDraggingObjectPos(PointerEventData eventData)
    {
        if (_draggingObject != null)
        {
            // �巡�� ���� �������� ��ũ�� ��ǥ ���
            Vector3 _screenPos = eventData.position + _draggingOffset;

            // ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
            Camera _cam = eventData.pressEventCamera;
            Vector3 _newPos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvasRectTransform, _screenPos, _cam, out _newPos))
            {
                _draggingObject.transform.position = _newPos;
                _draggingObject.transform.rotation = _canvasRectTransform.rotation;
            }
        }
    }

    /* ����� DraggableSlot ����
     * [SerializeField] Vector2 _draggingOffset = Vector2.zero; // item�� ��� �پ����� ������, zero��� ���콺 Ŀ���� ���

    GameObject _draggingObject;         // ���� �巡�� ���� ������Ʈ
    RectTransform _canvasRectTransform; // ĵ������ RectTransform
    /// <summary>
    /// ó�� �巡�װ� �Ͼ�� ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ���� �۾�
        if (_draggingObject != null)
        {
            Destroy(_draggingObject);
        }
        Image srcIcon = transform.GetChild(0).GetComponent<Image>(); //���� slot�̱� ������ �ȿ� ������ item�� icon image�� �����;� �Ѵ�.

        _draggingObject = new GameObject("Dragging Object");
        _draggingObject.transform.SetParent(srcIcon.canvas.transform); // ���� �����ִ� ĵ����
        _draggingObject.transform.SetAsLastSibling();       // ���� ���� ���̵���
        _draggingObject.transform.localScale = Vector3.one;

        // ���� ĵ������ �Բ� �����̵��� �ϴ� ��쿡 ����ϴ� ���� canvasGroup�̴�.
        CanvasGroup _canvasGroup = _draggingObject.AddComponent<CanvasGroup>();
        _canvasGroup.blocksRaycasts = false;        // �ٸ� �͵��� �����ǵ��� raycast�� ��������.

        // ���� �巡�� ���� Object�� �巡�׸� �Ϸ��� �ϴ� Object�� ���� ������� ����� ��
        Image draggingImge = _draggingObject.AddComponent<Image>();
        draggingImge.sprite = srcIcon.sprite;
        draggingImge.rectTransform.sizeDelta = srcIcon.rectTransform.sizeDelta;
        draggingImge.color = srcIcon.color;
        draggingImge.material = srcIcon.material;

        _canvasRectTransform = draggingImge.canvas.transform as RectTransform;

        // ��ġ����
        UpdateDraggingObjectPos(eventData);
    }
    /// <summary>
    /// �巡�װ� �Ͼ�� ���·� ���콺�� Ŭ���� �������� ���� ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        UpdateDraggingObjectPos(eventData);
    }
    /// <summary>
    /// �����̴ٰ� �巡���ϴ� ����� ���� ���, �������� �巡�׾� ����ϰ� �� �� �߻��ϴ� �̺�Ʈ
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(_draggingObject);
    }

    // �Լ�
    void UpdateDraggingObjectPos(PointerEventData eventData)
    {
        if (_draggingObject != null)
        {
            // �巡�� ���� �������� ��ũ�� ��ǥ ���
            Vector3 _screenPos = eventData.position + _draggingOffset;

            // ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
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
