using UnityEngine;

namespace VRUpperBodyIK.IK
{
    public class ShoulderPositionerProvider : PositionerProviderBehaviour
    {
        public override Positioner Positioner { get; } = new ShoulderPositioner();
    }
}
