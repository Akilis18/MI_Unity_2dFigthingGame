using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    #region [Initalize]
    public bool boolInputMethod;
    public Image HPbar;
    private int HP;

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

        HPbar.fillAmount = 1;
        HP = 100;
    }
 

    void Update()
    {
        if (HP == 0)
        {
            Debug.Log("Game over");
        }

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
            if (!playerSInfo.IsName("Jump") && !playerSInfo.IsName("Duck"))
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
            else
            {
                //Change the direction of player
                if (playerRB.velocity.x > 0.1f)
                    playerSR.flipX = false;

                if (playerRB.velocity.x < -0.1f)
                    playerSR.flipX = true;

                //Change velocity when get key
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    playerRB.velocity = new Vector2(5f, playerRB.velocity.y);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    playerRB.velocity = new Vector2(-5f, playerRB.velocity.y);
                }
            }

            //Jump
            if (!playerSInfo.IsName("Jump") && Input.GetKeyDown(KeyCode.UpArrow))
            {
                playerANI.Play("Jump");
                playerRB.AddForce(new Vector2(0f, 7f), ForceMode2D.Impulse);
            }

            //Duck
            if (!playerSInfo.IsName("Duck") && Input.GetKeyDown(KeyCode.DownArrow))
            {
                playerANI.Play("Duck");
                playerRB.AddForce(new Vector2(0f, -7f), ForceMode2D.Impulse);
            }
            if (playerSInfo.IsName("Duck") && Input.GetKeyUp(KeyCode.DownArrow))
            {
                playerANI.Play("Stand");
            }

            //Attack
            if ((!playerSInfo.IsName("Chop") && !playerSInfo.IsName("CutU")) && Input.GetKeyDown(KeyCode.Space))
            {
                int moveChoice = Random.Range(1, 3);
                switch (moveChoice)
                {
                    case 1:
                        playerANI.Play("Chop");
                        break;
                    case 2:
                        playerANI.Play("CutU");
                        break;
                }
            }

            #endregion
        }
        else
        {
            //TODO: input by kinect
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            playerRB.position = new Vector3(0f, 0f);
            Camera.main.gameObject.transform.position = new Vector3(0f, 0f, -10f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.layer != LayerMask.NameToLayer("Attack"))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Attack"))
            {
                if (collision.gameObject.tag == "Enemy")
                {
                    Debug.Log("Hit!!");
                    HP -= 10;
                    HPbar.fillAmount = HP / 100f;
                }
            }
        }

    }
}
