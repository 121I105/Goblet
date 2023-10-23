using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public int objectNumber; // Inspectorから番号を設定するための変数

    // Start is called before the first frame update
    void Start()
    {
        // オブジェクトの名前を設定
        gameObject.name = objectNumber.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
