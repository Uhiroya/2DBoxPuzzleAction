using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MakingCube : MonoBehaviour
{
    [SerializeField]
    GameObject stage_parent;
    [SerializeField]
    GameObject GameClearCanvas;
    [SerializeField]
    Text DeathCount;
    [SerializeField]
    Text Timetext;

    Rigidbody2D rb;
    Mesh mesh ;
    BoxCollider2D Boxcollider;
    Material material;

    
    public int deathCount = 0;
    //cubeを構成する要素
    private Vector3[] myVertices = {
            new Vector3 (0, 0, 0),
            new Vector3 (1, 0, 0),
            new Vector3 (1, 1, 0),
            new Vector3 (0, 1, 0),
        };
    private int[] myTriangles = { 0, 2, 1, //face front
			0, 3, 2,};
    //中間地点更新用
    private Vector3 restart_potision =  new Vector3(-8f, -3.5f, 0f);
    //伸縮レート
    private float rate = 0.05f;
    //cube動作設定用
    private float jump_power = 5f;
    private float speed;
    private float inputSpeed = 5 ;
    //屍と当たっているかどうか
    public bool collision_me = false;

    public List<GameObject> Obj = new List<GameObject>();
    public List<Rigidbody2D> Obj_rigid = new List<Rigidbody2D>();
    
    void Start()
    {

        material = (Material)Resources.Load("Material/FreezedPlayerMaterial");
        Boxcollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = myVertices;
        mesh.triangles = myTriangles;
        DeathCount.text = "DeathCount :" + deathCount;
    }
    //各動作の時間管理用
    private float elaptime;
    private float Stime;
    private float Wtime;
    private float Jtime;
    void Update()
    {
        //連続ジャンプ防止用
        elaptime+= Time.deltaTime;
        Timetext.text = ((int)elaptime / 60).ToString("00") + ":" + ((elaptime % 60).ToString("00.00"));

        Jtime += Time.deltaTime;
        //特殊動作
        //自身をinstance化、再生成
        if (Input.GetKeyDown(KeyCode.Q))
        {

            //亡骸の生成
            GameObject tmpObj;
            tmpObj = Instantiate(gameObject, transform.position,transform.rotation, stage_parent.transform);
            tmpObj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            tmpObj.GetComponent<MakingCube>().enabled = false;
            tmpObj.tag = "DeadPlayer";
            //Instance化の時に参照型の参照渡し？になっているため、参照を値で初期化
            Mesh tmpmesh = new Mesh();
            tmpmesh.vertices = myVertices;
            tmpmesh.triangles = myTriangles;
            tmpObj.GetComponent<MeshFilter>().mesh = tmpmesh;
            tmpObj.GetComponent<MeshRenderer>().material = material;
            Obj.Add(tmpObj);
            //形状の初期化
            myVertices[0] = new Vector3(0, 0, 0);
            myVertices[1] = new Vector3(1, 0, 0);
            myVertices[2] = new Vector3(1, 1, 0);
            myVertices[3] = new Vector3(0, 1, 0);
            mesh.vertices = myVertices;
            Boxcollider.size = new Vector2(1,1);
            Boxcollider.offset = new Vector2(0.5f, 0.5f);
            this.transform.position = restart_potision;


            //UI更新
            deathCount++;
            DeathCount.text = "DeathCount :" + deathCount;
        }  
        //下につぶす
        if (Input.GetKey(KeyCode.S))
        {
            Stime += Time.deltaTime;
            if (Stime > 0.1f && mesh.vertices[2].y - mesh.vertices[0].y > 0.4f)
            {
                myVertices[0] = new Vector3(myVertices[0].x - rate /2, myVertices[0].y, 0);
                myVertices[1] = new Vector3(myVertices[1].x + rate /2, myVertices[1].y, 0);
                myVertices[2] = new Vector3(myVertices[2].x + rate /2, 1 / (myVertices[1].x - myVertices[0].x), 0);
                myVertices[3] = new Vector3(myVertices[3].x - rate /2 , 1 / (myVertices[1].x - myVertices[0].x), 0);
                Stime = 0f;
                mesh.vertices = myVertices;
                mesh.triangles = myTriangles;
                float box_rate = Boxcollider.size.x + rate;
                Boxcollider.size = new Vector2(box_rate ,1 / box_rate);
                Boxcollider.offset = new Vector2(0.5f, (1 / box_rate) /2);
            }
        }
        //縦に伸ばす
        if (Input.GetKey(KeyCode.W))
        {
            Wtime += Time.deltaTime;
            if (Wtime > 0.1f && mesh.vertices[1].x - mesh.vertices[0].x > 0.4f)
            {
                myVertices[0] = new Vector3(myVertices[0].x + rate/2, myVertices[0].y, 0);
                myVertices[1] = new Vector3(myVertices[1].x - rate/2, myVertices[1].y, 0);
                myVertices[2] = new Vector3(myVertices[2].x - rate/2, 1 / (myVertices[1].x - myVertices[0].x), 0);
                myVertices[3] = new Vector3(myVertices[3].x + rate/2, 1 / (myVertices[1].x - myVertices[0].x), 0);

                Wtime = 0f;
                mesh.vertices = myVertices;
                mesh.triangles = myTriangles;
                float box_rate = Boxcollider.size.x - rate;
                Boxcollider.size = new Vector2(box_rate, 1 / box_rate);
                Boxcollider.offset = new Vector2(0.5f, (1 / Boxcollider.size.x + rate) / 2);
            }
        }

        //通常動作
        speed = 0f;
        //左に移動
        if (Input.GetKey(KeyCode.A))
        {
            speed = -inputSpeed;
        }
        //右に移動   
        if (Input.GetKey(KeyCode.D))
        {
            speed = inputSpeed;
        }
        //ジャンプ 
        if (Input.GetKeyDown(KeyCode.Space ) && (rb.velocity.y == 0f ||collision_me) && (Jtime > 0.3f) )
        {
            rb.AddForce(transform.up * jump_power,ForceMode2D.Impulse);
            Jtime = 0f;
        }
        //リスタート
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene("SelectScene");
        }
            rb.velocity = new Vector2(speed, rb.velocity.y);

    }
    //オブジェクトとぶつかった際の動作,2段ジャンプ可否
    public void OnCollisionStay2D(Collision2D other)
    {
        
        if (other.gameObject.CompareTag("DeadPlayer"))
        {
            collision_me = true;
            if (Input.GetKey(KeyCode.E))
            {
                Destroy(other.gameObject);
            }
        }
    }
    public void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("DeadPlayer"))
        {
            collision_me = false;
        }
    }
    //ゲームクリア
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("middle"))
        {
            restart_potision = other.transform.position + new Vector3(0 , 0 , -1);
            other.gameObject.GetComponent<SpriteRenderer>().color = new Color32(77, 255, 0, 255);
        }
        if (other.gameObject.CompareTag("GameClear"))
        {
            string myScene = SceneManager.GetActiveScene().name;
            int minDeathCount = PlayerPrefs.GetInt(myScene + "DeathCount", deathCount);
            float minTime = PlayerPrefs.GetFloat(myScene + "Time", 0);
            //初回かどうか
            if (minTime == 0f)
            {
                PlayerPrefs.SetInt(myScene + "DeathCount", deathCount);
                PlayerPrefs.SetFloat(myScene + "Time", elaptime);
                PlayerPrefs.Save();

            }
            else
            {
                //ハイスコア更新できたかどうか
                if (deathCount < minDeathCount || (deathCount == minDeathCount && elaptime < minTime))
                {
                    PlayerPrefs.SetInt(myScene + "DeathCount", deathCount);
                    PlayerPrefs.SetFloat(myScene + "Time", elaptime);
                    PlayerPrefs.Save();
                }

            }
            GameClearCanvas.SetActive(true);
            Destroy(other.gameObject);
            Coroutine coroutine = StartCoroutine(DelayMethod(1.0f, () => {
            SceneManager.LoadScene("SelectScene"); 
            }));
            
        }
    }


    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

}


