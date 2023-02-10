using UnityEngine;

namespace VRUpperBodyIK.IK
{
    public class ElbowPositionerProvider : PositionerProviderBehaviour
    {
        public bool isLeftArm;

        private Positioner _positioner;
        public override Positioner Positioner
        {
            get
            {
                return _positioner ??= new ElbowPositioner(isLeftArm);
            }
        }
    }
}
