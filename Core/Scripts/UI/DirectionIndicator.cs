using UnityEngine;

namespace Roguelike.Core
{
    public class DirectionIndicator : MonoBehaviour
    {
        [SerializeField] private Actor owner;
        [SerializeField] private float radius;
        [SerializeField] private Vector3 offset;

        private void Awake()
        {
            owner = GetComponentInParent<Actor>();
        }

        private void FixedUpdate()
        {
            Vector3 ownerDirection = owner.Direction;
            if (ownerDirection == Vector3.zero)
            {
                ownerDirection = Vector3.right;
            }
            Vector3 localPos = ownerDirection * radius;
            localPos += offset;
            float ownerAngle = Vector3.Angle(Vector3.right, ownerDirection);
            if (ownerDirection.y < 0f)
            {
                ownerAngle = 360f - ownerAngle;
            }
            Quaternion localRot = Quaternion.Euler(0f, 0f, ownerAngle);
            transform.localPosition= localPos;
            transform.localRotation = localRot;
            //transform.SetLocalPositionAndRotation(localPos, localRot);
        }
    }
}
