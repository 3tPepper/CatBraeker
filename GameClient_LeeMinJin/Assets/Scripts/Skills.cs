using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public static bool is_atk_ready = false;
    const int num = 5;

    Vector2 down_pos;
    Vector2 up_pos;

    private void Update()
    {
        if (is_atk_ready)
        {
            if (Input.touchCount > 0)
            {
                Touch t = Input.GetTouch(0);
                switch (t.phase)
                {
                    case TouchPhase.Began:
                        down_pos = t.position;
                        break;
                    case TouchPhase.Ended:
                        up_pos = t.position;
                        //down_pos.y + 조정값 < up_pos.y 라면, 위로 스와이프 한 것으로 간주
                        if (down_pos.y + num < up_pos.y)
                        {
                            AttackSkill();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void AttackSkill()
    {
        //skill

        //init
        GameManager.atk_gage = 0f;
        GameManager.instance.AtkSkillUI(0f);
    }
        
}
