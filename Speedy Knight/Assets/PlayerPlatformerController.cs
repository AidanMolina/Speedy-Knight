using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    public float currentSpeed = 5f;
    public float maxSpeed = 10f;
    public float jumpTakeOffSpeed = 3f;

    bool ticked;
    float tick = 0.5f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        ticked = false;
    }

    protected override void ComputeVelocity(){
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal")/5 * currentSpeed;
        if(Input.GetAxis("Horizontal") != 0 && ticked == false){
            currentSpeed += 0.5f;
            if(currentSpeed > maxSpeed){
                currentSpeed = maxSpeed;
            }
            ticked = true;
        }
        if(ticked == true){
            tick -= Time.deltaTime;
            if(tick <= 0.0f){
                ticked = false;
                tick = 0.5f;
            }
        }

        if(Input.GetAxis("Horizontal") == 0){
            currentSpeed = 5f;
        }

        if(Input.GetButtonDown("Jump") && grounded){
            velocity.y = jumpTakeOffSpeed * currentSpeed/3;
        }
        else if(Input.GetButtonUp("Jump")){
            if(velocity.y > 0f){
                velocity.y = velocity.y * 0.5f;
            }
        }

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if(flipSprite){
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / currentSpeed);
        
        targetVelocity = move * currentSpeed;
        Debug.Log(currentSpeed);

    }
}
