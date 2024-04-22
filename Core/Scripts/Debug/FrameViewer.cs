using UnityEngine;

namespace Roguelike.Core
{
    public class FrameViewer : MonoBehaviour
    {
        [Range(1, 100)]
        [SerializeField] private int fontSize;
        [Range(0, 1)]
        [SerializeField] private float red, green, blue;
        [SerializeField] private Variable<int> monsterCount;

        private float deltaTime = 0.0f;

        private void Start()
        {
            
        }

        private void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            float height = 0f;
            DrawText(text, ref height);
            DrawText($"Monster: {monsterCount.BoxedValue}", ref height);
        }

        private void DrawText(string text, ref float height)
        {
            GUIStyle style = new GUIStyle();
            Rect rect = new Rect(0, height, Screen.width, Screen.height * 0.02f);
            height += fontSize;
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = fontSize;
            style.normal.textColor = new Color(red, green, blue, 1.0f);
            GUI.Label(rect, text, style);
        }
    }
}
