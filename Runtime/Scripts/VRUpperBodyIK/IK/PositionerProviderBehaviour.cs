using UnityEngine;

namespace VRUpperBodyIK.IK
{
    public abstract class PositionerProviderBehaviour : MonoBehaviour, PositionerProvider
    {
        public abstract Positioner Positioner { get; }
    }
}
