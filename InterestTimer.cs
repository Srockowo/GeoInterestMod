using Modding;
using UnityEngine;
using UnityEngine.UI;

namespace GeoInterestMod
{
    public class InterestTimer
    {
        public static GameObject timerCanvas;
        private static Text timerDisplay;

        public static int width = 240;
        public static int height = 40;

        public static string textToDisplay { get; set; }

        public static void Initialize()
        {
            if (timerCanvas != null) return;

            CanvasUtil.RectData rect = new CanvasUtil.RectData(
                new Vector2(width, height),
                new Vector2(-740, 300)
            );

            timerCanvas = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceOverlay, new Vector2(1920, 1080));
            timerDisplay = CanvasUtil.CreateTextPanel(
                timerCanvas,
                textToDisplay,
                30,
                TextAnchor.MiddleCenter,
                rect
            ).GetComponent<Text>();

            Object.DontDestroyOnLoad(timerCanvas);
            Object.DontDestroyOnLoad(timerDisplay);
        }

        public static void UpdateText(double timeRemaining)
        {
            if (timeRemaining <= 0) timeRemaining = 0;

            // format to 2 decimal places.
            timerDisplay.text = $"{timeRemaining:0.00}s";
        }

        public static void SetActive(bool state)
        {
            if (HeroController.instance == null) return;

            if (timerCanvas != null) timerCanvas.SetActive(state);
        }

        public static void Destroy()
        {
            Object.Destroy(timerCanvas);
            Object.Destroy(timerDisplay);
        }
    }
}