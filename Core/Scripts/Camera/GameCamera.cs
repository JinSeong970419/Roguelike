using TMPro;
using UnityEngine;

namespace Roguelike.Core
{
    public abstract class GameCamera : MonoBehaviour
    {
        [SerializeField] protected Camera _camera;
        [SerializeField] protected GameObject _target;
        [SerializeField] protected float _zoom;

        protected Vector3 _position= Vector3.zero;
        #region Zoom
        protected bool _zoomFlag = false;
        protected float _zoomTick = 0f;
        protected float _zoomDuration = 1f;
        protected float _targetZoom;

        #endregion

        #region Confine
        protected CameraConfiner _confiner;
        protected float _tangent;
        protected float _aspectRatio;
        #endregion


        public Camera Camera { get { return _camera; } }
        public GameObject Target { get { return _target; } }
        public float Zoom { get { return _zoom; } }

        protected virtual void OnEnable()
        {
            if(_camera!= null)
            {
                float fov = Mathf.Deg2Rad * _camera.fieldOfView;
                _tangent = Mathf.Tan(fov * 0.5f);

                _aspectRatio = (float)Screen.width / Screen.height;
            }
            
        }

        protected virtual void OnValidate()
        {
            if (_camera == null) _camera = GetComponent<Camera>();
        }

        protected virtual void LateUpdate()
        {
            if (_camera == null) return;
            

            ProcessZoom();

            if(_target != null )
            {
                ProcessFollowTarget(_target);
            }

            ProcessConfine();
            ProcessPosition();
        }

        public void SetTarget(GameObject target)
        {
            if (target == null) return;
            _target = target;
        }

        public void SetZoom(float zoom, float time)
        {
            _targetZoom = zoom;
            _zoomFlag = true;
            _zoomDuration = time;
        }

        public void SetConfiner(CameraConfiner confiner)
        {
            this._confiner = confiner;
        }

        protected abstract void ProcessFollowTarget(GameObject target);
        protected abstract void ProcessZoom();

        private void ProcessConfine()
        {
            if (_confiner == null) return;
            if (_target == null) return;

            var position = _position;
            var halfHeigth = _tangent * Mathf.Abs(transform.position.z);
            var halfWidth = _aspectRatio * halfHeigth;
            float viewMinX = position.x - halfWidth;
            float viewMinY = position.y - halfHeigth;
            float viewMaxX = position.x + halfWidth;
            float viewMaxY = position.y + halfHeigth;

            if (_confiner.XAxis)
            {
                if (viewMinX < _confiner.MinX)
                {
                    position.x = _confiner.MinX + halfWidth;
                }
                if (viewMaxX > _confiner.MaxX)
                {
                    position.x = _confiner.MaxX - halfWidth;
                }
            }

            if (_confiner.YAxis)
            {
                if (viewMinY < _confiner.MinY)
                {
                    position.y = _confiner.MinY + halfHeigth;
                }
                if (viewMaxY > _confiner.MaxY)
                {
                    position.y = _confiner.MaxY - halfHeigth;
                }
            }

            this._position = position;
        }

        private void ProcessPosition()
        {
            transform.position = _position;
        }


#if UNITY_EDITOR
        [DebugButton]
        public void Test()
        {
            Debug.Log($"{Camera.fieldOfView}");

        }

        private void OnDrawGizmos()
        {
            if (_confiner == null) return;
            if (_target == null) return;

            var position = transform.position;
            var halfHeigth = _tangent * transform.position.z;
            var halfWidth = _aspectRatio * halfHeigth;
            float minX = position.x - halfWidth;
            float minY = position.y - halfHeigth;
            float maxX = position.x + halfWidth;
            float maxY = position.y + halfHeigth;

            UnityEditor.Handles.color = Color.blue;
            Vector3[] lineSegments = new Vector3[8];
            lineSegments[0] = new Vector3(minX, minY, 0);
            lineSegments[1] = new Vector3(maxX, minY, 0);
            lineSegments[2] = new Vector3(minX, maxY, 0);
            lineSegments[3] = new Vector3(maxX, maxY, 0);

            lineSegments[4] = new Vector3(minX, minY, 0);
            lineSegments[5] = new Vector3(minX, maxY, 0);
            lineSegments[6] = new Vector3(maxX, minY, 0);
            lineSegments[7] = new Vector3(maxX, maxY, 0);
            UnityEditor.Handles.DrawLine(lineSegments[0], lineSegments[1], 3);
            UnityEditor.Handles.DrawLine(lineSegments[2], lineSegments[3], 3);
            UnityEditor.Handles.DrawLine(lineSegments[4], lineSegments[5], 3);
            UnityEditor.Handles.DrawLine(lineSegments[6], lineSegments[7], 3);
        }
#endif


    }
}
