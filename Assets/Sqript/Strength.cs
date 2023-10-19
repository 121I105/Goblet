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

    }

    void Update()
    {

    }

    public void ActivatePiece()
    {
        gameObject.SetActive(true);
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
                // 自分の駒の強さが小さい場合、自分の駒を非アクティブ化する
                gameObject.SetActive(false);
                Debug.Log("My piece is weaker. Disabling my piece.");
            }
        }
    }
}