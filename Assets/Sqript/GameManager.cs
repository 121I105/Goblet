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

            if (piece != null)
            {
                Debug.Log("Found piece with tag " + piece.gameObject.tag);
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