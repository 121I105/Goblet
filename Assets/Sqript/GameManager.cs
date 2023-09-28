using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int[,] squares = new int[3, 3];  // 3x3のゲームボードの状態を表す配列

    private const int EMPTY = 0;  // ゲームボードの空きスペースを表す定数
    private const int Blue = 1;   // 青プレイヤーを表す定数
    private const int Orange = -1; // オレンジプレイヤーを表す定数

    private int currentPlayer = Blue;  // 現在のプレイヤー（初期値は青）
    private Camera camera_object;  // カメラオブジェクト
    private RaycastHit hit;  // Raycastの結果を格納するオブジェクト
   



    public GameObject BlueBig1;
    public GameObject BlueBig2;
    public GameObject BlueMedium1;
    public GameObject BlueMedium2;
    public GameObject BlueSmall1;
    public GameObject BlueSmall2;
    public GameObject OrangeBig1;
    public GameObject OrangeBig2;
    public GameObject OrangeMedium1;
    public GameObject OrangeMedium2;
    public GameObject OrangeSmall1;
    public GameObject OrangeSmall2;

    

//    private Dictionary<string, Vector3> cellNameToPosition = new Dictionary<string, Vector3>
//{
//    { "cube1-1", new Vector3(0, 2, 0) },
//    { "cube1-2", new Vector3(1.25f, 2, 0) },
//    { "cube1-3", new Vector3(2.5f, 2, 0) },

//    { "cube2-1", new Vector3(0, 1.25f, 0) },
//    { "cube2-2", new Vector3(1.25f, 1.25f, 0) },
//    { "cube2-3", new Vector3(2.5f, 1.25f, 0) },

//    { "cube3-1", new Vector3(0, 0.5f, 0) },
//    { "cube3-2", new Vector3(1.25f, 0.5f, 0) },
//    { "cube3-3", new Vector3(2.5f, 0.5f, 0) }
//};


    public global::System.Int32 CurrentPlayer { get => currentPlayer; set => currentPlayer = value; }
    private bool isPlayer1Turn = true; // Player1のターンから始める

    // Start is called before the first frame update
    void Start()
    {
        camera_object = GameObject.Find("Main Camera").GetComponent<Camera>();  // シーン内のメインカメラを取得

        InitializeArray();  // ゲームボードの初期化

        // 最初に青のターンにする
        // 最初のターンを設定
        CurrentPlayer = (int)PieceTeam.Blue; // 青のターンに設定
        Debug.Log("現在のターン: 青");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckStone(PieceTeam.Blue) || CheckStone(PieceTeam.Orange))  // 青かオレンジのどちらかが勝利している場合、入力を受け付けない
        {
            return;
        }
    }

    public void AttemptToCaptureSquare(Vector3 targetPosition, GameObject sphere)
    {
        // 目的地のマス目にある駒を取得
        Piece movingPiece = sphere.GetComponent<Piece>();
        Piece existingPiece = GetPieceOnSquare(targetPosition);

        // マス目に駒があり、移動しようとしている駒が既存の駒よりも強力である場合
        if (existingPiece != null && movingPiece.IsStrongerThan(existingPiece))
        {
            // 乗っ取られた駒の情報を保存
            capturedPieces.Add(new CapturedPieceInfo
            {
                piece = existingPiece.gameObject,
                originalPosition = existingPiece.transform.position
            });

            // 既存の駒を非アクティブに
            existingPiece.gameObject.SetActive(false);
        }

        // その後、sphereを目的地に移動させるロジックもここに追加することができます
    }




    //private Piece GetPieceAtPosition(Vector3 position)
    //{
    //    int x = Mathf.FloorToInt(position.x);
    //    int y = Mathf.FloorToInt(position.y);

    //    if (x >= 0 && x < 3 && y >= 0 && y < 3)
    //    {
    //        return boardSquares[x, y].CurrentPiece;
    //    }

    //    return null;
    //}

    //public void MoveSphereToPosition(Transform sphere, RaycastHit groundHit)
    //{
    //    if (cellNameToPosition.TryGetValue(groundHit.collider.name, out Vector3 newPiecePosition))
    //    {
    //        sphere.GetComponent<Rigidbody>().velocity *= 0.5f;
    //        sphere.position = newPiecePosition;
    //    }
    //}



    private bool CheckStone(PieceTeam color)
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
                if (piece == null || piece.team != color || !piece.gameObject.activeSelf)
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
                if (piece == null || piece.team != color || !piece.gameObject.activeSelf)
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
            if (piece != null && piece.team == color && piece.gameObject.activeSelf)
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
            if (piece != null && piece.team == color && piece.gameObject.activeSelf)
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
            if (color == PieceTeam.Blue)
            {
                Debug.Log("青の勝ち");
            }
            else if (color == PieceTeam.Orange)
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


    // 与えられた3D座標位置（position）からPieceを取得する関数
    private Piece GetPieceOnSquare(Vector3 position)
    {
        // Rayが当たったオブジェクトを格納するための配列を宣言
        RaycastHit[] hits = Physics.RaycastAll(position, Vector3.up, 1.0f);

        // Rayが当たったオブジェクトを1つずつチェック
        foreach (RaycastHit hit in hits)
        {
            // 当たったオブジェクトからPieceコンポーネントを取得
            Piece piece = hit.collider.gameObject.GetComponent<Piece>();
            
            // Pieceコンポーネントが取得できた場合
            if (piece != null)
            {
                // ピースを見つけたらそれを返す
                return piece;
            }
        }

        // ピースが見つからなかった場合はnullを返す
        return null;
    }

    // Player1とPlayer2のターンを交互に切り替えるメソッド
    public void SwitchTurn()
    {
        if (isPlayer1Turn)
        {
            // Player1のターン終了
            isPlayer1Turn = false;
            CurrentPlayer = (int)PieceTeam.Orange; // オレンジのターンに切り替え
            Debug.Log("現在のターン: オレンジ");
        }
        else
        {
            // Player2のターン終了
            isPlayer1Turn = true;
            CurrentPlayer = (int)PieceTeam.Blue; // 青のターンに切り替え
            Debug.Log("現在のターン: 青");
        }
    }
}