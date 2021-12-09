using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    int health = 3;
    public float jumpsLeft = 0f;
    float attackPower = 1f;

    public float currentSpeed = 5f;
    public float maxSpeed = 8f;
    public float jumpTakeOffSpeed = 3f;

    public float speedForJump;
    public float jumpSpeedForJump;

    bool ticked;
    float tick = 0.35f;

    bool attacking;
    float attackTimer = 0.5f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider;

    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        ticked = false;
        attacking = false;
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

        if(Input.GetKeyDown(KeyCode.W) && grounded){
            velocity.y = jumpTakeOffSpeed * currentSpeed/3;
        }
        else if(Input.GetKeyUp(KeyCode.W)){
            if(velocity.y > 0f){
                velocity.y = velocity.y * 0.5f;
            }
        }

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if(flipSprite){
            spriteRenderer.flipX = !spriteRenderer.flipX;
            if(spriteRenderer.flipX == true){
                boxCollider.offset = new Vector2(-0.5f, 0.5f);
            }
            else{
                boxCollider.offset = new Vector2(0.55f, 0.5f);
            }
        }

        animator.SetBool("grounded", grounded);
        if(grounded){
            jumpsLeft = 0;
            speedForJump = currentSpeed;
            jumpSpeedForJump = jumpTakeOffSpeed;
            
        }
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / currentSpeed);
        
        targetVelocity = move * currentSpeed;


        if(Input.GetButtonDown("Jump") && attacking == false){
            boxCollider.enabled = !boxCollider.enabled;
            attacking = true;
        }

        if(attacking == true){
            attackTimer -= Time.deltaTime;
            if(attackTimer <= 0.0f){
                boxCollider.enabled = !boxCollider.enabled;
                attacking = false;
                attackTimer = 0.5f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Enemy"){
            collider.gameObject.GetComponent<Enemy>().health -= attackPower * currentSpeed;
            Debug.Log(collider.gameObject.GetComponent<Enemy>().health);
        }
    }
}
