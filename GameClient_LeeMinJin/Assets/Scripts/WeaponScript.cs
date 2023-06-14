using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    const int atk = 1000;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PlayerMove.player_status.Equals("attack"))    //공격 상태
        {
            GetComponentInParent<PlayerMove>().StopCoroutine(PlayerMove.coroutine);
            //해당 enemy 객체 체력 감소
            GameManager.instance.EnemyAttack(atk);
            PlayerMove.player_status = "none";
        }
    }
}
