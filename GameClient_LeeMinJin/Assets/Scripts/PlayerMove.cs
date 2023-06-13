using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb;

    public static int hp = 3;
    public static string player_status = "none";
    const float jump_power = 400.0f;  //player jump speed
    Boolean is_ground = true;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        player_status = "none";
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            is_ground = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            //플레이어 상태 체크
            if (player_status.Equals("defence"))        //방어 상태
            {
                //enemy folder 위로 상승
                GameManager.instance.EnemyUp();
            }
            else if (player_status.Equals("attack"))    //공격 상태
            {
                //해당 enemy 객체 비활성화
            }
            else
            {
                GameManager.instance.EnemyUp();
                //플레이어 hp감소
                hp--;
                GameManager.instance.PlayerHPUI(hp);
                if (hp <= 0)
                {
                    GameManager.instance.GameOver();
                }
            }
        }
    }


    //------------Button-----------//
    public void PressJumpBtn()
    {
        //if 땅과 접한 상태일 경우 실행
        if (is_ground)
        {
            Debug.Log("press jump btn");

            rb.AddForce(Vector2.up * jump_power);

            is_ground = false;
        }

    }


    //defence 버튼 누르고 있는 동안 -> defence 상태
    public void PointerDownDefBtn()
    {
        if(!player_status.Equals("game over"))
        {
            Debug.Log("press defence btn");
            player_status = "defence";
        }

    }


    public void PointerUpDefBtn()
    {
        if (!player_status.Equals("game over"))
        {
            Debug.Log("no press defence btn");
            player_status = "none";
        }
    }


    public void PressAttackBtn()
    {
        //if player_status == "none" 일 경우 실행
        if (player_status.Equals("none"))
        {
            Debug.Log("press attack btn");
            player_status = "attack";
            //animation play
        }
    }


}
