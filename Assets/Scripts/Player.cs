using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    //地面の判定
    private Transform LGroundCheck;
    private Transform RGroundCheck;
    public  LayerMask GroundCheckLayer;
    public  bool Grounded = false;

    //壁の判定
    private Transform UWallCheck;
    private Transform DWallCheck;
    public  LayerMask WallCheckLayer;
    public  bool Walled = false;

    //壁判定オブジェクト初期位置
    private float TmpWallpos_x;

    //スピード
    [SerializeField, Range(0, 10)]
    private float speed = 10f;

    //ジャンプ力
    [SerializeField, Range(0, 10)]
    private float jump = 15f;

    //弾のTransform
    private Transform launcher;

    //弾の発射間隔
    [SerializeField, Range(0, 10)]
    private float garagaraInterval = 0.5f;

    //経過時間
    private float mTimeElapsed = 0;

    private SpriteRenderer sr;
    private Rigidbody2D rb2d;
    private Animator anim;

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        launcher = transform.Find("garagara");


        //地面判定
        LGroundCheck = transform.Find("LGroundCheck");
        RGroundCheck = transform.Find("RGroundCheck");

        //壁判定
        UWallCheck = transform.Find("UWallCheck");
        DWallCheck = transform.Find("DWallCheck");

        //壁判定オブジェクトの初期位置を取得
        TmpWallpos_x = UWallCheck.transform.position.x - rb2d.position.x;
    }

    void Update()
    {
        //壁判定オブジェクトの相対位置を示す変数
        float tmp_x = 0;

        //着地判定
        Grounded = Physics2D.Linecast(LGroundCheck.transform.position, RGroundCheck.transform.position, GroundCheckLayer);

        //壁面判定
        Walled = Physics2D.Linecast(UWallCheck.transform.position, DWallCheck.transform.position, WallCheckLayer);

        //移動
        float direction = Input.GetAxisRaw("Horizontal");

        if (direction != 0)
        {
            anim.SetBool("isRun", true);

            //左向きに設定
            if (direction == -1)
            {
                sr.flipX = true;
                tmp_x =- TmpWallpos_x;
            }
            //右向きに設定
            else if (direction == 1)
            {
                tmp_x = TmpWallpos_x;
                sr.flipX = false;
            }
        }
        else
        {
            anim.SetBool("isRun", false);
        }

        //移動処理
        //壁がある場合は、移動させない
        rb2d.velocity = new Vector2(Walled ? 0 : speed * direction, rb2d.velocity.y);

        //進行方向に壁判定のオブジェクトを移動
        UWallCheck.transform.position = new Vector2(rb2d.position.x + tmp_x, UWallCheck.position.y);
        DWallCheck.transform.position = new Vector2(rb2d.position.x + tmp_x, DWallCheck.position.y);

        //ジャンプ処理
        if (Input.GetKey(KeyCode.Space) && Grounded)
        {
            anim.SetBool("isRun", false);
            anim.SetBool("isJump", true);

            rb2d.velocity = new Vector2(rb2d.velocity.x, jump);
        }
        else if (Grounded)
        {
            anim.SetBool("isJump", false);
        }

        mTimeElapsed += Time.deltaTime;
        if (mTimeElapsed >= garagaraInterval)
        {
            //弾（ガラガラ）の発射
            if (Input.GetKey(KeyCode.Z))
            {
                launcher.GetComponent<Launcher>().Fire(sr);
//                transform.GetComponent<Launcher>().Fire(sr);
                mTimeElapsed = 0.0f;
            }
        }

        //yが-100以下になるとリスタート
        if(rb2d.position.y < -100)
        {
            StartCoroutine("restart");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //敵に当たると死ぬ
        if (other.tag == "Enemy")
        {
            //死ぬアニメーション
            transform.Rotate(0, 0, 180);
            gameObject.layer = LayerMask.NameToLayer("Empty");

            //リスタート処理
            StartCoroutine("restart");
        }
    }

    //リスタート処理
    IEnumerator restart()
    {
        //5秒待つ
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("main");
    }


    // テキスト表示
    void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 400, 40), UWallCheck.transform.position.ToString());

        // リセットボタン
        if (GUI.Button(new Rect(5, 50, 110, 30), "リセットボタン"))
        {
            SceneManager.LoadScene("main");
        }
    }
}