using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 駒の種類を表す列挙型
public enum PieceType
{
    Big,     // 大きな駒
    Medium,  // 中程度の駒
    Small    // 小さな駒
}

// 駒の所属チームを表す列挙型
public enum PieceTeam
{
    Blue,    // 青チーム
    Orange   // オレンジチーム
}

// 駒を表すクラス
public class Piece : MonoBehaviour
{
    public PieceType type;   // 駒の種類（大、中、小）
    public PieceTeam team;   // 駒の所属チーム（青、オレンジ）
    public int number;       // 駒の番号

    private bool isGrabbing; // マウスがつかんでいるかどうかのフラグ
    private GameManager gameManager; // GameManagerへの参照

    // 駒のアセットパスを生成するプライベートメソッド
    private string GetAssetPath()
    {
        string teamName = team.ToString();   // チーム名を取得
        string typeName = "";                // 種類名を格納する変数
        string numberName = number.ToString(); // 番号を文字列に変換して取得

        // 駒の種類に応じて種類名を設定
        switch (type)
        {
            case PieceType.Big:
                typeName = "Big";
                break;
            case PieceType.Medium:
                typeName = "Medium";
                break;
            case PieceType.Small:
                typeName = "Small";
                break;
        }

        // チーム名、種類名、番号を結合してアセットパスを生成
        return teamName + typeName + numberName;
    }

    Plane plane;             // マウスクリック時に生成される平面
    Transform sphere;          // つかんでいるオブジェクトのTransform
    Transform selectedPiece = null; // 選択された駒を保存する変数

    // Start is called before the first frame update
    void Start()
    {
        // 平面の定義：法線ベクトル(Vector3.up)がy軸方向で、位置(Vector3.up)が原点上にある平面
        plane = new Plane(Vector3.up, Vector3.up);

        // GameManagerへの参照を取得
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update内に以下のような変更を加えます：

    

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (selectedPiece == null)
                {
                    if (hit.collider.CompareTag("Player1") && gameManager.CurrentPlayer == (int)PieceTeam.Blue)
                    {
                        // Player1のターンでPlayer1の駒を選択
                        selectedPiece = hit.transform;
                    }
                    else if (hit.collider.CompareTag("Player2") && gameManager.CurrentPlayer == (int)PieceTeam.Orange)
                    {
                        // Player2のターンでPlayer2の駒を選択
                        selectedPiece = hit.transform;
                    }
                }
                else
                {
                    if (hit.collider.CompareTag("Ground"))
                    {
                        // 駒の新しい位置を地面のセルの位置に基づいて設定
                        Vector3 newPiecePosition = hit.collider.transform.position + new Vector3(0, 2, 0); // 2は駒の高さとして仮定

                        // 以前のコードのように、特定のセルの名前に基づいて位置を調整する場合
                        if (hit.collider.name == "cube1-1")
                        {
                            newPiecePosition = new Vector3(0, 2, 0);
                        }
                        if (hit.collider.name == "cube1-2")
                        {
                            newPiecePosition = new Vector3(1.25f, 2, 0);
                        }
                        if (hit.collider.name == "cube1-3")
                        {
                            newPiecePosition = new Vector3(2.5f, 2, 0);
                        }
                        if (hit.collider.name == "cube2-1")
                        {
                            newPiecePosition = new Vector3(0, 2, 1.25f);
                        }
                        if (hit.collider.name == "cube2-2")
                        {
                            newPiecePosition = new Vector3(1.25f, 2, 1.25f);
                        }
                        if (hit.collider.name == "cube2-3")
                        {
                            newPiecePosition = new Vector3(2.5f, 2, 1.25f);
                        }
                        if (hit.collider.name == "cube3-1")
                        {
                            newPiecePosition = new Vector3(0, 2, 2.5f);
                        }
                        if (hit.collider.name == "cube3-2")
                        {
                            newPiecePosition = new Vector3(1.25f, 2, 2.5f);
                        }
                        if (hit.collider.name == "cube3-3")
                        {
                            newPiecePosition = new Vector3(2.5f, 2, 2.5f);
                        }
                        // 最終的に駒の位置を更新
                        selectedPiece.position = newPiecePosition;
                 

                        // ここで勝利条件をチェック
                        if (gameManager.CurrentPlayer == (int)PieceTeam.Blue)
                        {
                            gameManager.CheckStone("Player1");
                        }
                        else
                        {
                            gameManager.CheckStone("Player2");
                        }


                        // ターンを切り替える
                        gameManager.SwitchTurn();
                        

                        // 駒の選択を解除
                        selectedPiece = null;
                    }
                }
            }
        }
    }


}