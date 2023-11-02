using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameObject currentPiece; // 現在このマス目に存在する駒
    private List<GameObject> deactivatedPieces = new List<GameObject>(); // このマス目で非アクティブ化された駒のリスト

    // 駒がこのマス目に入るときに呼び出す
    public void EnterPiece(GameObject piece)
    {
        Debug.Log("Entering piece: " + piece.name);
        // 既に駒がある場合
        if (currentPiece != null)
        {
            Strength currentStrength = currentPiece.GetComponent<Strength>();
            Strength newStrength = piece.GetComponent<Strength>();

            // 新しい駒が現在の駒よりも強い場合
            if (newStrength.GetStrength() > currentStrength.GetStrength())
            {
                // 現在の駒を非アクティブ化してリストに追加
                currentPiece.SetActive(false);
                deactivatedPieces.Add(currentPiece);

                // 新しい駒をアクティブ化
                currentPiece = piece;
            }
            else
            {
                // 新しい駒を非アクティブ化（弱い場合）
                piece.SetActive(false);
                deactivatedPieces.Add(piece);
            }
        }
        else
        {
            // マス目が空なら新しい駒を置く
            currentPiece = piece;
        }
    }

    // 駒がこのマス目から離れるときに呼び出す
    public void ExitPiece(GameObject piece)
    {
        Debug.Log("Exiting piece: " + piece.name);
        if (currentPiece == piece)
        {
            // 最も強い非アクティブな駒を再アクティブ化
            ReactivateStrongestPiece();

            // 現在の駒をクリア
            currentPiece = null;
        }
        else
        {
            // パラメータの駒がcurrentPieceでない場合、リストから削除
            deactivatedPieces.Remove(piece);
        }
    }

    private void ReactivateStrongestPiece()
    {
        if (deactivatedPieces.Count > 0)
        {
            var strongestPiece = deactivatedPieces
                .OrderByDescending(p => p.GetComponent<Strength>().GetStrength())
                .First();

            strongestPiece.SetActive(true);
            currentPiece = strongestPiece;
            deactivatedPieces.Remove(strongestPiece);
        }
    }
}
