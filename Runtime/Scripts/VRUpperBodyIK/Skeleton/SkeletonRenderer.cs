using UnityEngine;

namespace VRUpperBodyIK.Skeleton
{
    [RequireComponent(typeof(Skeleton))]
    public class SkeletonRenderer : MonoBehaviour
    {
        public LineRenderer lineRendererPrefab;

        public bool useCalibratedPose = false;

        private Skeleton skeleton;
        private LineRenderer[] lineRenderers = new LineRenderer[4];

        private void Awake()
        {
            skeleton = GetComponent<Skeleton>();

            for (int i = 0; i < 4; ++i)
            {
                lineRenderers[i] = Instantiate(lineRendererPrefab, transform);
            }
        }

        private void Update()
        {
            var pose = useCalibratedPose ? skeleton.CalibratedWorldPose : skeleton.UncalibratedWorldPose;

            ApplyForArm(pose.leftArm, 0);
            ApplyForArm(pose.rightArm, 2);
        }

        private void ApplyForArm(Arm arm, int index)
        {
            lineRenderers[index + 0].positionCount = 2;
            lineRenderers[index + 0].SetPosition(0, arm.shoulderPosition);
            lineRenderers[index + 0].SetPosition(1, arm.elbowPosition);

            lineRenderers[index + 1].positionCount = 2;
            lineRenderers[index + 1].SetPosition(0, arm.elbowPosition);
            lineRenderers[index + 1].SetPosition(1, arm.handPosition);
        }
    }
}
