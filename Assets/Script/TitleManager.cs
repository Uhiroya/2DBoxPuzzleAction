using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;    //�V�[���}�l�[�W���[�Ăяo��

public class TitleManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    //�X�^�[�g�{�^���������ƃZ���N�g��ʂɐ؂�ւ��
    public void StartBt()
    {
        
        SceneManager.LoadScene("SelectScene");
    }
}
