using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class playerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5, mouseSensX = 2, mouseSensY = 2;
    private float camRotY = 45;

    
    private int groundLayer;

    private playerPhysics playerPhysics;
    [SerializeField]
    private Animator animator;


    //Input
    private float xInput, yInput, mouseXRot, mouseYRot;
    private bool jump;

    // Start is called before the first frame update
    void Start()
    {
        playerPhysics = GetComponent<playerPhysics>();

        Cursor.lockState = CursorLockMode.Locked;
        groundLayer = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = transform.right * xInput;
        Vector3 moveVertical = transform.forward * yInput;

        Vector3 velocity = (moveHorizontal + moveVertical) * speed;

        //Animations
        animator.SetFloat("x", xInput);
        animator.SetFloat("y", yInput);

        playerPhysics.Move(velocity);

        //Rotation
        mouseXRot = Input.GetAxis("Mouse X") * 20 * Time.fixedDeltaTime;
        mouseYRot = Input.GetAxis("Mouse Y") * 20 * Time.fixedDeltaTime;

        Vector3 rot = new Vector3(0, mouseXRot, 0) * mouseSensX;
        camRotY = mouseYRot * mouseSensY;
        
        playerPhysics.Rotate(rot);
        playerPhysics.RotateCam(camRotY);

        //Jump
        jump = Input.GetButtonDown("Jump");

        playerPhysics.Jump(jump);


    }

}
