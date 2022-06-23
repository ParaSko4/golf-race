using System.Collections.Generic;
using UnityEngine;

namespace GolfRace.Gameplay.Car
{
    [RequireComponent(typeof(LineRenderer))]
    public class TrajectoryMovement : MonoBehaviour
    {
        private class DirectionData
        {
            public Vector3 direction;
            public Vector3 endPoint;
            public float distance;
        }

        [SerializeField]
        private int reflectionsCount;
        [SerializeField]
        private float maxLineLength;
        [SerializeField]
        private float rotationSpeed;

        private LineRenderer lineRenderer;
        private Queue<DirectionData> directionMovementOrder = new Queue<DirectionData>();

        public Vector3 CurrentPath { get; private set; }
        public Vector3 CurrentEndPoint { get; private set; }
        public float CurrentDistance { get; private set; }
        public int DirectionCount { get => directionMovementOrder.Count; }

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        public void ShowTrajectory()
        {
            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, transform.position);

            RaycastHit raycastHit;
            var ray = new Ray(transform.position, transform.forward);
            float remainingDistance = maxLineLength;

            for (int i = 0; i < reflectionsCount; i++)
            {
                if (remainingDistance <= 0f)
                {
                    break;
                }

                lineRenderer.positionCount += 1;

                if (Physics.Raycast(ray, out raycastHit, remainingDistance, ~(1 << 2)))
                {
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, raycastHit.point);

                    remainingDistance -= Vector3.Distance(ray.origin, raycastHit.point);
                    ray = new Ray(raycastHit.point, Vector3.Reflect(ray.direction, raycastHit.normal));
                }
                else
                {
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingDistance);
                    break;
                }
            }
        }

        public void FollowTrajectory(Vector3 direction)
        {
            var rotation = Quaternion.LookRotation(direction);
            var correctRotation = new Quaternion(0f, rotation.y, 0f, rotation.w);

            transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, correctRotation, rotationSpeed);
        }

        public void FixTrajectoryDirections()
        {
            directionMovementOrder.Clear();

            var points = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(points);

            for (int i = 1; i < lineRenderer.positionCount; i++)
            {
                var directionData = new DirectionData();
                directionData.direction = (points[i] - points[i - 1]).normalized;
                directionData.endPoint = new Vector3(points[i].x, transform.parent.position.y, points[i].z);
                directionData.distance = Vector3.Distance(points[i - 1], points[i]);

                directionMovementOrder.Enqueue(directionData);
            }

            lineRenderer.positionCount = 0;
        }

        public void StopMovement()
        {
            directionMovementOrder.Clear();
        }

        public bool GetNextDirection()
        {
            if (directionMovementOrder.Count == 0)
            {
                return false;
            }

            var directionData = directionMovementOrder.Dequeue();
            var correctionDirection = new Vector3(directionData.direction.x, 0f, directionData.direction.z);
            CurrentPath = transform.parent.position + correctionDirection * directionData.distance;
            CurrentEndPoint = directionData.endPoint;
            CurrentDistance = directionData.distance;

            return true;
        }
    }
}