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
    private Vector3 originalPosition;  // ここで変数を宣言


    public int GetStrength() //駒の強さの設定
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
    // ここにpreviousPositionを追加
    public Vector3 previousPosition;


    // Start is called before the first frame update
    void Start()
    {
        // 平面の定義：法線ベクトル(Vector3.up)がy軸方向で、位置(Vector3.up)が原点上にある平面
        plane = new Plane(Vector3.up, Vector3.up);

        // GameManagerへの参照を取得
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    // Update is called once per frame
    void Update()
    {
        // マウス左ボタンがクリックされたときの処理
        if (Input.GetMouseButtonDown(0))
        {
            // マウスのスクリーン座標からRayを作成
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
                // Rayが他のオブジェクトに当たったかチェック
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    // 当たったオブジェクトのタグが"Player"の場合
                if (hit.collider.tag == "Player1" && gameManager.CurrentPlayer == (int)PieceTeam.Blue)
                {
                    // Player1のターンでPlayer1の駒をつかんでいる場合の処理
                    originalPosition = hit.transform.position;  // ここで位置を保存
                    isGrabbing = true;
                    sphere = hit.transform;
                }
                else if (hit.collider.tag == "Player2" && gameManager.CurrentPlayer == (int)PieceTeam.Orange)
                {
                    // Player2のターンでPlayer2の駒をつかんでいる場合の処理
                    // ここで駒の前回の位置を保存
                    previousPosition = hit.transform.position;
                    originalPosition = hit.transform.position;  // ここで位置を保存
                    isGrabbing = true;
                    sphere = hit.transform;
                }
            }
        }

        // マウスをクリックしながら移動させる処理
        if (isGrabbing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            plane.Raycast(ray, out rayDistance);

            // マウス位置に応じてオブジェクトを移動
            sphere.position = ray.GetPoint(rayDistance);

            // マウス左ボタンが離されたときの処理
            if (Input.GetMouseButtonUp(0))
            {
                isGrabbing = false; // つかんでいる状態を解除


                // 衝突判定
                RaycastHit groundHit;
                if (Physics.Raycast(sphere.position, Vector3.down, out groundHit, Mathf.Infinity))
                {
                    Vector3 newPiecePosition; // ここで宣言

                    // もし衝突したオブジェクトのタグが"Ground"であれば
                    if (groundHit.collider.CompareTag("Ground"))
                    {
                        // そのマス目に別の駒が存在する場合の処理
                        Piece existingPiece = groundHit.collider.GetComponent<Piece>();
                        if (existingPiece)
                        {
                            // 駒を比較して新しい駒の方が強いかチェック
                            if (sphere.GetComponent<Piece>().GetStrength() > existingPiece.GetStrength())
                            {
                                existingPiece.gameObject.SetActive(false); // 既存の駒を非アクティブに
                            }
                            else
                            {
                                sphere.position = sphere.GetComponent<Piece>().originalPosition; // 新しい駒を元の位置に戻す
                                return;
                            }
                        }

                        // 駒の速度を半分にする
                        sphere.GetComponent<Rigidbody>().velocity *= 0.5f;

                        // オブジェクトの名前に応じて、新しい駒の位置を設定
                        switch (groundHit.collider.name)
                        {
                            case "cube1-1":
                                newPiecePosition = new Vector3(0, 2, 0);
                                break;
                            case "cube1-2":
                                newPiecePosition = new Vector3(1.25f, 2, 0);
                                break;
                            case "cube1-3":
                                newPiecePosition = new Vector3(2.5f, 2, 0);
                                break;
                            case "cube2-1":
                                newPiecePosition = new Vector3(0, 2, 1.25f);
                                break;
                            case "cube2-2":
                                newPiecePosition = new Vector3(1.25f, 2, 1.25f);
                                break;
                            case "cube2-3":
                                newPiecePosition = new Vector3(2.5f, 2, 1.25f);
                                break;
                            case "cube3-1":
                                newPiecePosition = new Vector3(0, 2, 2.5f);
                                break;
                            case "cube3-2":
                                newPiecePosition = new Vector3(1.25f, 2, 2.5f);
                                break;
                            case "cube3-3":
                                newPiecePosition = new Vector3(2.5f, 2, 2.5f);
                                break;

                            // 他のケースについても同様に追加してください。
                            // ...
                            default:
                                return;
                        }

                        sphere.position = newPiecePosition;
                        // もし衝突したオブジェクトのタグが"Ground"であれば
                        if (groundHit.collider.CompareTag("Ground"))
                        {
                            // ターンを切り替える
                            gameManager.SwitchTurn();
                        }
                    }
                }

                 
            }               
        }
    }
}