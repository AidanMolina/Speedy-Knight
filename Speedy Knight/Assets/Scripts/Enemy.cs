using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 6f;
    public float speed = 1f;

    [SerializeField] GameObject point1;
    [SerializeField] GameObject point2;
    private GameObject[] movement;
    private int pointer = 0;

    bool moved;
    float waitTime = 1.0f;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = new GameObject[]{point1, point2};
        moved = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0){
            Destroy(point1);
            Destroy(point2);
            Destroy(gameObject);
        }

        if(moved == false){
            Move(movement[pointer].transform.position);
        }
        else{
            waitTime -= Time.deltaTime;
            if(waitTime <= 0.0f){
                moved = false;
                waitTime = 1.0f;
            }
        }

        if(pointer == 0){
            spriteRenderer.flipX = false;
        }
        else{
            spriteRenderer.flipX = true;
        }
    }

    void Move(Vector3 target){
        if(gameObject.activeSelf){
            float step =  speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            if(Vector3.Distance(transform.position, target) < 0.001f){
                if(pointer < 1){
                    pointer += 1;
                }
                else{
                    pointer = 0;
                }
                moved = true;
            }
        }
    }
}
