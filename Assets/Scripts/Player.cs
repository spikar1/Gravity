using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{

    Controller2D controller;
    Animator anim;

    public Vector2 velocity;
    public LayerMask traps;

    float gravity = -0.5625f * 64;
    float maxFallSpeed = 18.75f;// 10;
    float maxSpeed = 11.25f;
    float jumpForce = 15.5f;
    float acceleration = .375f * 64;
    float deacceleration = .46875f * 64;

    public bool grounded;


    public bool gravityLock = false;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && !gravityLock)
        {
            gravity *= -1;
        }

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        if (controller.collisions.left || controller.collisions.right)
        {
            velocity.x = 0;
        }

        grounded = (gravity > 0) ? controller.collisions.above : controller.collisions.below;
        anim.SetBool("Grounded", grounded);


        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
            velocity.y = jumpForce * Mathf.Sign(-gravity);

        if (Input.GetKeyUp(KeyCode.UpArrow) && Mathf.Sign(gravity) != Mathf.Sign(velocity.y))
            velocity.y *= .5f;

        if (input.x != 0)
        {
            //if change direction, deaccelerate
            if (Mathf.Sign(input.x) != Mathf.Sign(velocity.x) && velocity.x != 0)
            {
                velocity.x = Mathf.MoveTowards(velocity.x, 0f, deacceleration * Time.deltaTime);
            }
            else //run, accelerate
            {

                if (Mathf.Abs(velocity.x) < maxSpeed)
                    velocity.x += acceleration * input.x * Time.deltaTime;
                else if (Mathf.Sign(input.x) == Mathf.Sign(velocity.x))
                    velocity.x = input.x * maxSpeed;
            }
        }
        else
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deacceleration * Time.deltaTime);

        //velocity.x = input.x * Time.deltaTime * speed;
        velocity.y += gravity * Time.deltaTime;
        if (Math.Abs(velocity.y) > maxFallSpeed)
            velocity.y = velocity.y.Sign() * maxFallSpeed;

        //print(velocity);
        controller.Move(velocity * Time.deltaTime);


        anim.SetInteger("InputX", (int)input.x);

        if (input.x != 0)
            transform.localScale = new Vector3(input.x, 1, 1);
        transform.localScale = new Vector3(transform.localScale.x, -Mathf.Sign(gravity), 1);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }

        if (Physics2D.OverlapArea(transform.position - controller.col.bounds.extents * .8f, transform.position + controller.col.bounds.extents * .8f, traps))
        {
            Scene scene = SceneManager.GetActiveScene();
            //SceneManager.UnloadScene(SceneManager.GetActiveScene().buildIndex); 
            SceneManager.LoadScene(scene.buildIndex);
        }
    }


}
