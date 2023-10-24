using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    Big,
    Medium,
    Small
}

public class Strength : MonoBehaviour
{
    public PieceType type;

    private bool isActive = true;
    private Vector3 deactivatePosition; // 非アクティブ化された位置

    public int GetStrength()
    {
        switch (type)
        {
            case PieceType.Big:
                return 3;
            case PieceType.Medium:
                return 2;
            case PieceType.Small:
                return 1;
            default:
                return 0;
        }
    }

    void Start()
    {
        // 初期位置を記憶
        deactivatePosition = new Vector3(1000f, 1000f, 1000f); // デフォルトの非アクティブ位置
    }

    void Update()
    {

    }

    public void ActivatePiece()
    {
        isActive = true;
        // デフォルトの非アクティブ位置に移動して再度表示
        transform.position = deactivatePosition;
        gameObject.SetActive(true);
    }

    public void DeactivatePiece()
    {
        isActive = false;
        // 現在の位置を非アクティブ位置として記憶
        deactivatePosition = transform.position;
        // 遠くの位置に移動して非アクティブ化
        transform.position = new Vector3(1000f, 1000f, 1000f);
        gameObject.SetActive(false);
    }

    public bool IsPieceActive
    {
        get { return isActive; }
    }

    void OnCollisionEnter(Collision collision)
    {
        Strength otherStrength = collision.gameObject.GetComponent<Strength>();

        if (otherStrength != null)
        {
            int myStrength = GetStrength();
            int otherPieceStrength = otherStrength.GetStrength();

            if (myStrength < otherPieceStrength)
            {
                // 自分の駒の強さが小さい場合、駒を非アクティブ化
                DeactivatePiece();
                Debug.Log("My piece is weaker. Disabling my piece.");
            }
        }
    }
}