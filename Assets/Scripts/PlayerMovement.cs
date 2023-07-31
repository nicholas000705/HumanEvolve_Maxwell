using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float horizontal;
    private bool facingRight = true;
    private bool IsCrouching = false;
    private bool isAttacking = false;
    private bool isGrounded = false;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform headCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider crouchDisableCollider;
    [SerializeField] public Animator animator;

    [SerializeField] public float speed = 400f;
    [SerializeField] public float jumpForce = 300f;
    [SerializeField] public float exJumpRate = 0.5f;
    [SerializeField] public float crouchSpeedRate = 0.4f;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {

        isAttacking = GetComponent<PlayerAttack>().isAttacking;
        //Horizontal Movement
        horizontal = Input.GetAxisRaw("Horizontal");
        //animator.SetBool("isCrouching", IsCrouching);

        //Move and Ilde animations
        //animator.SetFloat("speed",Mathf.Abs(horizontal));

        //Flip player object
        Flip();

        //Jumping
        if (Input.GetButtonDown("Jump") && isGrounded && !IsCrouching && !isAttacking){
            //Change velocity of player object
            rb.velocity = new Vector2(rb.velocity.x, jumpForce*Time.fixedDeltaTime);
            //animator.SetBool("isJumping", true);
        }
        else if (isGrounded){
            //animator.SetBool("isJumping",false);
        }
        //Reduce jump foce while fast tapping
        if (Input.GetButtonUp("Jump") && rb.velocity.y>0f){
            rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y*exJumpRate);
            
        }

        //Crouch
        if ((Input.GetAxisRaw("Vertical")<0) && isGrounded && !isAttacking){
            IsCrouching = true;
            Debug.Log("Is crouching");
            //Disable upper collider while crouching
            if (crouchDisableCollider != null){
                crouchDisableCollider.enabled = false;
            }
        }
        else {
            IsCrouching = false;
            if (crouchDisableCollider != null){
                crouchDisableCollider.enabled = false;
            }     

        }
    }

    void FixedUpdate()
    {
        //Movement of object (Speed depends of crouching condition)
        if (!isAttacking){
            if (IsCrouching){
                rb.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime * crouchSpeedRate, rb.velocity.y);
            }
            else{
                rb.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rb.velocity.y);
            }
        }

    }

    private void Flip(){
        if (!isAttacking && (facingRight && horizontal < 0f || !facingRight && horizontal > 0f) ){
            facingRight = !facingRight;
            //Vector3 localScale = transform.localScale;
            //localScale.x *= -1f;
            //transform.localScale = localScale;

       

            transform.Rotate(0f,180f,0f);
        }
    }
    void OnCollisionEnter(Collision obj)
    {
        if (((1<<obj.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision obj)
{
    if (((1<<obj.gameObject.layer) & groundLayer) != 0)
    {
        isGrounded = false;
    }
}

}