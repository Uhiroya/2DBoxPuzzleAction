using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    //�Z���N�g�{�^���������ƑΉ������X�e�[�W�ɐ؂�ւ��
    public void SelectBt(int Stage)
    {
        SceneManager.LoadScene("Stage" + Stage);
    }
}