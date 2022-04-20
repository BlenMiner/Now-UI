using UnityEngine;

public class ShapedRectangles : MonoBehaviour
{
    [Range(1, 200)]
    public int COUNT = 16;

    [SerializeField] Color outline = Color.red;

    [SerializeField, Range(-200, 200)] float m_radius;

    [SerializeField, Range(0, 200)] float m_outline;

    [SerializeField, Range(-200, 200)] float m_blur;

    [SerializeField, Range(-200, 200)] float m_padding;

    private void OnPostRender()
    {
        NowUI.BeingUI();

        float SIZEX = (float)Screen.width / COUNT;
        float SIZEY = (float)Screen.height / COUNT;

        var style = NowUI.Rectangle(default)
            .SetOutlineColor(outline)
            .SetBlur(m_blur)
            .SetRadius(m_radius)
            .SetPadding(m_padding)
            .SetOutline(m_outline);

        for (int x = 0; x < COUNT; ++x)
        {
            for (int y = 0; y < COUNT; ++y)
            {
                var rect = new Vector4(SIZEX * x, SIZEY * y, SIZEX, SIZEY);
                if ((x + y) % 2 == 0)
                {
                    style.SetPosition(rect)
                        .Draw();
                }
            }
        }

        NowUI.EndUI();
    }
}
