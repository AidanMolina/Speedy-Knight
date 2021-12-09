using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumper : MonoBehaviour
{
     PlayerPlatformerController player;
     float speed;
     float currentJump;

     private CapsuleCollider2D capsule;
     bool jumped;
     float offTime = 0.5f;
     bool stuck;

    void Awake(){
        capsule = GetComponent<CapsuleCollider2D>();
        stuck = false;
        jumped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(stuck && Input.GetKeyDown(KeyCode.W)){
            player.velocity.y = currentJump;
            jumped = true;
            capsule.enabled = !capsule.enabled;
            player.jumpsLeft -= 1;
        }
        
        if(jumped == true){
            offTime -= Time.deltaTime;
            if(offTime <= 0.0f){
                jumped = false;
                capsule.enabled = !capsule.enabled;
                offTime = 0.5f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            player = collider.gameObject.GetComponent<PlayerPlatformerController>();
            speed = player.speedForJump;
            currentJump = player.jumpSpeedForJump * speed/3;
            if(player.jumpsLeft == 0){
                player.jumpsLeft = speed/2;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collider){
        if(collider.gameObject.tag == "Player" && player.jumpsLeft >= 1){
            player.velocity.y = 0f;
            stuck = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            stuck = false;
        }
    }
}
