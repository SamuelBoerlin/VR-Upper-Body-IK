using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRUpperBodyIK.IK;
using VRUpperBodyIK.Skeleton;

namespace VRUpperBodyIK.Evaluation
{
    public class EvaluationController : MonoBehaviour
    {
        [Tooltip("Root transform of the rig. Used e.g. to reset the rig position after an animation.")]
        public Transform rootTransform;

        [Tooltip("Skeleton solver to be used to do the IK solving.")]
        public SkeletonSolver solver;

        [Tooltip("Animator of the rig.")]
        public Animator animator;

        [Tooltip("Animation trigger for playing the calibration clip.")]
        public string calibrationAnimationTrigger;

        [Tooltip("Root transform position is set to this value when calibration clip is playing.")]
        public Vector3 calibrationPosition = Vector3.zero;

        [Tooltip("Root transform rotation is set to this value when calibration clip is playing.")]
        public Quaternion calibrationRotation = Quaternion.identity;

        [Tooltip("Animation triggers to be used for the evaluation.")]
        public string[] evaluationAnimationTriggers;

        public ManualBodyCalibration bodyCalibration;

        public RootMeanSquareError rmse;

        public bool start = false;

        private bool running = false;
        private int simulationFps = 60;

        private void Start()
        {
            if (solver != null)
            {
                solver.enabled = false;
            }

            if (animator != null)
            {
                animator.speed = 0.0f;
            }

            if (rmse != null)
            {
                rmse.enabled = false;
            }
        }

        private void Update()
        {
            if (start)
            {
                start = false;

                if (!running && solver != null && animator != null)
                {
                    StartCoroutine(Evaluate());
                }
            }
        }

        private IEnumerator Evaluate()
        {
            running = true;

            foreach (string trigger in evaluationAnimationTriggers)
            {
                if (!enabled || !running)
                {
                    break;
                }

                yield return Calibrate();

                Debug.Log("Starting animation: " + trigger);

                if (rmse != null)
                {
                    rmse.ResetEvaluation();
                }

                var clipInfo = PoseAtTime(trigger, 0.0f);

                var numFrames = Mathf.CeilToInt(clipInfo.clip.length * simulationFps);

                for (int i = 0; i < numFrames; ++i)
                {
                    float time = 1.0f / numFrames * i;

                    PoseAtTime(trigger, time);

                    yield return new WaitForEndOfFrame();

                    solver.SolveAndApply();

                    if (rmse != null)
                    {
                        rmse.AddSample();
                    }
                }

                if (rmse != null)
                {
                    Debug.Log("Shoulder error (cm): " + (rmse.ShoulderError * 100.0f));
                    Debug.Log("Elbow error (cm): " + (rmse.ElbowError * 100.0f));
                }
            }

            running = false;
        }
        private IEnumerator Calibrate()
        {
            PoseAtTime(calibrationAnimationTrigger, 0.0f);

            solver.SolveAndApply();

            yield return new WaitForEndOfFrame();

            if (rootTransform != null)
            {
                rootTransform.position = calibrationPosition;
                rootTransform.rotation = calibrationRotation;
            }

            if (bodyCalibration != null)
            {
                bodyCalibration.Calibrate();
            }
        }

        private AnimatorClipInfo PoseAtTime(string state, float time)
        {
            animator.Rebind();

            animator.Play(state, -1, time);
            animator.Update(0);

            var clipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];

            animator.StopPlayback();

            return clipInfo;
        }
    }
}
