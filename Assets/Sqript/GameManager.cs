using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int[,] squares = new int[3,3];  // 3x3のゲームボードの状態を表す配列

    private const int EMPTY = 0;  // ゲームボードの空きスペースを表す定数
    private const int Blue = 1;   // 青プレイヤーを表す定数
    private const int Orange = -1; // オレンジプレイヤーを表す定数

    private int currentPlayer = Blue;  // 現在のプレイヤー（初期値は青）
    private Camera camera_object;  // カメラオブジェクト
    private RaycastHit hit;  // Raycastの結果を格納するオブジェクト

    public GameObject BlueBig1;  // 青プレイヤーの石のプレハブ
    public GameObject OrangeBig1; // オレンジプレイヤーの石のプレハブ

    public global::System.Int32 CurrentPlayer { get => currentPlayer; set => currentPlayer = value; }


    // Start is called before the first frame update
    void Start()
    {
        camera_object = GameObject.Find("Main Camera").GetComponent<Camera>();  // シーン内のメインカメラを取得

        InitializeArray();  // ゲームボードの初期化
    }


    // Update is called once per frame
    void Update()
    {
        if (CheckStone(Blue) || CheckStone(Orange))  // 青かオレンジのどちらかが勝利している場合、入力を受け付けない
        {
            return;
        }

        /*if (Input.GetMouseButtonDown(0))  // マウスの左ボタンがクリックされた場合
        {
            Ray ray = camera_object.ScreenPointToRay(Input.mousePosition);  // マウスの位置からRayを作成

            if (Physics.Raycast(ray, out hit))  // Rayがオブジェクトに当たったかどうかの判定
            {
                int x = (int)hit.collider.gameObject.transform.position.x;  // 当たったオブジェクトの位置からx座標を取得
                int z = (int)hit.collider.gameObject.transform.position.z;  // 当たったオブジェクトの位置からz座標を取得

                if (squares[z, x] == EMPTY)  // クリックされた位置が空きスペースの場合
                {
                    if (currentPlayer == Blue)  // 現在のプレイヤーが青の場合
                    {
                        squares[z, x] = Blue;  // ゲームボードの位置を青に設定

                        GameObject stone = Instantiate(BlueBig1);  // 青の石のプレハブからオブジェクトを生成
                        stone.transform.position = hit.collider.gameObject.transform.position;  // 生成したオブジェクトの位置を設定

                        currentPlayer = Orange;  // プレイヤーをオレンジに切り替え
                    }
                    else if (currentPlayer == Orange)  // 現在のプレイヤーがオレンジの場合
                    {
                        squares[z, x] = Orange;  // ゲームボードの位置をオレンジに設定

                        GameObject stone = Instantiate(OrangeBig1);  // オレンジの石のプレハブからオブジェクトを生成
                        stone.transform.position = hit.collider.gameObject.transform.position;  // 生成したオブジェクトの位置を設定

                        currentPlayer = Blue;  // プレイヤーを青に切り替え
                    }
                }
            }
        }*/
        
    }

      

    private bool CheckStone(int color)
    {
        int count = 0;
        bool hasWon = false;

        // 横方向のラインをチェック
        for (int i = 0; i < 3; i++)
        {
            count = 0;  // カウントのリセット
            for (int j = 0; j < 3; j++)
            {
                Piece piece = GetPieceOnSquare(new Vector3(i, j, 0));
                if (piece == null || piece.Color != color || !piece.gameObject.activeSelf)
                {
                    count = 0;
                }
                else
                {
                    count++;
                }

                if (count == 3)
                {
                    hasWon = true;
                    break;
                }
            }
        }

        // 縦方向のラインをチェック
        for (int i = 0; i < 3 && !hasWon; i++)
        {
            count = 0;
            for (int j = 0; j < 3; j++)
            {
                Piece piece = GetPieceOnSquare(new Vector3(j, i, 0));
                if (piece == null || piece.Color != color || !piece.gameObject.activeSelf)
                {
                    count = 0;
                }
                else
                {
                    count++;
                }

                if (count == 3)
                {
                    hasWon = true;
                    break;
                }
            }
        }

        // 斜めの勝利判定 (左上から右下)
        count = 0;
        for (int i = 0; i < 3 && !hasWon; i++)
        {
            Piece piece = GetPieceOnSquare(new Vector3(i, i, 0));
            if (piece != null && piece.Color == color && piece.gameObject.activeSelf)
            {
                count++;
                if (count == 3)
                {
                    hasWon = true;
                    break;
                }
            }
            else
            {
                count = 0;
            }
        }

        // 斜めの勝利判定 (左下から右上)
        count = 0;
        for (int i = 0; i < 3 && !hasWon; i++)
        {
            Piece piece = GetPieceOnSquare(new Vector3(i, 2 - i, 0));
            if (piece != null && piece.Color == color && piece.gameObject.activeSelf)
            {
                count++;
                if (count == 3)
                {
                    hasWon = true;
                    break;
                }
            }
            else
            {
                count = 0;
            }
        }

        if (hasWon)
        {
            if (color == Blue)
            {
                Debug.Log("青の勝ち");
            }
            else
            {
                Debug.Log("オレンジの勝ち");
            }
        }

        return hasWon;
    }


    private void InitializeArray()
    {
        for (int i = 0; i < 3; i++)  // ゲームボードの状態を空きスペースで初期化
        {
            for (int j = 0; j < 3; j++)
            {
                squares[i, j] = EMPTY;
            }
        }
    }
}
