using UnityEngine;

public class ShapedRectangles : MonoBehaviour
{
    [SerializeField] Color outline = Color.red;

    [SerializeField, Range(-200, 200)] float m_radius;

    [SerializeField, Range(0, 200)] float m_outline;

    [SerializeField, Range(-200, 200)] float m_blur;

    [SerializeField, Range(-200, 200)] float m_padding;

    private void OnPostRender()
    {
        NowUI.BeingUI();

        NowUI.Rectangle(new Rect(50, 50, 200, 80))
            .SetOutlineColor(outline)
            .SetBlur(m_blur)
            .SetRadius(m_radius)
            .SetPadding(m_padding)
            .SetOutline(m_outline)
            .Draw()
            .SetBlur(m_blur)
            .SetPosition(new Rect(70, 70, 200, 80))
            .SetColor(Color.red)
            .Draw()
            .SetPosition(new Rect(90, 90, 200, 80))
            .SetColor(Color.yellow)
            .Draw();

        NowUI.EndUI();
    }
}
