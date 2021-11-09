using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    protected Vector2 velocity;

    public float gravityModifier = 1f;
    
    protected ContactFilter2D contactFilter;
    protected Rigidbody2D rb2d;
    protected const float minMoveDistance = 0.001f;
    protected RaycastHit2D[] = hitBuffer = new RaycastHit2D[16];
    protected const float shellRadius = 0.01f;

    void OnEnable(){
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 move = Vector2.up *deltaPosition.y;

        Movement(move);
    }

    void Movement(Vector2 move){
        float distance = move.magnitude;
        if(distance > minMoveDistance){
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
        }

        rb2d.position = rb2d.position + move;
    }
}
