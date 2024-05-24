using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    #region [Initalize]
    public bool boolInputMethod;
    public int HP;

    private GameObject player;

    private Animator enemyANI;
    private AnimatorStateInfo enemySInfo;
    private Rigidbody2D enemyRB;
    private SpriteRenderer enemySR;

    private double xDiff;
    private double yDiff;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        enemyANI = GetComponent<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();

        player = GameObject.Find("Player");
        xDiff = 0;
        yDiff = 0;

        HP = 70;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Debug.Log("Enemy defeated");
            Destroy(gameObject);
        }

        //Always get the state of the animator
        enemySInfo = enemyANI.GetCurrentAnimatorStateInfo(0);

        //Change animator state by x velocity
        if (enemySInfo.IsName("EnemyStand") || enemySInfo.IsName("EnemyWalk") || enemySInfo.IsName("EnemyRun"))
        {
            if (enemyRB.velocity.x <= -3f)
            {
                enemyANI.Play("EnemyRun");
                enemySR.flipX = true;
            }

            if (enemyRB.velocity.x <= -0.1f && enemyRB.velocity.x > -3f)
            {
                enemyANI.Play("EnemyWalk");
                enemySR.flipX = true;
            }

            if (enemyRB.velocity.x <= 0.1f && enemyRB.velocity.x > -0.1f)
            {
                enemyANI.Play("EnemyStand");
            }

            if (enemyRB.velocity.x <= 3f && enemyRB.velocity.x > 0.1f)
            {
                enemyANI.Play("EnemyWalk");
                enemySR.flipX = false;
            }

            if (enemyRB.velocity.x > 3f)
            {
                enemyANI.Play("EnemyRun");
                enemySR.flipX = false;
            }
        }


        xDiff = transform.position.x - player.transform.position.x;
        yDiff = transform.position.y - player.transform.position.y;

        if (yDiff < 1)
        {
            if (xDiff > 2 && xDiff <= 5 && enemyRB.velocity.x <= 4f && enemyRB.velocity.x >= -4f)
            {
                enemyRB.AddForce(new Vector2(-500f * Time.deltaTime, 0), ForceMode2D.Force);
            }
            else if (xDiff < -2 && xDiff >= -5 && enemyRB.velocity.x >= -4f && enemyRB.velocity.x <= 4f)
            {
                enemyRB.AddForce(new Vector2(500f * Time.deltaTime, 0), ForceMode2D.Force);
            }

            if (xDiff >= 0 && xDiff <= 2 && enemySInfo.IsName("EnemyStand"))
            {
                Debug.Log("Right Triggered");
                enemySR.flipX = true;
                gameObject.transform.GetChild(0).gameObject.transform.localScale = new Vector2(-1, 1);

                int moveChoice = Random.Range(1, 3);
                switch (moveChoice)
                {
                    case 1:
                        enemyANI.Play("EnemyPunchL");
                        break;
                    case 2:
                        enemyANI.Play("EnemyPunchR");
                        break;
                }
            }
            if (xDiff <= 0 && xDiff >= -2 && enemySInfo.IsName("EnemyStand"))
            {
                Debug.Log("Left Triggered");
                enemySR.flipX = false;
                gameObject.transform.GetChild(0).gameObject.transform.localScale = new Vector2(1, 1);

                int moveChoice = Random.Range(1, 3);
                switch (moveChoice)
                {
                    case 1:
                        enemyANI.Play("EnemyPunchL");
                        break;
                    case 2:
                        enemyANI.Play("EnemyPunchR");
                        break;
                }
            }
        }
    }

    //Fall out of the game field
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            Destroy(gameObject);
        }
    }

    //Judge the trigger(collision) event
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Attack"))
        {
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("Hit!!");
                HP -= 10;
            }
        }
    }
}
