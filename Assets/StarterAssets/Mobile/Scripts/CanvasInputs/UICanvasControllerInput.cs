using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            if (starterAssetsInputs)
            {
                starterAssetsInputs.MoveInput(virtualMoveDirection);
            }
            
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            if(starterAssetsInputs)
            {
                starterAssetsInputs.LookInput(virtualLookDirection);
            }
           
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            if (starterAssetsInputs)
            {
                starterAssetsInputs.JumpInput(virtualJumpState);
            }
            
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            if (starterAssetsInputs)
            {
                starterAssetsInputs.SprintInput(virtualSprintState);
            }
            
        }
        
    }

}
