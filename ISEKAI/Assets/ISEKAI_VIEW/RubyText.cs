// BEGIN MIT LICENSE BLOCK //
//
// Copyright (c) 2017 dskjal
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//
// END MIT LICENSE BLOCK   //
/*
 * *注意*
 * 改行の処理はしてない．ルビを必要とする部分が途中で改行されないよう処理する必要がある．
 * http://dskjal.com/unity/detect-unity-ugui-break-pos.html を参照．
 */
/*
 * 設定方法
 * uGUI の Text にこのスクリプトをつける．
 * テキストのセンタリング設定をした uGUI の Text のプレハブを作り TextPrefab にセット
 * プレハブのフォントサイズをテキストのフォントサイズの 1/2 ぐらいにする
 */
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text), typeof(RectTransform))]
public class RubyText : MonoBehaviour
{

    Text text;
    RectTransform rt;
    public GameObject TextPrefab;  // テキストはセンタリングしておくこと

    class RubyPos
    {
        public int start;   // ルビの開始インデックス
        public int end;     // ルビの終了インデックス
        public string ruby; // ルビ
        public RubyPos(int start, int end, string ruby)
        {
            this.start = start;
            this.end = end;
            this.ruby = ruby;
        }
    }
    void Awake()
    {
        text = GetComponent<Text>();
        rt = GetComponent<RectTransform>();

        text.text = "ルビが必要な例文章です．";
        RubyPos[] rubyPos = new RubyPos[] {
            new RubyPos(3, 4, "ひつよう"),
            new RubyPos(6, 8, "れいぶんしょう")
        };

        var generator = new TextGenerator();
        // テキストのレンダリング位置の計算
        generator.Populate(text.text, text.GetGenerationSettings(rt.sizeDelta));

        // 各文字のレンダリング位置を記録した文字配列の取得
        var charArray = generator.GetCharactersArray();
        foreach (var ruby in rubyPos)
        {
            var start = charArray[ruby.start].cursorPos;
            var end = charArray[ruby.end].cursorPos;
            end.x += charArray[ruby.end].charWidth;

            PlaceRuby(start.x + (end.x - start.x) / 2f, start.y, ruby.ruby);
        }
    }

    // TextPrefab をインスタンス化して配置する
    void PlaceRuby(float x, float y, string text)
    {
        var o = GameObject.Instantiate(TextPrefab);
        o.name = text;
        o.transform.SetParent(this.transform);
        var prt = o.GetComponent<RectTransform>();
        prt.localPosition = new Vector3(x, y + 20f, 0f);

        o.GetComponent<Text>().text = text;
    }
}