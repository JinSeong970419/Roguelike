using UnityEngine;

namespace Roguelike.Core
{
    public class CameraConfiner : MonoBehaviour
    {
        [SerializeField] private bool _xAxis;
        [SerializeField] private bool _yAxis;
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;
        [SerializeField] private float _minY;
        [SerializeField] private float _maxY;

        public bool XAxis { get { return _xAxis; } }
        public bool YAxis { get { return _yAxis; } }
        public float MinX { get { return _minX; } }
        public float MaxX { get { return _maxX; } }
        public float MinY { get { return _minY; } }
        public float MaxY { get { return _maxY; } }

        private void OnEnable()
        {
            var gameCamera = FindObjectOfType<GameCamera>();
            gameCamera.SetConfiner(this);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.red;
            Vector3[] lineSegments = new Vector3[8];
            lineSegments[0] = new Vector3(_minX, _minY, 0);
            lineSegments[1] = new Vector3(_maxX, _minY, 0);
            lineSegments[2] = new Vector3(_minX, _maxY, 0);
            lineSegments[3] = new Vector3(_maxX, _maxY, 0);

            lineSegments[4] = new Vector3(_minX, _minY, 0);
            lineSegments[5] = new Vector3(_minX, _maxY, 0);
            lineSegments[6] = new Vector3(_maxX, _minY, 0);
            lineSegments[7] = new Vector3(_maxX, _maxY, 0);
            UnityEditor.Handles.DrawLine(lineSegments[0], lineSegments[1], 3);
            UnityEditor.Handles.DrawLine(lineSegments[2], lineSegments[3], 3);
            UnityEditor.Handles.DrawLine(lineSegments[4], lineSegments[5], 3);
            UnityEditor.Handles.DrawLine(lineSegments[6], lineSegments[7], 3);
        }
#endif
    }
}
