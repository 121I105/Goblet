using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : MonoBehaviour
{
    [SerializeField]
    private GameObject currentPiece;  // ���݂̃}�X�ɒu����Ă����
    [SerializeField]
    private bool isActive = true;     // �}�X�ɒu����Ă����A�N�e�B�u���ǂ���

    public GameObject CurrentPiece
    {
        get { return currentPiece; }
        set { currentPiece = value; }
    }

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }
}
