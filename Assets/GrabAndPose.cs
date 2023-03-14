using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GrabAndPose : MonoBehaviour
{
    public float poseTransitionDuration = 0.2f;
    public HandData leftHandPose;
    public HandData rightHandPose;

    private Vector3 _startingHandPosition;
    private Vector3 _finalHandPosition;
    private Quaternion _startingHandRotation;
    private Quaternion _finalHandRotation;
    private Quaternion[] _startingFingerRotations;
    private Quaternion[] _finalFingerRotations;

    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnsetPose);        
        leftHandPose.gameObject.SetActive(false);
        rightHandPose.gameObject.SetActive(false);
    }

    public void SetupPose(BaseInteractionEventArgs arg){
        if(arg.interactorObject is XRDirectInteractor){
            HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = false;

            if(handData.handType == HandData.HandModelType.Left){
                SetHandDataValues(handData, leftHandPose);                
            }else{
                SetHandDataValues(handData, rightHandPose);
            }

            StartCoroutine(SetHandDataRoutine(handData, _finalHandPosition, _finalHandRotation, _finalFingerRotations, _startingHandPosition, _startingHandRotation, _startingFingerRotations));
        }
    }

    public void UnsetPose(BaseInteractionEventArgs arg){
        if(arg.interactorObject is XRDirectInteractor){
            HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = true;

            StartCoroutine(SetHandDataRoutine(handData, _startingHandPosition, _startingHandRotation, _startingFingerRotations, _finalHandPosition, _finalHandRotation, _finalFingerRotations));
        }
    }

    public void SetHandDataValues(HandData h1, HandData h2){
        _startingHandPosition = new Vector3(h1.root.localPosition.x / h1.root.localScale.x, h1.root.localPosition.y / h1.root.localScale.y, h1.root.localPosition.z / h1.root.localScale.z);
        _finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x, h2.root.localPosition.y / h2.root.localScale.y, h2.root.localPosition.z / h2.root.localScale.z);

        _startingHandRotation = h1.root.localRotation;
        _finalHandRotation = h2.root.localRotation;

        _startingFingerRotations = new Quaternion[h1.fingerBones.Length];
        _finalFingerRotations = new Quaternion[h2.fingerBones.Length];

        for(int i = 0; i < h1.fingerBones.Length; i++){
            _startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            _finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    public void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation){
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;
        for(int i = 0; i< newBonesRotation.Length; i++){
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }

    public IEnumerator SetHandDataRoutine(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation, Vector3 startingPosition, Quaternion startingRotation, Quaternion[] startingBonesRotation){
        float timer = 0;

        while(timer < poseTransitionDuration){

            Vector3 p = Vector3.Lerp(startingPosition, newPosition, timer / poseTransitionDuration);
            Quaternion r = Quaternion.Lerp(startingRotation, newRotation, timer / poseTransitionDuration);

            h.root.localPosition = p;
            h.root.localRotation = r;

            for(int i = 0; i < newBonesRotation.Length; i++){
                h.fingerBones[i].localRotation = Quaternion.Lerp(startingBonesRotation[i], newBonesRotation[i], timer / poseTransitionDuration);
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

#if UNITY_EDITOR

    [MenuItem("Tools/Mirror Selected Left Grab Pose")]
    public static void MirrorLeftPose(){
        Debug.Log("Mirror Left Pose");
        GrabAndPose handPose = Selection.activeGameObject.GetComponent<GrabAndPose>();
        handPose.MirrorPose(handPose.rightHandPose, handPose.leftHandPose);
    }

#endif

    public void MirrorPose(HandData poseToMirror, HandData poseUsedToMirror){
        Vector3 mirroredPosition = poseUsedToMirror.root.localPosition;
        mirroredPosition.x *= -1;

        Quaternion mirroredQuaternion = poseUsedToMirror.root.localRotation;
        mirroredQuaternion.y *= -1;
        mirroredQuaternion.z *= -1;

        poseToMirror.root.localPosition = mirroredPosition;
        poseToMirror.root.localRotation = mirroredQuaternion;

        for(int i = 0; i < poseUsedToMirror.fingerBones.Length; i++){
            poseToMirror.fingerBones[i].localRotation = poseUsedToMirror.fingerBones[i].localRotation;
        }
    }
}
