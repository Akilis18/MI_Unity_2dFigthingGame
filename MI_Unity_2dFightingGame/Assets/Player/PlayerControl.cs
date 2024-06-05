using System.Collections;
using System.Collections.Generic;
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

    private float timer;
    private float hitTm;

    public GDTFadeEffect GDT;

    Dictionary<string, bool> HoldingKey = new Dictionary<string, bool>();

    #endregion
 
    void Start()
    {
        playerANI = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();

        HPbar.fillAmount = 1;
        HP = 100;

        timer = 0;
        hitTm = 0;

        HoldingKey.Add("RightArr", false);
        HoldingKey.Add("LeftArr", false);
    }
 

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Game Interrupted");
            SceneManager.LoadScene("StartScene");
        }

        if (HP <= 0)
        {
            Debug.Log("Game over");
            SceneManager.LoadScene("GameOverScene");
        }

        //Timer count after triggered
        if (timer > 0f)
            timer += Time.deltaTime;
        if (timer > 3f)
        {
            //show success scene
            SceneManager.LoadScene("GameCompleteScene");
        }

        // Hit timer decrease when > 0
        if (hitTm > 0f)
            hitTm -= Time.deltaTime;
        if (hitTm < 0f)
            hitTm = 0f;

        //Always get the state of the animator
        playerSInfo = playerANI.GetCurrentAnimatorStateInfo(0);

        #region [Input: Keyboard]
        //Change animator state by x velocity
        if (hitTm == 0f)
        {
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
        if (hitTm == 0f)
        {
            if (!playerSInfo.IsName("PlayerJump") && !playerSInfo.IsName("PlayerDuck"))
            {
                if (Input.GetKeyDown(KeyCode.RightArrow)) HoldingKey["RightArr"] = true;
                if (Input.GetKeyDown(KeyCode.LeftArrow)) HoldingKey["LeftArr"] = true;
                if (Input.GetKeyUp(KeyCode.RightArrow)) HoldingKey["RightArr"] = false;
                if (Input.GetKeyUp(KeyCode.LeftArrow)) HoldingKey["LeftArr"] = false;


                if (HoldingKey["RightArr"] && playerRB.velocity.x <= 7f)
                {
                    playerRB.AddForce(new Vector2(1000f * Time.deltaTime, 0), ForceMode2D.Force);
                }
                if (HoldingKey["LeftArr"] && playerRB.velocity.x >= -7f)
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

                if (Input.GetKeyDown(KeyCode.RightArrow)) HoldingKey["RightArr"] = true;
                if (Input.GetKeyDown(KeyCode.LeftArrow)) HoldingKey["LeftArr"] = true;
                if (Input.GetKeyUp(KeyCode.RightArrow)) HoldingKey["RightArr"] = false;
                if (Input.GetKeyUp(KeyCode.LeftArrow)) HoldingKey["LeftArr"] = false;

                //Change velocity when get key
                if (HoldingKey["RightArr"])
                {
                    playerRB.velocity = new Vector2(5f, playerRB.velocity.y);
                }
                if (HoldingKey["LeftArr"])
                {
                    playerRB.velocity = new Vector2(-5f, playerRB.velocity.y);
                }
            }
        }

        //Jump
        if (hitTm == 0f && !playerSInfo.IsName("PlayerJump") && Input.GetKeyDown(KeyCode.UpArrow))
        {
            playerANI.Play("PlayerJump");
            playerRB.AddForce(new Vector2(0f, 7f), ForceMode2D.Impulse);
        }

        //Duck
        if (hitTm == 0f && !playerSInfo.IsName("PlayerDuck") && Input.GetKeyDown(KeyCode.DownArrow))
        {
            playerANI.Play("PlayerDuck");
            playerRB.AddForce(new Vector2(0f, -7f), ForceMode2D.Impulse);
        }
        if (playerSInfo.IsName("PlayerDuck") && Input.GetKeyUp(KeyCode.DownArrow))
        {
            playerANI.Play("PlayerStand");
        }

        //Attack
        if ((hitTm == 0f && !playerSInfo.IsName("PlayerChop") && !playerSInfo.IsName("PlayerCutU")) && Input.GetKeyDown(KeyCode.Space))
        {
            if (playerSR.flipX)
            {
                //flip sword
                gameObject.transform.GetChild(0).gameObject.transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                gameObject.transform.GetChild(0).gameObject.transform.localScale = new Vector2(1, 1);
            }

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

                //play animation
                playerANI.Play("PlayerHit");
                hitTm = 0.2f;
                if (collision.transform.GetComponentInParent<SpriteRenderer>().flipX)
                {
                    playerRB.AddForce(new Vector2(-3f, 1f), ForceMode2D.Impulse);
                    playerSR.flipX = false;
                }
                else
                {
                    playerRB.AddForce(new Vector2(3f, 1f), ForceMode2D.Impulse);
                    playerSR.flipX = true;
                }
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

                if (timer == 0)
                {
                    timer = 1;
                    GDT.firstToLast = true;
                    GDT.gameObject.SetActive(true);
                }
            }
        }
    }
}
