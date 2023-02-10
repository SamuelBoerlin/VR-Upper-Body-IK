using System.Collections.Generic;
using UnityEngine;

namespace VRUpperBodyIK.Evaluation
{
    public class RootMeanSquareError : MonoBehaviour
    {
        public Skeleton.Skeleton skeleton1, skeleton2;

        private List<float> shoulderErrors = new();
        private List<float> elbowErrors = new();

        private void FixedUpdate()
        {
            var pose1 = skeleton1.CalibratedWorldPose;
            var pose2 = skeleton2.CalibratedWorldPose;

            var leftArm1 = pose1.leftArm;
            var rightArm1 = pose1.rightArm;

            var leftArm2 = pose2.leftArm;
            var rightArm2 = pose2.rightArm;

            shoulderErrors.Add((leftArm1.shoulderPosition - leftArm2.shoulderPosition).sqrMagnitude);
            shoulderErrors.Add((rightArm1.shoulderPosition - rightArm2.shoulderPosition).sqrMagnitude);

            elbowErrors.Add((leftArm1.elbowPosition - leftArm2.elbowPosition).sqrMagnitude);
            elbowErrors.Add((rightArm1.elbowPosition - rightArm2.elbowPosition).sqrMagnitude);

            Debug.Log("--- RMSE (cm) ----");
            Debug.Log("shoulder: " + RMSE(shoulderErrors) * 100.0f);
            Debug.Log("elbow: " + RMSE(elbowErrors) * 100.0f);
        }

        private double RMSE(List<float> errors)
        {
            if (errors.Count == 0)
            {
                return 0.0f;
            }
            float total = 0.0f;
            foreach (float error in errors)
            {
                total += error;
            }
            return Mathf.Sqrt(total / errors.Count);
        }
    }
}