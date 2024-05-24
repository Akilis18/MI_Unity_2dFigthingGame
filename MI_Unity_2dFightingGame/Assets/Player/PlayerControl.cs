using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    #region [Initialize]
    public bool boolInputMethod;
    public Image HPbar;
    private int HP;

    private Animator playerANI;
    private AnimatorStateInfo playerSInfo;
    private Rigidbody2D playerRB;
    private SpriteRenderer playerSR;

    public MessageControl message;
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
        if (HP <= 0)
        {
            Debug.Log("Game over");
            SceneManager.LoadScene("GameOverScene");
        }

        //Always get the state of the animator
        playerSInfo = playerANI.GetCurrentAnimatorStateInfo(0);

        if (!boolInputMethod)
        {
            #region [Input: Keyboard]
            //Change animator state by x velocity
            if (playerSInfo.IsName("PlayerStand") || playerSInfo.IsName("PlayerWalk") || playerSInfo.IsName("PlayerRun"))
            {
                if (playerRB.velocity.x <= -3f)
                {
                    playerANI.Play("PlayerRun");
                    playerSR.flipX = true;
                }

                if (playerRB.velocity.x <= -0.1f && playerRB.velocity.x > -3f)
                {
                    playerANI.Play("PlayerWalk");
                    playerSR.flipX = true;
                }

                if (playerRB.velocity.x <= 0.1f && playerRB.velocity.x > -0.1f)
                {
                    playerANI.Play("PlayerStand");
                }

                if (playerRB.velocity.x <= 3f && playerRB.velocity.x > 0.1f)
                {
                    playerANI.Play("PlayerWalk");
                    playerSR.flipX = false;
                }

                if (playerRB.velocity.x > 3f)
                {
                    playerANI.Play("PlayerRun");
                    playerSR.flipX = false;
                }
            }

            //Change animator state by y velocity
            if (playerSInfo.IsName("PlayerJump"))
            {
                if (Mathf.Abs(playerRB.velocity.y) <= 0.00001f)
                {
                    playerANI.Play("PlayerStand");
                }
            }

            //increase velocity when press L/R key down
            if (!playerSInfo.IsName("PlayerJump") && !playerSInfo.IsName("PlayerDuck"))
            {
                if (Input.GetKey(KeyCode.RightArrow) && playerRB.velocity.x <= 7f)
                {
                    playerRB.AddForce(new Vector2(1000f * Time.deltaTime, 0), ForceMode2D.Force);
                }
                if (Input.GetKey(KeyCode.LeftArrow) && playerRB.velocity.x >= -7f)
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
            if (!playerSInfo.IsName("PlayerJump") && Input.GetKeyDown(KeyCode.UpArrow))
            {
                playerANI.Play("PlayerJump");
                playerRB.AddForce(new Vector2(0f, 7f), ForceMode2D.Impulse);
            }

            //Duck
            if (!playerSInfo.IsName("PlayerDuck") && Input.GetKeyDown(KeyCode.DownArrow))
            {
                playerANI.Play("PlayerDuck");
                playerRB.AddForce(new Vector2(0f, -7f), ForceMode2D.Impulse);
            }
            if (playerSInfo.IsName("PlayerDuck") && Input.GetKeyUp(KeyCode.DownArrow))
            {
                playerANI.Play("PlayerStand");
            }

            //Attack
            if ((!playerSInfo.IsName("PlayerChop") && !playerSInfo.IsName("PlayerCutU")) && Input.GetKeyDown(KeyCode.Space))
            {
                int moveChoice = Random.Range(1, 3);
                switch (moveChoice)
                {
                    case 1:
                        playerANI.Play("PlayerChop");
                        break;
                    case 2:
                        playerANI.Play("PlayerCutU");
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

    //Fall out of the game field
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            playerRB.position = new Vector3(0f, 0f);
            Camera.main.gameObject.transform.position = new Vector3(0f, 0f, -10f);

            HP -= 10;
            HPbar.fillAmount = HP / 100f;
        }
    }

    //Judge the trigger(collision) event
    private void OnTriggerEnter2D(Collider2D collision)
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

        if (collision.gameObject.name == "CheckPoint")
        {
            GameObject[] GOs = GameObject.FindGameObjectsWithTag("Enemy");

            Debug.Log(GOs.Length);

            if (GOs.Length > 0)
            {
                Debug.Log("Enemies not defeated");

                //show window
                message.showMessage();

            }
            else
            {
                Debug.Log("Game complete");

                //show success scene
                SceneManager.LoadScene("GameCompleteScene");
            }
        }
    }
}
