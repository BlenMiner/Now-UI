using UnityEngine;

public class TextTests : MonoBehaviour
{
    [SerializeField] NowFont m_font;

    [SerializeField] Color m_color;

    [SerializeField] Color m_outOolor;

    [SerializeField, Range(-100, 100)] float m_outline = 0f;

    [SerializeField, Range(1, 200)] float m_fontSize = 100;

    [SerializeField, Range(-200, 200)] float m_padding;

    [SerializeField, TextArea] string m_text = "Hello World";

    private void OnPostRender()
    {
        NowUI.StartUI();

        NowUI.Rectangle(new Vector4(10, 10, 100, 150))
            .SetColor(new Color(0.1f, 0.1f, 0.1f, 1f))
            .Draw();

        NowUI.Text(new Vector4(10, 10, 100, 150), m_font)
            .SetColor(m_color)
            .SetFontSize(m_fontSize)
            .SetPadding(m_padding)
            .SetOutline(m_outline)
            .SetOutlineColor(m_outOolor)
            .Draw(m_text);

        NowUI.FlushUI();
    }
}
