using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    private CharacterController _characterController;
    public Animator ThirdPersonAnimator;


    [Header("Movement Stats")]
    public float MovementSpeed = 5f;
    public float RunningSpeed = 10f;
    public float JumpForce = 5f;

    public float MovementModifier = 1f;
    public float CrouchingModifier = 1f;

    [Header("Air Stats")]
    public float AirModifier = 1f;
    public float MidAirDampening = 1f;

    [Header("Controller Settings")]
    public float StandingHeight = 1.8f;
    public float CrouchingHeight = 0.9f;

    private Vector3 velocity;

    public CharacterStates states;

    private Vector3 movementVec;
    private Vector3 motion;
    private float groundedCooldown;


    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool sprint = Input.GetKey(KeyCode.LeftShift);

        states.IsSprinting = CanSprint ? sprint : false;


        if (Input.GetKeyDown(KeyCode.Space))
            DoJump();

        HandleGravity();
        HandleCrouching();
        SyncAnimationStates();
        DoMovement(h, v, sprint);

    }

    private void SyncAnimationStates()
    {
        ThirdPersonAnimator.SetBool("Crouching", states.IsCrouching);
        ThirdPersonAnimator.SetBool("Sprinting", states.IsSprinting);
    }

    private void HandleGravity()
    {
        groundedCooldown -= Time.deltaTime;


        //If the Character is collided below
        if (_characterController.collisionFlags.HasFlag(CollisionFlags.Below))
        {
            if (groundedCooldown <= 0)
            {
                velocity = Vector3.down * 0.1f;
                states.IsGrounded = true;
            }
        }
        else
        {
            //Add Gravity forces to Velocity.
            velocity += Physics.gravity * Time.deltaTime;
            states.IsGrounded = false;
        }
    }

    //Handle Jump
    private void DoJump()
    {
        if (states.IsGrounded)
        {
            velocity.x = (motion.x) / Time.deltaTime;
            velocity.z = (motion.z) / Time.deltaTime;

            groundedCooldown = 0.05f;
            velocity.y = JumpForce;
        }

    }

    private void HandleCrouching()
    {
        _characterController.height = states.IsCrouching ? CrouchingHeight : StandingHeight;
        Vector3 center = _characterController.center;

        center.y = (states.IsCrouching ? -CrouchingHeight : 0) * 0.5f;

        _characterController.center = center;

        bool crouch = Input.GetKey(KeyCode.C);

        if (crouch && !states.IsCrouching && CanCrouch)
            states.IsCrouching = true;


        if (states.IsCrouching && !crouch && CanStandUp())
            states.IsCrouching = false;

        
    }


    //Movement function
    private void DoMovement(float h, float v, bool sprint)
    {
        //Convert Inputs to Vector
        movementVec = new Vector3(h, 0, v);
        //Localise Vector
        movementVec = transform.TransformDirection(movementVec);

        //Overriding Movement Speed if sprinting.
        float modifiedSpeed = GetMovementSpeed();

        motion = (velocity * Time.deltaTime);

        //Direct movement if grounded, or velocity based movement if mid
        if (states.IsGrounded)
            motion += (movementVec * modifiedSpeed * Time.deltaTime);
        else
        {
            velocity += movementVec * modifiedSpeed * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, 0f, MidAirDampening * Time.deltaTime);
            velocity.z = Mathf.MoveTowards(velocity.z, 0f, MidAirDampening * Time.deltaTime);
        }

        _characterController.Move(motion);
        ThirdPersonAnimator.SetFloat("Strafe X", transform.InverseTransformDirection(motion).normalized.x * 5f, 0.01f, Time.deltaTime);
        ThirdPersonAnimator.SetFloat("Strafe Y", transform.InverseTransformDirection(motion).normalized.z * 5f, 0.01f, Time.deltaTime);
    }


    //Rotate whole player instead of Camera.
    public void Rotate(float h)
    {
        _characterController.transform.Rotate(Vector3.up * h);
    }

    //Check conditions to see if the character can sprint.
    public bool CanSprint
    {
        get
        {
            return states.IsGrounded;
        }
    }

    public bool CanCrouch
    {
        get
        {
            return states.IsGrounded;
        }
    }

    public bool CanStandUp()
    {
        RaycastHit hit;

        Vector3 center = transform.position + (_characterController.center * 2);


        if (Physics.Raycast(new Ray(center, Vector3.up), out hit, StandingHeight))
        {
            Debug.Log(hit.transform.name);
            Debug.DrawRay(center, Vector3.up * StandingHeight, Color.green, 1f);
            return false;
        }
        else
        {
            Debug.DrawRay(center, Vector3.up * StandingHeight, Color.green, 1f);
            return true;
        }

    }

    //Get movement speed from states
    public float GetMovementSpeed()
    {
        float speed = MovementSpeed;

        if (states.IsSprinting)
            speed = RunningSpeed;

        if (states.IsCrouching)
            speed *= CrouchingModifier;

        if (!states.IsGrounded)
            speed *= AirModifier;

        return speed * MovementModifier;
    }
}
