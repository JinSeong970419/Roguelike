using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Roguelike.Core
{
    public enum ScrollDirection
    {
        Horizontal,
        Vertical,
    }

    public enum ScrollType
    {
        Step,
        Velocity,
    }

    public class ScrollPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] private ScrollDirection _scrollDirection;
        [SerializeField] private RectTransform _rect;
        [SerializeField] private RectTransform _content;

        [SerializeField] private int _startingScreen;
        [SerializeField] private ScrollType _scrollType;

        [DrawIf("_scrollType", ScrollType.Step)]
        [Range(0, 8)]
        [SerializeField] private float _pageStep;
        [DrawIf("_scrollType", ScrollType.Step)]
        [SerializeField] private float _swipeVelocityThreshold = 100;

        [DrawIf("_scrollType", ScrollType.Velocity)]
        [Range(1f, 10f)]
        [SerializeField] private float _scrollSpeed;
        [DrawIf("_scrollType", ScrollType.Velocity)]
        [Range(0f, 1f)]
        [SerializeField] private float _demping;

        [SerializeField] private bool _screenMatch;


        private Vector2 _totalSize;
        private Vector2 _minPos;
        private Vector2 _maxPos;

        private bool _firstDragging = true;
        private bool _isDragging = false;
        private float _limitYPos = 0f;
        private Vector2 _dragPoint;

        private Vector3 _velocity = Vector3.zero;
        private float _currentSpeed = 0f;
        private Vector2 _direction = Vector2.zero;

        private List<Vector2> _pagePositionList = new List<Vector2>();

        private int _currentPage = 0;

        // Step Only
        private Vector2 _targetPosition = Vector2.zero;
        private float _tick = 0f;
        private bool _swipe = false;

        // Common
        private Vector2 _canvasSize;
        private RectTransform _topCanvasRect;

        private void OnValidate()
        {
            _rect = GetComponent<RectTransform>();

            if (_content == null)
            {
                Transform content = transform.Find("Content");
                if (content != null)
                {
                    _content = content.GetComponent<RectTransform>();
                }
            }
        }

        private void Start()
        {
            Initialize();
            ProcessStartingScreen();
        }

        private void Update()
        {
            ProcessScroll();
            ProcessScreenResolution();
        }

        private void Initialize()
        {
            var topCanvas = transform.root.GetComponentInChildren<Canvas>();
            if (topCanvas != null)
            {
                _topCanvasRect = topCanvas.GetComponent<RectTransform>();
                if (_topCanvasRect != null)
                {
                    //Debug.Log($"Top Canvas: {_topCanvasRect.rect}");
                    _canvasSize = new Vector2(_topCanvasRect.rect.width, _topCanvasRect.rect.height);
                }
            }

            if (_content != null)
            {
                _pagePositionList.Clear();
                Vector2 accumulateChildPos = Vector2.zero;
                Vector2 accumulateContentPos = Vector2.zero;
                _totalSize = Vector2.zero;
                for (int i = 0; i < _content.childCount; i++)
                {
                    var child = _content.GetChild(i) as RectTransform;
                    //Debug.Log(child.sizeDelta);

                    if(_screenMatch)
                    {
                        child.sizeDelta = _canvasSize;
                    }

                    _totalSize.x += child.rect.width;
                    _totalSize.y += child.rect.height;

                    child.anchoredPosition = accumulateChildPos;
                    _pagePositionList.Add(accumulateContentPos);

                    switch (_scrollDirection)
                    {
                        case ScrollDirection.Horizontal:
                            accumulateChildPos.x += child.rect.width;
                            accumulateContentPos.x -= child.rect.width;
                            break;
                        case ScrollDirection.Vertical:
                            accumulateChildPos.y += child.rect.height;
                            accumulateContentPos.y -= child.rect.height;
                            break;
                        default:
                            break;
                    }
                }

                //Debug.Log($"content height {_rect.rect.height}");
            }

            _minPos = Vector2.zero;
            _maxPos = Vector2.zero;
            int count = _pagePositionList.Count;
            for (int i = 0; i < count; i++)
            {
                var pagePosition = _pagePositionList[i];
                _minPos.x = pagePosition.x < _minPos.x ? pagePosition.x : _minPos.x;
                _maxPos.x = pagePosition.x > _maxPos.x ? pagePosition.x : _maxPos.x;

                _minPos.y = pagePosition.y < _minPos.y ? pagePosition.y : _minPos.y;
                _maxPos.y = pagePosition.y > _maxPos.y ? pagePosition.y : _maxPos.y;
            }
        }

        private void ProcessStartingScreen()
        {
            int index = _startingScreen;
            if (index < 0) return;
            if (index >= _pagePositionList.Count) return;

            _content.anchoredPosition = _pagePositionList[index];
        }

        private void ProcessScroll()
        {
            switch (_scrollType)
            {
                case ScrollType.Step:
                    {
                        if (_swipe)
                        {
                            _tick += Time.deltaTime;
                            if (_tick >= 1f)
                            {
                                _tick = 1f;
                                _swipe = false;
                            }
                            Vector2 nextPosition = Vector2.Lerp(_content.anchoredPosition, _targetPosition, _tick);
                            _content.anchoredPosition = nextPosition;
                        }

                    }
                    break;
                case ScrollType.Velocity:
                    {
                        if (_scrollDirection != ScrollDirection.Horizontal)
                        {
                            _velocity.x = 0f;
                        }

                        if (_scrollDirection != ScrollDirection.Vertical)
                        {
                            _velocity.y = 0f;
                        }

                        Vector2 nextPosition = _content.localPosition + _velocity * _scrollSpeed * Time.deltaTime;

                        if (nextPosition.x < _minPos.x)
                        {
                            nextPosition.x = _minPos.x;
                        }
                        if (nextPosition.x > _maxPos.x)
                        {
                            nextPosition.x = _maxPos.x;
                        }

                        if (nextPosition.y < _minPos.y)
                        {
                            nextPosition.y = _minPos.y;
                        }
                        if (nextPosition.y > _maxPos.y)
                        {
                            nextPosition.y = _maxPos.y;
                        }

                        _content.anchoredPosition = nextPosition;

                        _velocity = _velocity - (_velocity * _demping * Time.deltaTime);
                        _currentSpeed = _velocity.magnitude;
                    }
                    break;
                default:
                    break;
            }

            _currentPage = GetNearestPage(_content.anchoredPosition);
        }

        private void ProcessScreenResolution()
        {
            if (_topCanvasRect == null) return;

            float widthDiff = Mathf.Abs(_canvasSize.x - _topCanvasRect.rect.width);
            float heightDiff = Mathf.Abs(_canvasSize.y - _topCanvasRect.rect.height);

            if (widthDiff > float.Epsilon || heightDiff > float.Epsilon)
            {
                Initialize();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            _dragPoint = eventData.position;
            _velocity = Vector3.zero;
            if (_firstDragging)
            {
                _firstDragging = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isDragging)
            {
                _direction = eventData.position - _dragPoint;

                Debug.Log($"Direction {_direction}");
                _velocity = _direction;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;

            if (_scrollType == ScrollType.Step)
            {
                switch (_scrollDirection)
                {
                    case ScrollDirection.Horizontal:
                        {
                            if (_velocity.x > _swipeVelocityThreshold)
                            {
                                PreviousPage();
                            }
                            else if (_velocity.x < -_swipeVelocityThreshold)
                            {
                                NextPage();
                            }
                        }
                        break;
                    case ScrollDirection.Vertical:
                        {
                            if (_velocity.y > _swipeVelocityThreshold)
                            {
                                PreviousPage();
                            }
                            else if (_velocity.y < -_swipeVelocityThreshold)
                            {
                                NextPage();
                            }
                        }
                        break;
                    default:
                        break;
                }

                _velocity = Vector3.zero;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public void ChangePage(int index)
        {
            if (index < 0) return;
            if (index >= _pagePositionList.Count) return;

            _tick = 0f;
            _swipe = true;
            _targetPosition = _pagePositionList[index];

        }

        public void NextPage()
        {
            ChangePage(_currentPage + 1);
        }

        public void PreviousPage()
        {
            ChangePage(_currentPage - 1);
        }

        private int GetNearestPage(Vector2 pos)
        {
            int nearest = 0;
            float minDist = float.MaxValue;
            int pageCount = _pagePositionList.Count;
            for (int i = 0; i < pageCount; i++)
            {
                Vector2 pagePos = _pagePositionList[i];
                float dist = (pos - pagePos).sqrMagnitude;
                if (dist < minDist)
                {
                    nearest = i;
                    minDist = dist;
                }
            }

            return nearest;
        }
    }
}
