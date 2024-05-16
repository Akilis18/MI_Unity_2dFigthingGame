using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region [Initalize]
    public bool boolInputMethod;

    private Animator playerANI;
    private AnimatorStateInfo playerSInfo;
    private Rigidbody2D playerRB;
    private SpriteRenderer playerSR;
    #endregion
 
    void Start()
    {
        playerANI = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();
    }
 

    void Update()
    {
        //Always get the state of the animator
        playerSInfo = playerANI.GetCurrentAnimatorStateInfo(0);

        if (!boolInputMethod)
        {
            #region [Input: Keyboard]
            //Change animator state by x velocity
            if (playerSInfo.IsName("Stand") || playerSInfo.IsName("Walk") || playerSInfo.IsName("Run"))
            {
                if (playerRB.velocity.x <= -3f)
                {
                    playerANI.Play("Run");
                    playerSR.flipX = true;
                }

                if (playerRB.velocity.x <= -0.1f && playerRB.velocity.x > -3f)
                {
                    playerANI.Play("Walk");
                    playerSR.flipX = true;
                }

                if (playerRB.velocity.x <= 0.1f && playerRB.velocity.x > -0.1f)
                {
                    playerANI.Play("Stand");
                }

                if (playerRB.velocity.x <= 3f && playerRB.velocity.x > 0.1f)
                {
                    playerANI.Play("Walk");
                    playerSR.flipX = false;
                }

                if (playerRB.velocity.x > 3f)
                {
                    playerANI.Play("Run");
                    playerSR.flipX = false;
                }
            }

            //Change animator state by y velocity
            if (playerSInfo.IsName("Jump"))
            {
                if (Mathf.Abs(playerRB.velocity.y) <= 0.00001f)
                {
                    playerANI.Play("Stand");
                }
            }

            //increase velocity when press L/R key down
            if (!playerSInfo.IsName("Jump"))
            {
                if (Input.GetKey(KeyCode.RightArrow) && playerRB.velocity.x <= 7f)
                {
                    playerRB.AddForce(new Vector2(1000f * Time.deltaTime, 0), ForceMode2D.Force);
                }
                if (Input.GetKey(KeyCode.LeftArrow) && playerRB.velocity.x > -7f)
                {
                    playerRB.AddForce(new Vector2(-1000f * Time.deltaTime, 0), ForceMode2D.Force);
                }
            }

            //Jump
            if (!playerSInfo.IsName("Jump") && Input.GetKeyDown(KeyCode.UpArrow))
            {
                playerANI.Play("Jump");
                playerRB.AddForce(new Vector2(0f, 7f), ForceMode2D.Impulse);
            }

            #endregion
        }
        else
        {
            //TODO: input by kinect
        }

    }
}
