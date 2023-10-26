using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : MonoBehaviour
{
    [SerializeField]
    private GameObject currentPiece;  // 現在のマスに置かれている駒
    [SerializeField]
    private bool isActive = true;     // マスに置かれている駒がアクティブかどうか

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
