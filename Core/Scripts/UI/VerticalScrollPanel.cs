using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Roguelike.Core
{
    public class VerticalScrollPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private RectTransform _content;
        [Range(1f, 10f)]
        [SerializeField] private float _scrollSpeed;

        [SerializeField] private HorizontalScrollSnap _horizontalScrollSnap;
        [SerializeField] private RectTransform _horizontalContent;

        private Image _image;

        private float _totalHeight = 0f;

        private bool _firstDragging = true;
        private bool _isDragging = false;
        private float _defaultYPos = 0f;
        private float _limitYPos = 0f;
        private Vector2 _dragPoint;
        private Vector2 _scrollStartPoint;
        private Vector3 _horizontalScrollStartPoint;
        private bool _horizontal = false;

        private Vector3 _velocity = Vector3.zero;
        private Vector2 _direction = Vector2.zero;

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

            if (_horizontalScrollSnap != null)
            {
                Transform horizontalContent = _horizontalScrollSnap.transform.Find("Content");
                if (horizontalContent != null)
                {
                    _horizontalContent = horizontalContent.GetComponent<RectTransform>();
                }
            }
        }

        private void Start()
        {
            _image = GetComponent<Image>();

            if (_content != null)
            {
                _totalHeight = 0f;
                _defaultYPos = _content.position.y;
                for (int i = 0; i < _content.childCount; i++)
                {
                    var child = _content.GetChild(i) as RectTransform;
                    _totalHeight += child.rect.height;
                }

                Debug.Log($"content height {_rect.rect.height}");
            }

            var parentTransform = transform.parent.GetComponent<RectTransform>();
            if (parentTransform != null)
            {
                Debug.Log($"{parentTransform.sizeDelta.y}");
                _limitYPos = _totalHeight - parentTransform.sizeDelta.y;
            }
        }

        private void Update()
        {
            if (_isDragging == false)
            {

            }
            //_content.position += _velocity;
            //_velocity *= 0.9f * Time.deltaTime;
            //if (_velocity.magnitude <= float.Epsilon)
            //{
            //    _velocity = Vector3.zero;
            //}
            Vector2 nextPosition = _content.localPosition + _velocity * _scrollSpeed * Time.deltaTime;
            if (nextPosition.y < 0)
            {
                nextPosition.y = 0;
            }
            if (nextPosition.y > _limitYPos)
            {
                nextPosition.y = _limitYPos;
            }

            _content.localPosition = nextPosition;
            //Debug.Log(_velocity);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            _dragPoint = eventData.position;
            _scrollStartPoint = _content.localPosition;
            _horizontalScrollStartPoint = _horizontalContent.localPosition;
            _velocity = Vector3.zero;
            if (_firstDragging)
            {
                _firstDragging = false;
                _defaultYPos = _content.position.y;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isDragging)
            {
                _direction = eventData.position - _dragPoint;
                Vector3 horizontalDirection = new Vector3(_direction.x, 0f);
                Vector3 verticalDirection = new Vector3(0f, _direction.y);
                _horizontal = Mathf.Abs(_direction.x) > Mathf.Abs(_direction.y);
                
                Debug.Log($"Direction {_direction}");
                _velocity = verticalDirection;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;

            if(_horizontal)
            {
                if (_direction.x > 100f)
                {
                    _horizontalScrollSnap.ChangePage(_horizontalScrollSnap.CurrentPage - 1);
                }
                else if (_direction.x < -100f)
                {
                    _horizontalScrollSnap.ChangePage(_horizontalScrollSnap.CurrentPage + 1);
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {

        }
    }
}
