using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ActiveTeleportationScript : MonoBehaviour
{
        public GameObject leftTeleportation;        
        public GameObject rightTeleportation;
        public InputActionProperty leftActivate;
        public InputActionProperty rightActivate;
        public InputActionProperty leftCancel; 
        public InputActionProperty rightCancel;

        // public XRRayInteractor rightRay; 
    void Update()
    {
        leftTeleportation.SetActive(leftCancel.action.ReadValue<float>() == 0 && leftActivate.action.ReadValue<float>() > 0.1f);
        rightTeleportation.SetActive(rightCancel.action.ReadValue<float>() == 0 && rightActivate.action.ReadValue<float>() > 0.1f);
        // bool isRIghtRayHovering = rightRay.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNormal, out int rightNumber, out bool rightValid);
        
        // rightTeleportation.SetActive(!isRIghtRayHovering && rightCancel.action.ReadValue<float>() == 0 && rightActive.action.ReadValue<float>() > 0.1f);
    }
}
