using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private bool gameEnded = false; // ゲームが終了したかどうかを示すフラグ
    public GameObject player1;
    public GameObject player2;
    public GameObject restart;
    public GameObject title;

    // Start is called before the first frame update
    void Start()
    {
        camera_object = GameObject.Find("Main Camera").GetComponent<Camera>();  // シーン内のメインカメラを取得

        // 最初に青のターンにする
        // 最初のターンを設定
        CurrentPlayer = (int)PieceTeam.Blue; // 青のターンに設定
        Debug.Log("現在のターン: 青");

        player1.SetActive(false);
        player2.SetActive(false);
        restart.SetActive(false);
        title.SetActive(false);
    }

    // 勝利条件を判定するメソッド
    private bool CheckWinCondition(int playerTag)
    {
        Vector3[] winPositionsGroup1 = new Vector3[]
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(1.25f, 0f, 0f),
            new Vector3(2.5f, 0f, 0f)
        };

        Vector3[] winPositionsGroup2 = new Vector3[]
        {
            new Vector3(0f, 0f, 1.25f),
            new Vector3(1.25f, 0f, 1.25f),
            new Vector3(2.5f, 0f, 1.25f)
        };

        Vector3[] winPositionsGroup3 = new Vector3[]
        {
            new Vector3(0f, 0f, 2.5f),
            new Vector3(1.25f, 0f, 2.5f),
            new Vector3(2.5f, 0f, 2.5f)
        };

        Vector3[] winPositionsGroup4 = new Vector3[]
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 1.25f),
            new Vector3(0f, 0f, 2.5f)
        };

        Vector3[] winPositionsGroup5 = new Vector3[]
        {
            new Vector3(1.25f, 0f, 0f),
            new Vector3(1.25f, 0f, 1.25f),
            new Vector3(1.25f, 0f, 2.5f)
        };

        Vector3[] winPositionsGroup6 = new Vector3[]
        {
            new Vector3(2.5f, 0f, 0f),
            new Vector3(2.5f, 0f, 1.25f),
            new Vector3(2.5f, 0f, 2.5f)
        };

        Vector3[] winPositionsGroup7 = new Vector3[]
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(1.25f, 0f, 1.25f),
            new Vector3(2.5f, 0f, 2.5f)
        };

        Vector3[] winPositionsGroup8 = new Vector3[]
        {
            new Vector3(0f, 0f, 2.5f),
            new Vector3(1.25f, 0f, 1.25f),
            new Vector3(2.5f, 0f, 0f)
        };

        if (CheckWinGroup(winPositionsGroup1, playerTag) || CheckWinGroup(winPositionsGroup2, playerTag) || CheckWinGroup(winPositionsGroup3, playerTag) || CheckWinGroup(winPositionsGroup4, playerTag) || CheckWinGroup(winPositionsGroup5, playerTag) || CheckWinGroup(winPositionsGroup6, playerTag) || CheckWinGroup(winPositionsGroup7, playerTag) || CheckWinGroup(winPositionsGroup8, playerTag))        {
            return true;
        }

        return false;
    }

    private bool CheckWinGroup(Vector3[] winPositions, int playerTag)
    {
        int winCount = 0;

        foreach (Vector3 position in winPositions)
        {
            int maxStrength = 0;

            Collider[] colliders = Physics.OverlapBox(position, new Vector3(0.6f, 0.6f, 0.6f));
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Player" + playerTag))
                {
                    Strength strengthComponent = collider.GetComponent<Strength>();
                    if (strengthComponent != null)
                    {
                        maxStrength = Mathf.Max(maxStrength, strengthComponent.GetStrength());
                    }
                }
            }

            if (maxStrength > 0)
            {
                winCount++;
                if (winCount >= 3)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        // ゲームが既に終了している場合は何もしない
        if (gameEnded)
        {
            return;
        }

        // 勝利条件を判定する
        if (CheckWinCondition(1))
        {
            Debug.Log("Player 1の勝利！");
            gameEnded = true; // ゲーム終了フラグを設定
            player1.SetActive(true);
            restart.SetActive(true);
            title.SetActive(true);
        }
        else if (CheckWinCondition(2))
        {
            Debug.Log("Player 2の勝利！");
            gameEnded = true; // ゲーム終了フラグを設定
            player2.SetActive(true);
            restart.SetActive(true);
            title.SetActive(true);
        }
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

    public void StartButton()
    {
        Debug.Log("ゲームスタート");
        SceneManager.LoadScene("Main");
    }

    public void RestartButton()
    {
        Debug.Log("再戦");
        SceneManager.LoadScene("Main");
    }

    public void ResetButton()
    {
        Debug.Log("タイトルに戻る");
        SceneManager.LoadScene("Title");
    }
}