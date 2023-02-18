using UnityEngine;

namespace VRUpperBodyIK.Skeleton
{
    [RequireComponent(typeof(Skeleton))]
    public class SkeletonRenderer : MonoBehaviour
    {
        public LineRenderer lineRendererPrefab;

        public bool useCalibratedPose = true;

        private Skeleton skeleton;
        private LineRenderer[] lineRenderers = new LineRenderer[4];

        private void Awake()
        {
            skeleton = GetComponent<Skeleton>();
        }

        private void OnEnable()
        {
            for (int i = 0; i < 4; ++i)
            {
                lineRenderers[i] = Instantiate(lineRendererPrefab, transform);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < 4; ++i)
            {
                if (lineRenderers[i] != null)
                {
                    Destroy(lineRenderers[i].gameObject);
                }
            }
        }

        private void LateUpdate()
        {
            var pose = useCalibratedPose ? skeleton.CalibratedWorldPose : skeleton.UncalibratedWorldPose;

            ApplyForArm(pose.leftArm, 0);
            ApplyForArm(pose.rightArm, 2);
        }

        private void ApplyForArm(Arm arm, int index)
        {
            if (lineRenderers[index + 0] != null)
            {
                lineRenderers[index + 0].positionCount = 2;
                lineRenderers[index + 0].SetPosition(0, arm.shoulderPosition);
                lineRenderers[index + 0].SetPosition(1, arm.elbowPosition);
            }

            if (lineRenderers[index + 1] != null)
            {
                lineRenderers[index + 1].positionCount = 2;
                lineRenderers[index + 1].SetPosition(0, arm.elbowPosition);
                lineRenderers[index + 1].SetPosition(1, arm.handPosition);
            }
        }
    }
}
