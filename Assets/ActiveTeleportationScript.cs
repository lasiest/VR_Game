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
    void Update()
    {
        rightTeleportation.SetActive(rightCancel.action.ReadValue<float>() == 0 && rightActive.action.ReadValue<float>() > 0.1f);
    }
}
