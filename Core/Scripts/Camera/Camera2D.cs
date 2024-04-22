using UnityEngine;

namespace Roguelike.Core
{
    public class Camera2D : GameCamera
    {
        protected override void ProcessFollowTarget(GameObject target)
        {
            _position = new Vector3(target.transform.position.x, target.transform.position.y, Zoom);
        }

        protected override void ProcessZoom()
        {
            if(_zoomFlag)
            {
                _zoomTick += Time.fixedDeltaTime;
                if(_zoomTick > _zoomDuration)
                {
                    _zoomTick = 0f;
                    _zoomFlag = false;
                }
                float time = _zoomTick / _zoomDuration;
                time = Mathf.Clamp01(time);
                _zoom = Mathf.Lerp(_zoom, _targetZoom, time);
            }
        }
    }
}
