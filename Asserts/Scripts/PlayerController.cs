using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _character;
    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private float _rotateSpeed = 100.0f;
    public GameObject weaponPrefab;
    public Transform leftHandTransform;
    public GrabInteractor rightHand;
    public Transform rightHandTransform;
    public LineRenderer positionSelectLine;
    private bool _isHoldingWeapon = false;
    public Transform forwardDirection;

    // Start is called before the first frame update
    void Start()
    {
        _character = GetComponent<CharacterController>();
        rightHand.WhenStateChanged += args =>
        {
            Debug.Log("State changed to " + args.NewState);
            switch (args.NewState)
            {
                case InteractorState.Select:
                case InteractorState.Hover:
                    _isHoldingWeapon = true;
                    break;
                case InteractorState.Normal:
                case InteractorState.Disabled:
                    _isHoldingWeapon = false;
                    break;
            }
        };
        positionSelectLine.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        var inputDirection = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        var direction = new Vector3(inputDirection.x, 0, inputDirection.y) * Time.deltaTime;
        _character.Move((forwardDirection.TransformDirection(direction) + Vector3.down) * _moveSpeed);

        var inputRotation = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        var rotation = new Vector3(0, inputRotation.x, 0) * Time.deltaTime;
        transform.Rotate(rotation * _rotateSpeed);

        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) && !_isHoldingWeapon)
        {
            Debug.Log($"isHoldingWeapon {_isHoldingWeapon} Spawn weapon");
            var weapon = Instantiate(weaponPrefab);
            weapon.transform.position = rightHandTransform.position;
            weapon.transform.rotation = rightHandTransform.rotation;
            rightHand.SetComputeCandidateOverride(() => weapon.GetComponent<GrabInteractable>(), true);
        }
        
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Debug.Log("begin select position");
            positionSelectLine.enabled = true;
        }
        
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            Debug.Log("end select position");
            positionSelectLine.enabled = false;
            var ray = new Ray(leftHandTransform.position, leftHandTransform.forward);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    transform.position = hit.point;
                }
            }
        }
    }
}