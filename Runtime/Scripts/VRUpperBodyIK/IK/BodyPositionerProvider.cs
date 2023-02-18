using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRUpperBodyIK.IK
{
    public class BodyPositionerProvider : PositionerProviderBehaviour
    {
        [SerializeField] private float headNeckDistance = 0.18f;
        [SerializeField] private float neckShoulderDistance = 0.17f;

        [SerializeField] private float handElbowDistance = 0.42f;
        [SerializeField] private float elbowShoulderDistance = 0.26f;

        [SerializeField] private float height = 0.0f;

        public float HeadNeckDistance
        {
            get => headNeckDistance;
            set => SetAndNotify(ref headNeckDistance, value);
        }

        public float NeckShoulderDistance
        {
            get => neckShoulderDistance;
            set => SetAndNotify(ref neckShoulderDistance, value);
        }

        public float HandElbowDistance
        {
            get => handElbowDistance;
            set => SetAndNotify(ref handElbowDistance, value);
        }

        public float ElbowShoulderDistance
        {
            get => elbowShoulderDistance;
            set => SetAndNotify(ref elbowShoulderDistance, value);
        }

        public float Height
        {
            get => height;
            set => SetAndNotify(ref height, value);
        }


        public UnityEvent OnSettingsChanged;

        private readonly BodyPositioner bodyPositioner = new();

        public override Positioner Positioner => bodyPositioner;

        private void Awake()
        {
            ApplySettings();
        }

        private void Update()
        {
            ApplySettings();
        }

        private void ApplySettings()
        {
            bodyPositioner.Settings.HeadNeckDistance = headNeckDistance;
            bodyPositioner.Settings.NeckShoulderDistance = neckShoulderDistance;
            bodyPositioner.Settings.HandElbowDistance = handElbowDistance;
            bodyPositioner.Settings.ElbowShoulderDistance = elbowShoulderDistance;
            bodyPositioner.Settings.Height = height;
        }

        private void SetAndNotify<T>(ref T prop, T val)
        {
            if (!EqualityComparer<T>.Default.Equals(prop, val))
            {
                prop = val;
                OnSettingsChanged?.Invoke();
            }
        }
    }
}
