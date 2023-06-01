using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerCubeController : MonoBehaviour
{
    [SerializeField]
    GameObject _stageParent;
    [SerializeField]
    GameObject _gameClearCanvas;
    [SerializeField]
    Text _deathCountText;
    [SerializeField]
    Text _timetext;

    Rigidbody2D _rb;
    Mesh _mesh ;
    BoxCollider2D _boxcollider;
    Material _material;

    
    public int _deathCount = 0;
    //cube���\������v�f
    private Vector3[] _myVertices = {
            new Vector3 (0, 0, 0),
            new Vector3 (1, 0, 0),
            new Vector3 (1, 1, 0),
            new Vector3 (0, 1, 0),
        };
    private int[] _myTriangles = { 0, 2, 1, //face front
			0, 3, 2,};
    //���Ԓn�_�X�V�p
    private Vector3 _restartPotision =  new Vector3(-8f, -3.5f, 0f);
    //�L�k���[�g
    private float _rate = 0.05f;
    //cube����ݒ�p
    private float _jumpPower = 5f;
    private float _speed;
    private float _inputSpeed = 5 ;
    [SerializeField]
    private float _velocityLimit = 15f;
    //�r�Ɠ������Ă��邩�ǂ���
    public bool _collisionMe = false;
    
    void Start()
    {
        _material = (Material)Resources.Load("Material/FreezedPlayerMaterial");
        _boxcollider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _mesh = GetComponent<MeshFilter>().mesh;
        _mesh.vertices = _myVertices;
        _mesh.triangles = _myTriangles;
        _deathCountText.text = "DeathCount :" + _deathCount;
    }
    //�e����̎��ԊǗ��p
    private float _elapTime;
    private float _sTime;
    private float _wTime;
    private float _jTime;
    //����L�[��������Ă��邩�ǂ���
    private bool _isInputHorizonalKey = false;
    void Update()
    {
        //�A���W�����v�h�~�p
        _elapTime+= Time.deltaTime;
        _timetext.text = ((int)_elapTime / 60).ToString("00") + ":" + ((_elapTime % 60).ToString("00.00"));

        _jTime += Time.deltaTime;
        //���ꓮ��
        //�X�e�[�W�O�ł̃��X�|�[��
        if(transform.position.y < -50f)
        {
            transform.position = _restartPotision;
            //UI�X�V
            _deathCount++;
            _deathCountText.text = "DeathCount :" + _deathCount;
        }
        //���g��instance���A�Đ���
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //�S�[�̐���
            GameObject tmpObj;
            tmpObj = Instantiate(gameObject, transform.position,transform.rotation, _stageParent.transform);
            tmpObj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            tmpObj.GetComponent<PlayerCubeController>().enabled = false;
            tmpObj.tag = "DeadPlayer";
            //Instance���̎��ɎQ�ƌ^�̎Q�Ɠn���H�ɂȂ��Ă��邽�߁A�Q�Ƃ�l�ŏ�����
            Mesh tmpmesh = new Mesh();
            tmpmesh.vertices = _myVertices;
            tmpmesh.triangles = _myTriangles;
            tmpObj.GetComponent<MeshFilter>().mesh = tmpmesh;
            tmpObj.GetComponent<MeshRenderer>().material = _material;
            //�`��̏�����
            _myVertices[0] = new Vector3(0, 0, 0);
            _myVertices[1] = new Vector3(1, 0, 0);
            _myVertices[2] = new Vector3(1, 1, 0);
            _myVertices[3] = new Vector3(0, 1, 0);
            _mesh.vertices = _myVertices;
            _boxcollider.size = new Vector2(1,1);
            _boxcollider.offset = new Vector2(0.5f, 0.5f);
            transform.position = _restartPotision;
            //UI�X�V
            _deathCount++;
            _deathCountText.text = "DeathCount :" + _deathCount;
        }  
        //���ɂԂ�
        if (Input.GetKey(KeyCode.S))
        {
            _sTime += Time.deltaTime;
            if (_sTime > 0.1f && _mesh.vertices[2].y - _mesh.vertices[0].y > 0.4f)
            {
                //���_���W��ω�������
                _myVertices[0] = new Vector3(_myVertices[0].x - _rate /2, _myVertices[0].y, 0);
                _myVertices[1] = new Vector3(_myVertices[1].x + _rate /2, _myVertices[1].y, 0);
                _myVertices[2] = new Vector3(_myVertices[2].x + _rate /2, 1 / (_myVertices[1].x - _myVertices[0].x), 0);
                _myVertices[3] = new Vector3(_myVertices[3].x - _rate /2 , 1 / (_myVertices[1].x - _myVertices[0].x), 0);
                _sTime = 0f;
                _mesh.vertices = _myVertices;
                _mesh.triangles = _myTriangles;
               //collider�̒���
                float box_rate = _boxcollider.size.x + _rate;
                _boxcollider.size = new Vector2(box_rate ,1 / box_rate);
                _boxcollider.offset = new Vector2(0.5f, (1 / box_rate) /2);
            }
        }
        //�c�ɐL�΂�
        if (Input.GetKey(KeyCode.W))
        {
            _wTime += Time.deltaTime;
            if (_wTime > 0.1f && _mesh.vertices[1].x - _mesh.vertices[0].x > 0.4f)
            {
                _myVertices[0] = new Vector3(_myVertices[0].x + _rate/2, _myVertices[0].y, 0);
                _myVertices[1] = new Vector3(_myVertices[1].x - _rate/2, _myVertices[1].y, 0);
                _myVertices[2] = new Vector3(_myVertices[2].x - _rate/2, 1 / (_myVertices[1].x - _myVertices[0].x), 0);
                _myVertices[3] = new Vector3(_myVertices[3].x + _rate/2, 1 / (_myVertices[1].x - _myVertices[0].x), 0);

                _wTime = 0f;
                _mesh.vertices = _myVertices;
                _mesh.triangles = _myTriangles;
                float box_rate = _boxcollider.size.x - _rate;
                _boxcollider.size = new Vector2(box_rate, 1 / box_rate);
                _boxcollider.offset = new Vector2(0.5f, (1 / _boxcollider.size.x + _rate) / 2);
            }
        }

        //�ʏ퓮��
        //_speed = 0f;
        //���Ɉړ�
        if (Input.GetKey(KeyCode.A))
        {
            _speed = -_inputSpeed;
            _isInputHorizonalKey = true;
            
        }
        //�E�Ɉړ�   
        else if (Input.GetKey(KeyCode.D))
        {
            _speed = _inputSpeed;
            _isInputHorizonalKey = true;
        }
        else
        {
            _isInputHorizonalKey = false;
        }
            

        //�W�����v 
        if (Input.GetKeyDown(KeyCode.Space ) && (_rb.velocity.y == 0f ||_collisionMe) && (_jTime > 0.3f) )
        {
            _rb.AddForce(Vector2.up * _jumpPower,ForceMode2D.Impulse);
            _jTime = 0f;
        }
        //�X�e�[�W�Z���N�g��ʂɖ߂�
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene("SelectScene");
        }
        print(_rb.velocity);
        //臒l�̒���
        _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x,-15f, 15f), Mathf.Clamp(_rb.velocity.y, -15f, 15f));
    }
    void FixedUpdate()
    {
        //���E�̓���
        if (_isInputHorizonalKey)
        {
            _rb.velocity = new Vector2(_speed, _rb.velocity.y);
        }
        //���C�̒���
        _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, 0, 0.1f), _rb.velocity.y);
        if (Mathf.Abs(_rb.velocity.x) < 0.5f)
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
    }
    //�I�u�W�F�N�g�ƂԂ������ۂ̓���,2�i�W�����v��
    public void OnCollisionStay2D(Collision2D other)
    {
        
        if (other.gameObject.CompareTag("DeadPlayer"))
        {
            _collisionMe = true;
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
            _collisionMe = false;
        }
    }
    //�Q�[���N���A
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("middle"))
        {
            _restartPotision = other.transform.position + new Vector3(0 , 0 , -1);
            other.gameObject.GetComponent<SpriteRenderer>().color = new Color32(77, 255, 0, 255);
        }
        if (other.gameObject.CompareTag("GameClear"))
        {
            string myScene = SceneManager.GetActiveScene().name;
            int minDeathCount = PlayerPrefs.GetInt(myScene + "DeathCount", _deathCount);
            float minTime = PlayerPrefs.GetFloat(myScene + "Time", 0);
            //���񂩂ǂ���
            if (minTime == 0f)
            {
                PlayerPrefs.SetInt(myScene + "DeathCount", _deathCount);
                PlayerPrefs.SetFloat(myScene + "Time", _elapTime);
                PlayerPrefs.Save();

            }
            else
            {
                //�n�C�X�R�A�X�V�ł������ǂ���
                if (_deathCount < minDeathCount || (_deathCount == minDeathCount && _elapTime < minTime))
                {
                    PlayerPrefs.SetInt(myScene + "DeathCount", _deathCount);
                    PlayerPrefs.SetFloat(myScene + "Time", _elapTime);
                    PlayerPrefs.Save();
                }

            }
            _gameClearCanvas.SetActive(true);
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


