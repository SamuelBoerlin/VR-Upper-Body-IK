using UnityEngine;

namespace VRUpperBodyIK.IK
{
    public class Solver : MonoBehaviour
    {
        [Tooltip("Skeleton to be solved with IK. Head and hand should already be positioned appropriately.")]
        public Skeleton.Skeleton skeleton;

        public bool runSolver;

        private ShoulderPositioner shoulderPositioner = new();

        private ElbowPositioner leftElbowPositioner = new(true);
        private ElbowPositioner rightElbowPositioner = new(false);

        private void FixedUpdate()
        {
            if (runSolver)
            {
                var pose = skeleton.CalibratedWorldPose;

                var settings = new BodySettings();

                settings.HeadNeckDistance = 0.18f;
                settings.NeckShoulderDistance = 0.17f;
                settings.HandElbowDistance = 0.42f;
                settings.ElbowShoulderDistance = 0.26f;

                shoulderPositioner.Update(pose, settings);

                leftElbowPositioner.Update(pose, settings);
                rightElbowPositioner.Update(pose, settings);

                ApplyPoseToChain(pose);
            }
        }

        protected void ApplyPoseToChain(Skeleton.Pose pose)
        {
            skeleton.leftShoulder.position = pose.leftArm.shoulderPosition;
            skeleton.leftElbow.position = pose.leftArm.elbowPosition;

            skeleton.rightShoulder.position = pose.rightArm.shoulderPosition;
            skeleton.rightElbow.position = pose.rightArm.elbowPosition;
        }
    }
}