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
    GameObject game_start_panel;
    [SerializeField]
    GameObject game_over_panel;
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
    [SerializeField]
    Text wave_txt;
    [SerializeField]
    Slider wave_slider;
    [SerializeField]
    Text combo_txt;
    public Image atk_gage_img;

    public static float atk_gage = 0.0f;

    //Wave total
    const int wave_tot = 4;
    int all_enemy_cnt;
    const float enemy_height = 1.6f;
    const float enemy_up_power = 600.0f;
    const int atk = 1000;
    const float gage_per = 0.1f;

    //now wave
    int now_wave = 1;
    //field enemies cnt
    int now_enemies = 0;    
    int front_enemy = 0;    //가장 선두에 있는 적 객체 인덱스
    int solve_enemy = 0;    //해치운 적의 수
    public static int combo_num = 0;
    int now_score = 0;

    //animator
    Animator combo_animator;

    //enemies object pooling
    Enemy[] enemies = new Enemy[wave_tot * 5 + 5]; //wave당 적 객체 수: 10, 15, 20, 25...최대 8*5+5


    void Start()
    {
        game_start_panel.SetActive(true);
        game_over_panel.SetActive(false);
        combo_animator = combo_txt.GetComponent<Animator>();

        for(int i=1; i<=wave_tot; i++)
        {
            all_enemy_cnt += (i * 5) + 5;
        }
    }

    void Wave(int wave_num)
    {
        WaveUI();

        enemy_folder.transform.position = new Vector2(0, 15);

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
    public void EnemyAttack()
    {
        AtkSkillUI(gage_per);
        
        ComboUI(1);

        enemies[front_enemy].hp -= atk;
        if(enemies[front_enemy].hp <= 0) {
            //sound
            Sound.instance.EnemySound();

            //score 증가: 쓰러트린 적의 최대 체력만큼 점수 획득
            ScoreUI(enemies[front_enemy].max_hp);

            //init pool
            enemies[front_enemy].enemy.SetActive(false);
            enemies[front_enemy].hp = enemies[front_enemy].max_hp;

            now_enemies--;
            solve_enemy++;
            if (now_enemies > 0)
            {
                front_enemy++;
            }
        }
        WaveUI();
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
        game_over_panel.SetActive(true);

        //stop enemy
        enemy_folder.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        //stop player
        PlayerMove.player_status = "game over";
    }

    //---------------UI------------------//
    public void GameStartBtn()
    {
        game_start_panel.SetActive(false);
        game_over_panel.SetActive(false);
        //init
        enemy_folder.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        now_wave = 1;
        now_score = 0;
        now_enemies = 0;
        front_enemy = 0;
        solve_enemy = 0;
        combo_num = 0;
        atk_gage = 0f;

        PlayerMove.hp = 3;
        PlayerMove.player_status = "none";

        GameObject[] enemies_prefab = Resources.LoadAll<GameObject>("Prefabs/Enemies/");
        int enemyP_len = enemies_prefab.Length;

        // 적 객체 오브젝트 풀링
        for (int i = 0; i < enemies.Length; i++)
        {
            int num = Random.Range(0, enemyP_len);
            string enemyP_name = enemies_prefab[num].name;
            enemies[i] = new Enemy(Instantiate(enemies_prefab[num], enemy_folder.transform), enemyP_name);
            //비활성화
            enemies[i].enemy.SetActive(false);
        }


        //ui 초기화
        wave_slider.maxValue = all_enemy_cnt;
        PlayerHPUI(PlayerMove.hp);
        EnemyHPUI();
        ScoreUI(0);
        AtkSkillUI(0f);

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

    void ScoreUI(int get_score)
    {
        now_score += get_score;
        score_txt.text = now_score.ToString();
    }

    void WaveUI()
    {
        wave_txt.text = now_wave.ToString();
        wave_slider.value = solve_enemy;
    }

    void ComboUI(int num)
    {
        combo_num += num;
        combo_txt.text = combo_num + " COMBO";
        combo_animator.SetTrigger("Combo");
    }

    public void AtkSkillUI(float per)
    {
        if (atk_gage < 1)
        {
            atk_gage += per;
            atk_gage_img.fillAmount = atk_gage;
        }
        else
        {
            Skills.is_atk_ready = true;
        }
    }

    public void ExitBtn()
    {
        Application.Quit();
    }
}
