using UnityEngine;

/// <summary>
/// ゲーム内で使用されるカラーを一発で取得できる継承用 MonoBehaviour
/// </summary>
public class ColorPallet : MonoBehaviour
{
    Color _positive = Color.red;
    Color _negative = Color.blue;
    Color _base = new Color32(202, 238, 251, 255);
    Color _main = new Color32(242, 207, 242, 255);
    Color _accent = Color.yellow;
    Color _void = new Color32(191, 191, 191, 255);
    Color _anger = new Color32(255, 112, 192, 255);
    Color _grudge = new Color32(112, 48, 160, 255);
    Color _hatred = new Color32(192, 188, 0, 255);
    Color _suspicion = new Color32(11, 48, 65, 255);

    protected Color _Positive => _positive;
    protected Color _Negative => _negative;
    protected Color _Base => _base;
    protected Color _Main => _main;
    protected Color _Accent => _accent;
    protected Color _Void => _void;
    protected Color _Anger => _anger;
    protected Color _Grudge => _grudge;
    protected Color _Hatred => _hatred;
    protected Color _Suspicion => _suspicion;
    /// <summary>
    /// (color, 求めているアルファ値)を引数に指定すると、アルファの値だけが変更されてColorが返ってくるメソッド
    /// </summary>
    /// <param name="color"></param>
    /// <param name="alpha"></param>
    /// <returns></returns>
    protected Color ChangeAlpha(Color color, int alpha) => new (color.r, color.g, color.b, (float)alpha / 255);
}