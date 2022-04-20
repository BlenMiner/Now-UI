using UnityEngine;

public class SimpleRectangle : MonoBehaviour
{
    public bool useOnGUI = false;

    [Range(1, 500)]
    public int COUNT = 16;

    Texture2D white;

    private void Awake()
    {
        white = new Texture2D(1, 1);
        white.SetPixel(0, 0, Color.white);
        white.Apply();
    }

    private void DrawRectGUI(Rect rect)
    {
        GUI.DrawTexture(rect, white);
    }

    private void OnGUI()
    {
        if (useOnGUI)
        {
            float SIZEX = (float)Screen.width / COUNT;
            float SIZEY = (float)Screen.height / COUNT;

            for (int x = 0; x < COUNT; ++x)
            {
                for (int y = 0; y < COUNT; ++y)
                {
                    var rect = new Rect(SIZEX * x, SIZEY * y, SIZEX, SIZEY);
                    if ((x + y) % 2 == 0)
                        GUI.DrawTexture(rect, white);
                }
            }
        }
    }

    private void OnPostRender()
    {
        if (!useOnGUI)
        {
            NowUI.BeingUI();
            {
                float SIZEX = (float)Screen.width / COUNT;
                float SIZEY = (float)Screen.height / COUNT;

                for (int x = 0; x < COUNT; ++x)
                {
                    for (int y = 0; y < COUNT; ++y)
                    {
                        var rect = new Rect(SIZEX * x, SIZEY * y, SIZEX, SIZEY);
                        if ((x + y) % 2 == 0)
                            NowUI.DrawRect(rect);
                    }
                }
            }
            NowUI.EndUI();
        }
    }
}
