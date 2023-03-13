using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ActiveTeleportationScript : MonoBehaviour
{
        public GameObject rightTeleportation;
        public InputActionProperty rightActive;
        public InputActionProperty rightCancel;

        public XRRayInteractor rightRay; 
    void Update()
    {
        bool isRIghtRayHovering = rightRay.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNormal, out int rightNumber, out bool rightValid);
        
        rightTeleportation.SetActive(!isRIghtRayHovering && rightCancel.action.ReadValue<float>() == 0 && rightActive.action.ReadValue<float>() > 0.1f);
    }
}
