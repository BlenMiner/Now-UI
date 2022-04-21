using UnityEngine;

public class TextTests : MonoBehaviour
{
    [SerializeField] NowFont m_font;

    [SerializeField, Range(1, 200)] float m_fontSize = 100;

    private void OnPostRender()
    {
        NowUI.BeingUI();

        NowUI.Rectangle(new Vector4(10, 10, 100, 150))
            .SetColor(new Color(0.1f, 0.1f, 0.1f, 1f))
            .Draw();

        NowUI.DrawString(new Vector4(20, 20, 100, 150), "Hello World", m_font, m_fontSize);

        NowUI.EndUI();
    }
}
