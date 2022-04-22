using UnityEngine;

public class TextTests : MonoBehaviour
{
    [SerializeField] bool m_useGUI = false;

    [SerializeField] NowFont m_font;

    [SerializeField] Color m_color;

    [SerializeField, Range(1, 200)] float m_fontSize = 100;

    [SerializeField, Range(-200, 200)] float m_padding;

    [SerializeField, TextArea] string m_text = "Hello World";

    private void OnGUI()
    {
        if (m_useGUI)
        {
            GUI.Label(new Rect(20, 20, 10000, 15000), m_text);
        }
    }

    private void OnPostRender()
    {
        NowUI.BeingUI();

        NowUI.Rectangle(new Vector4(10, 10, 100, 150))
            .SetColor(new Color(0.1f, 0.1f, 0.1f, 1f))
            .Draw();

        if (!m_useGUI)
        {
            NowUI.Text(new Vector4(20, 20, 100, 150), m_font)
                .SetColor(m_color)
                .SetFontSize(m_fontSize)
                .SetPadding(m_padding)
                .Draw(m_text);
        }

        NowUI.EndUI();
    }
}
