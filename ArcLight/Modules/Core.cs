using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public static class Core
    {

        public static Scene Scene;
        public static GameSession Session;
        public static bool test_game = false;
        public static bool god_mode = false; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        public static void Initialize()
        {
            Input.Initialize();
        }

        public static void Update()
        {
            Input.Update();
            if (Scene != null)
                Scene.Update();
            Graphics.Update();
            Audio.Update();
            Data.Update();
        }

        public static void StartScene(Scene scene)
        {
            if (Scene != null)
            {
                EndScene();
            }
            Scene = scene;
            Scene.Start();
        }

        public static void EndScene()
        {
            Scene.End();
            Scene = null;
        }

        public static void ClearSession()
        {
            Session = null;
        }

        public static void StartNewTestGame()
        {
            test_game = true;
            Session = new GameSession(true, 0, 0, false, 1, 0);
            StartScene(new GameScene());
        }

        public static void StartNewGame(bool b1, int i1, int i2, bool b2, int i3, int i4)
        {
            Session = new GameSession(b1, i1, i2, b2, i3, i4);
            StartScene(new GameScene());
        }

        public static void StartGame()
        {
            //StartNewTestGame();
            StartScene(new LogoScene());
        }

        public static GameController Controller { get { return ((GameScene)Scene).controller; } }

    }
}
