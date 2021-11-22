using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(playerController))]
public class playerPhysics : MonoBehaviour
{
    private Vector3 velocity, rotation;
    private float camRotationY, wantedCamRot;
    private Rigidbody rb;
    [SerializeField]
    private Camera playerCam;
    private bool readyToJump = true, jump, isJumping;
    [SerializeField]
    private float jumpCooldown = 3, jumpForce = 500;
    [SerializeField]
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        AddCamRotation();
        AddRotation();

    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    public void RotateCam(float _camRot)
    {
        camRotationY = _camRot;
    }
    public void Jump(bool _jump)
    {
        if (_jump && readyToJump) { jump = _jump; };
    }

    private void FixedUpdate()
    {
        AddForces();
        
    }

    private void AddForces()
    {
        if (velocity != Vector3.zero)
        {
            if (!isJumping)
                rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            else
                rb.MovePosition(rb.position + velocity * 0.75f * Time.fixedDeltaTime);
        }

        if (jump && readyToJump)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            readyToJump = false;
            jump = false;
            isJumping = true;
            //JumpAnim
            animator.SetBool("Jump", true);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void AddRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));      
    }
    private void AddCamRotation()
    {
        wantedCamRot -= camRotationY;
        wantedCamRot = Mathf.Clamp(wantedCamRot, -90, 90);

        playerCam.transform.localEulerAngles = new Vector3(wantedCamRot, 0, 0);
    }

    private void ResetJump()
    {
        readyToJump = true;
        isJumping = false;
        //Jump Anim
        animator.SetBool("Jump", false);

    }

}
