using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    
    [SerializeField]
    GameObject enemy_folder;

    [SerializeField]
    GameObject[] player_hp = new GameObject[3];
    [SerializeField]
    Slider enemy_hp_slider;
    [SerializeField]
    Text enemy_hp_txt;
    [SerializeField]
    Text score_txt;

    //Wave total
    const int wave_tot = 4;
    const float enemy_height = 1.7f;
    const float enemy_up_power = 500.0f;

    //now wave
    int now_wave = 1;
    //field enemies cnt
    int now_enemies = 0;    
    int front_enemy = 0;    //가장 선두에 있는 적 객체 인덱스


    //enemies object pooling
    Enemy[] enemies = new Enemy[wave_tot*5 + 5]; //wave당 적 객체 수: 10, 15, 20, 25...최대 8*5+5


    void Start()
    {
        GameObject[] enemies_prefab = Resources.LoadAll<GameObject>("Prefabs/Enemies/");
        int enemyP_len = enemies_prefab.Length;
        
        // 적 객체 오브젝트 풀링
        for(int i=0; i<enemies.Length; i++)
        {
            int num = Random.Range(0, enemyP_len);
            string enemyP_name = enemies_prefab[num].name;
            enemies[i] = new Enemy(Instantiate(enemies_prefab[num]), enemyP_name);
            //enemy_folder의 하위로 이동
            enemies[i].enemy.transform.parent = enemy_folder.transform;
            //비활성화
            enemies[i].enemy.SetActive(false);
        }
        Wave(now_wave);

        //ui 초기화
        PlayerHPUI(PlayerMove.hp);
        EnemyHPUI();
    }

    void Wave(int wave_num)
    {
        enemy_folder.transform.position = new Vector2(0, 10);

        //해당 wave에서 등장할 적 객체 수
        int enemy_len = wave_num * 5 + 5;

        for (int i=0; i<enemy_len; i++)
        {
            //적 객체 활성화 & 위치 지정
            enemies[i].enemy.SetActive(true);
            Vector2 now_pos = new Vector2(0, 0);

            now_pos.y = now_pos.y + (now_enemies * enemy_height);
            enemies[i].enemy.transform.localPosition = now_pos;

            //적 객체 수 카운트 변수 증가
            now_enemies++;
        }
        front_enemy = 0;
    }

    //player와 부딪히면 enemy_folder 위로 상승한다.
    public void EnemyUp()
    {
        Rigidbody2D rb = enemy_folder.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * enemy_up_power);
    }


    //공격하면, enemy의 체력이 감소한다.
    public void EnemyAttack(int atk)
    {
        enemies[front_enemy].hp -= atk;
        if(enemies[front_enemy].hp <= 0) {
            //init pool
            enemies[front_enemy].enemy.SetActive(false);
            enemies[front_enemy].hp = enemies[front_enemy].max_hp;

            now_enemies--;
            if (now_enemies > 0)
            {
                front_enemy++;
            }
        }
        ChkEnemyCnt();
        EnemyHPUI();
    }

    void ChkEnemyCnt()
    {
        if(now_enemies == 0)
        {
            if (now_wave == wave_tot)
            {
                GameClear();
            }
            else
            {
                now_wave++;
                Wave(now_wave);
            }
        }
    }


    void GameClear()
    {
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        //stop enemy
        enemy_folder.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        //stop player
        PlayerMove.player_status = "game over";
    }

    //---------------UI------------------//
    public void GameStartBtn()
    {
        Wave(now_wave);
    }

    public void PlayerHPUI(int hp)
    {
        for (int i = 0; i < player_hp.Length; i++)
        {
            player_hp[i].SetActive(false);
            if (i < hp)
            {
                player_hp[i].SetActive(true);
            }
        }
    }

    public void EnemyHPUI()
    {
        enemy_hp_slider.maxValue = enemies[front_enemy].max_hp;
        enemy_hp_slider.value = enemies[front_enemy].hp;

        string hp_txt = enemy_hp_slider.value + " / " + enemy_hp_slider.maxValue;
        enemy_hp_txt.text = hp_txt;
    }
}
