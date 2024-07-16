using System;
using SplashKitSDK;

namespace SketchQuest
{
    public class Program
    {
        public static void Main()
        {
            GameManager _sketchQuest = GameManager.GetInstance();
            Window window = new Window("SketchQuest", 1003, 600);
            SplashKitSDK.Timer timer = new SplashKitSDK.Timer("GameTimer");
            timer.Start();
            SplashKit.PlayMusic(AssetManager.GetInstance().GetMusic("main-menu"), 99);

            while (!window.CloseRequested)
            {
                SplashKit.ClearScreen();
                SplashKit.ProcessEvents();

                // Handle user input
                if (SplashKit.MouseClicked(MouseButton.LeftButton))
                {
                    _sketchQuest.Click(SplashKit.MousePosition());
                }

                if (SplashKit.MouseDown(MouseButton.LeftButton))
                {
                    _sketchQuest.MouseDown(SplashKit.MousePosition());
                }

                // Update game timer
                if (Timer.GetInstance().SecondsRemaining > 0 && timer.Ticks > 1000)
                {
                    Timer.GetInstance().Tick();
                    timer.Reset();
                }

                _sketchQuest.Draw();

                SplashKit.RefreshScreen(120);
            }

        }
    }
}
