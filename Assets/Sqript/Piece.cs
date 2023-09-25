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
                    isGrabbing = true;
                    sphere = hit.transform;
                }
                else if (hit.collider.tag == "Player2" && gameManager.CurrentPlayer == (int)PieceTeam.Orange)
                {
                    // Player2のターンでPlayer2の駒をつかんでいる場合の処理
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
                    // もし衝突したオブジェクトのタグが"Ground"であれば
                    if (groundHit.collider.name == "cube1-1")
                    {
                        // 駒の速度を半分にする
                        sphere.GetComponent<Rigidbody>().velocity *= 0.5f;
                        // 駒の位置を地面の位置に合わせる
                        Vector3 newPiecePosition = new Vector3(0, 2, 0);
                        sphere.position = newPiecePosition;
                    }
                    if (groundHit.collider.name == "cube1-2")
                    {
                        // 駒の速度を半分にする
                        sphere.GetComponent<Rigidbody>().velocity *= 0.5f;
                        // 駒の位置を地面の位置に合わせる
                        Vector3 newPiecePosition = new Vector3(1.25f, 2, 0);
                        sphere.position = newPiecePosition;
                    }
                    if (groundHit.collider.name == "cube1-3")
                    {
                        // 駒の速度を半分にする
                        sphere.GetComponent<Rigidbody>().velocity *= 0.5f;
                        // 駒の位置を地面の位置に合わせる
                        Vector3 newPiecePosition = new Vector3(2.5f, 2, 0);
                        sphere.position = newPiecePosition;
                    }
                    if (groundHit.collider.name == "cube2-1")
                    {
                        // 駒の速度を半分にする
                        sphere.GetComponent<Rigidbody>().velocity *= 0.5f;
                        // 駒の位置を地面の位置に合わせる
                        Vector3 newPiecePosition = new Vector3(0, 2, 1.25f);
                        sphere.position = newPiecePosition;
                    }
                    if (groundHit.collider.name == "cube2-2")
                    {
                        // 駒の速度を半分にする
                        sphere.GetComponent<Rigidbody>().velocity *= 0.5f;
                        // 駒の位置を地面の位置に合わせる
                        Vector3 newPiecePosition = new Vector3(1.25f, 2, 1.25f);
                        sphere.position = newPiecePosition;
                    }
                    if (groundHit.collider.name == "cube2-3")
                    {
                        // 駒の速度を半分にする
                        sphere.GetComponent<Rigidbody>().velocity *= 0.5f;
                        // 駒の位置を地面の位置に合わせる
                        Vector3 newPiecePosition = new Vector3(2.5f, 2, 1.25f);
                        sphere.position = newPiecePosition;
                    }
                    if (groundHit.collider.name == "cube3-1")
                    {
                        // 駒の速度を半分にする
                        sphere.GetComponent<Rigidbody>().velocity *= 0.5f;
                        // 駒の位置を地面の位置に合わせる
                        Vector3 newPiecePosition = new Vector3(0, 2, 2.5f);
                        sphere.position = newPiecePosition;
                    }
                    if (groundHit.collider.name == "cube3-2")
                    {
                        // 駒の速度を半分にする
                        sphere.GetComponent<Rigidbody>().velocity *= 0.5f;
                        // 駒の位置を地面の位置に合わせる
                        Vector3 newPiecePosition = new Vector3(1.25f, 2, 2.5f);
                        sphere.position = newPiecePosition;
                    }
                    if (groundHit.collider.name == "cube3-3")
                    {
                        // 駒の速度を半分にする
                        sphere.GetComponent<Rigidbody>().velocity *= 0.5f;
                        // 駒の位置を地面の位置に合わせる
                        Vector3 newPiecePosition = new Vector3(2.5f, 2, 2.5f);
                        sphere.position = newPiecePosition;
                    }
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