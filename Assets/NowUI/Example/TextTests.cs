using UnityEngine;

public class TextTests : MonoBehaviour
{
    [SerializeField] NowFont m_font;

    private void OnPostRender()
    {
        NowUI.BeingUI();

        NowUI.Rectangle(new Vector4(10, 10, 100, 150))
            .SetColor(new Color(0.1f, 0.1f, 0.1f, 1f))
            .Draw();

        NowUI.DrawCharacter(new Vector4(20, 20, 100 - 20, 150 - 20), 'F', m_font);

        NowUI.EndUI();
    }
}
