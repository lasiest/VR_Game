using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class TeleportationManager : MonoBehaviour
{
    public InputActionProperty joystickUp;
    public GameObject ray;    

    void Update()
    {
        if(joystickUp.action.ReadValue<Vector2>().y > 0){
            Debug.Log("JoyStick Keatas");
            ray.SetActive(true);
        }else{
            ray.SetActive(false);
        }        
    }



}
