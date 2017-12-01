using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectMask2D))]
[RequireComponent(typeof(ScrollRect))]
public class SnapScrollRect : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    public UnityIntEvent OnPageChanged;

    public int StartPageIndex = 0;
    public float DecelerationRate = 10.0f;
    public float SnapDistance = 50.0f;

    private ScrollRect scrollRectComponent;
    private RectTransform scrollRectRect;
    private RectTransform scrollRectContainer;

    private bool horizontal;

    private int pageCount;
    private int currentPage;

    private bool lerping;
    private Vector2 lerpTo;

    private List<Vector2> pagePositions = new List<Vector2>();

    private bool dragging;
    private float dragStartTime;
    private Vector2 dragStartPosition;

    private void Start()
    {
        scrollRectComponent = GetComponent<ScrollRect>();
        scrollRectRect = GetComponent<RectTransform>();
        scrollRectContainer = scrollRectComponent.content;
        pageCount = scrollRectContainer.childCount;

        horizontal = (scrollRectComponent.horizontal && !scrollRectComponent.vertical);

        lerping = false;

        Invoke("InitialSetup", 0.01f);
    }

    private void Update()
    {
        if (lerping)
        {
            float decelerate = Mathf.Min(DecelerationRate * Time.deltaTime, 1.0f);
            scrollRectContainer.anchoredPosition =
                Vector2.Lerp(scrollRectContainer.anchoredPosition, lerpTo, decelerate);
            if (Vector2.SqrMagnitude(scrollRectContainer.anchoredPosition - lerpTo) < SnapDistance)
            {
                scrollRectContainer.anchoredPosition = lerpTo;
                lerping = false;
                scrollRectComponent.velocity = Vector2.zero;
            }
        }
    }

    private void InitialSetup()
    {
        SetPagePositions();
        SetPage(StartPageIndex);
    }

    private void SetPagePositions()
    {
        int width = 0;
        int height = 0;
        int offsetX = 0;
        int offsetY = 0;
        int containerWidth = 0;
        int containerHeight = 0;

        if (horizontal)
        {
            width = (int)scrollRectRect.rect.width;
            offsetX = width / 2;
            containerWidth = width * pageCount;
        }
        else
        {
            height = (int)scrollRectRect.rect.height;
            offsetY = height / 2;
            containerHeight = height * pageCount;
        }

        Vector2 newSize = new Vector2(containerWidth, containerHeight);
        scrollRectContainer.sizeDelta = newSize;

        pagePositions.Clear();

        for (int i = 0; i < pageCount; i++)
        {
            RectTransform child = scrollRectContainer.GetChild(i).GetComponent<RectTransform>();
            Vector2 childPosition;
            if (horizontal)
            {
                childPosition = new Vector2(i * width, 0.0f);
            }
            else
            {
                childPosition = new Vector2(0.0f, -(i * height));
            }
            child.anchoredPosition = childPosition;
            pagePositions.Add(-childPosition);
        }
    }

    private void SetPage(int pageIndex)
    {
        pageIndex = Mathf.Clamp(pageIndex, 0, pageCount - 1);
        scrollRectContainer.anchoredPosition = pagePositions[pageIndex];
        currentPage = pageIndex;

        OnPageChanged.Invoke(pageIndex);
    }

    private void LerpToPage(int pageIndex)
    {
        pageIndex = Mathf.Clamp(pageIndex, 0, pageCount - 1);
        lerpTo = pagePositions[pageIndex];
        lerping = true;
        currentPage = pageIndex;

        OnPageChanged.Invoke(pageIndex);
    }

    private int GetNearestPage()
    {
        Vector2 currentPosition = scrollRectContainer.anchoredPosition;
        float distance = float.MaxValue;
        int nearestPage = currentPage;

        for (int i = 0; i < pagePositions.Count; i++)
        {
            float testDistance = Vector2.SqrMagnitude(currentPosition - pagePositions[i]);
            if (testDistance < distance)
            {
                distance = testDistance;
                nearestPage = i;
            }
        }

        return nearestPage;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lerping = false;
        dragging = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float difference;
        if (horizontal)
        {
            difference = dragStartPosition.x - scrollRectContainer.anchoredPosition.x;
        }
        else
        {
            difference = -(dragStartPosition.y - scrollRectContainer.anchoredPosition.y);
        }

        LerpToPage(GetNearestPage());
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging)
        {
            dragging = true;
            dragStartTime = Time.unscaledTime;
            dragStartPosition = scrollRectContainer.anchoredPosition;
        }
        else
        {
            
        }
    }
}
