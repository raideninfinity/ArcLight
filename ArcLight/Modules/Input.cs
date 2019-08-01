using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ArcLight
{
    public static class Input
    {

        public static void Initialize()
        {
            InitializeKeyboard();
            InitializeGamepad();
        }

        public static void Update()
        {
            UpdateKeyboard();
            UpdateGamepad();
        }

        #region Keyboard

        static KeyboardState keyboardState;
        static KeyboardState lastKeyboardState;


        public static void InitializeKeyboard()
        {
            keyboardState = Keyboard.GetState();
            lastKeyboardState = Keyboard.GetState();
        }

        public static void UpdateKeyboard()
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }

        public static void Flush()
        {
            lastKeyboardState = keyboardState;
        }

        public static bool KeyRelease(Keys key)
        {
            return keyboardState.IsKeyUp(key) &&
                lastKeyboardState.IsKeyDown(key);
        }

        public static bool KeyTrigger(Keys key)
        {
            return keyboardState.IsKeyDown(key) &&
                lastKeyboardState.IsKeyUp(key);
        }

        public static bool KeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public static int KeyboardDir8(Keys[] keys)
        {
            if (KeyDown(keys[0]) && KeyDown(keys[1]))
                return 7;
            if (KeyDown(keys[0]) && KeyDown(keys[3]))
                return 9;
            if (KeyDown(keys[2]) && KeyDown(keys[1]))
                return 1;
            if (KeyDown(keys[2]) && KeyDown(keys[3]))
                return 3;
            if (KeyDown(keys[0]))
                return 8;
            if (KeyDown(keys[1]))
                return 4;
            if (KeyDown(keys[3]))
                return 6;
            if (KeyDown(keys[2]))
                return 2;
            return 0;
        }
        #endregion

        #region Gamepad

        static GamePadState[] padState;
        static GamePadState[] lastPadState;

        public static void InitializeGamepad()
        {
            padState = new GamePadState[] { GamePad.GetState(PlayerIndex.One), GamePad.GetState(PlayerIndex.Two) };
            lastPadState = new GamePadState[] { GamePad.GetState(PlayerIndex.One), GamePad.GetState(PlayerIndex.Two) };
        }

        public static void UpdateGamepad()
        {
            lastPadState[0] = padState[0];
            lastPadState[1] = padState[1];
            padState[0] = GamePad.GetState(PlayerIndex.One);
            padState[1] = GamePad.GetState(PlayerIndex.Two);
        }

        public static bool GamepadOnline(int index)
        {
            return padState[index].IsConnected;
        }

        public static bool ButtonRelease(Buttons button, int index)
        {
            if (!GamepadOnline(index)) return false;
            return padState[index].IsButtonUp(button) &&
               lastPadState[index].IsButtonDown(button);
        }

        public static bool ButtonTrigger(Buttons button, int index)
        {
            if (!GamepadOnline(index)) return false;
            return padState[index].IsButtonDown(button) &&
               lastPadState[index].IsButtonUp(button);
        }

        public static bool ButtonDown(Buttons button, int index)
        {
            if (!GamepadOnline(index)) return false;
            return padState[index].IsButtonDown(button);
        }

        public static bool ButtonReleaseA(Buttons button)
        {
            return ButtonRelease(button, 0) || ButtonRelease(button, 1);
        }

        public static bool ButtonTriggerA(Buttons button)
        {
            return ButtonTrigger(button, 0) || ButtonTrigger(button, 1);
        }

        public static bool ButtonDownA(Buttons button)
        {
            return ButtonDown(button, 0) || ButtonDown(button, 1);
        }

        public static int GamepadDir8(int index)
        {
            if (!GamepadOnline(index)) return 0;
            if (ButtonDown(Buttons.DPadUp, index) && ButtonDown(Buttons.DPadLeft, index))
                return 7;
            if (ButtonDown(Buttons.DPadUp, index) && ButtonDown(Buttons.DPadRight, index))
                return 9;
            if (ButtonDown(Buttons.DPadDown, index) && ButtonDown(Buttons.DPadLeft, index))
                return 1;
            if (ButtonDown(Buttons.DPadDown, index) && ButtonDown(Buttons.DPadRight, index))
                return 3;
            if (ButtonDown(Buttons.DPadUp, index))
                return 8;
            if (ButtonDown(Buttons.DPadLeft, index))
                return 4;
            if (ButtonDown(Buttons.DPadRight, index))
                return 6;
            if (ButtonDown(Buttons.DPadDown, index))
                return 2;
            return 0;
        }

        public static Vector2 LStickAxes(int index)
        {
            if (!GamepadOnline(index)) return Vector2.Zero;
            Vector2 axes = new Vector2(0, 0);
            axes.X = padState[index].ThumbSticks.Left.X;
            axes.Y = padState[index].ThumbSticks.Left.Y;
            return axes;
        }

        public static Vector2 LastLStickAxes(int index)
        {
            if (!GamepadOnline(index)) return Vector2.Zero;
            Vector2 axes = new Vector2(0, 0);
            axes.X = lastPadState[index].ThumbSticks.Left.X;
            axes.Y = lastPadState[index].ThumbSticks.Left.Y;
            return axes;
        }

        public static Vector2 LStickNormalized(int index)
        {
            if (!GamepadOnline(index)) return Vector2.Zero;
            Vector2 axes = LStickAxes(index);
            axes.Normalize();
            return axes;
        }

        public static Vector2 RStickAxes(int index)
        {
            if (!GamepadOnline(index)) return Vector2.Zero;
            Vector2 axes = new Vector2(0, 0);
            axes.X = padState[index].ThumbSticks.Right.X;
            axes.Y = padState[index].ThumbSticks.Right.Y;
            return axes;
        }

        public static Vector2 LastRStickAxes(int index)
        {
            if (!GamepadOnline(index)) return Vector2.Zero;
            Vector2 axes = new Vector2(0, 0);
            axes.X = lastPadState[index].ThumbSticks.Right.X;
            axes.Y = lastPadState[index].ThumbSticks.Right.Y;
            return axes;
        }

        public static Vector2 RStickNormalized(int index)
        {
            if (!GamepadOnline(index)) return Vector2.Zero;
            Vector2 axes = RStickAxes(index);
            axes.Normalize();
            return axes;
        }

        #endregion

        #region Logic

        public static bool TriggerAny()
        {
            for (int i = 0; i < 6; i++)
            {
                if (KeyTrigger(Data.settings.p1KeyboardControls[i])) return true;
                if (KeyTrigger(Data.settings.p2KeyboardControls[i])) return true;
                if (ButtonTrigger(Data.settings.p1GamepadControls[i], 0)) return true;
                if (ButtonTrigger(Data.settings.p2GamepadControls[i], 1)) return true;
            }
            return false;
        }

        public static bool Trigger(int index, int player)
        {
            Keys key;
            Buttons button;
            if (player == 0)
            {
                key = Data.settings.p1KeyboardControls[index];
                button = Data.settings.p1GamepadControls[index];
            }
            else
            {
                key = Data.settings.p2KeyboardControls[index];
                button = Data.settings.p2GamepadControls[index];
            }
            return (KeyTrigger(key) || ButtonTrigger(button, player));
        }

        public static bool Press(int index, int player)
        {
            Keys key;
            Buttons button;
            if (player == 0)
            {
                key = Data.settings.p1KeyboardControls[index];
                button = Data.settings.p1GamepadControls[index];
            }
            else
            {
                key = Data.settings.p2KeyboardControls[index];
                button = Data.settings.p2GamepadControls[index];
            }
            return (KeyDown(key) || ButtonDown(button, player));
        }

        public static bool Release(int index, int player)
        {
            Keys key;
            Buttons button;
            if (player == 0)
            {
                key = Data.settings.p1KeyboardControls[index];
                button = Data.settings.p1GamepadControls[index];
            }
            else
            {
                key = Data.settings.p2KeyboardControls[index];
                button = Data.settings.p2GamepadControls[index];
            }
            return (KeyRelease(key) || ButtonRelease(button, player));
        }

        private static double getAngleFromXY(float XAxisValue, float YAxisValue)
        {
            double angleInRadians = Math.Atan2(XAxisValue, YAxisValue);
            if (angleInRadians < 0.0f) angleInRadians += (Math.PI * 2.0f);
            double angleInDegrees = (180.0f * angleInRadians / Math.PI);
            return angleInDegrees;
        }

        private static int[] mapDirection = { 8, 9, 6, 3, 2, 1, 4, 7, 0 };

        private static int convertXYtoDirection(Vector2 vector)
        {
            float X = vector.X;
            float Y = vector.Y;
            double sectorSize = 360.0f / 8;
            double halfSectorSize = sectorSize / 2.0f;
            double thumbstickAngle = getAngleFromXY(X, Y);
            double convertedAngle = thumbstickAngle + halfSectorSize;
            int direction = (int)Math.Floor(convertedAngle / sectorSize);
            if (vector.Length() < 1) { return 0; }
            return mapDirection[direction];
        }

        private static int D8TriggerLAxes(int player)
        {
            Vector2 LastLAxes = LastLStickAxes(player);
            Vector2 LAxes = LStickAxes(player);
            int dir_l = convertXYtoDirection(LAxes);
            int last_dir_l = convertXYtoDirection(LastLAxes);
            return (dir_l == last_dir_l) ? 0 : dir_l;
        }

        public static int D4Trigger(int player)
        {
            Keys[] keys = (player == 0) ? Data.settings.p1KeyboardDPad : Data.settings.p2KeyboardDPad;
            if (KeyTrigger(keys[0]) || ButtonTrigger(Buttons.DPadUp, player) || D8TriggerLAxes(player) == 8)
                return 8;
            if (KeyTrigger(keys[1]) || ButtonTrigger(Buttons.DPadLeft, player) || D8TriggerLAxes(player) == 4)
                return 4;
            if (KeyTrigger(keys[3]) || ButtonTrigger(Buttons.DPadRight, player) || D8TriggerLAxes(player) == 6)
                return 6;
            if (KeyTrigger(keys[2]) || ButtonTrigger(Buttons.DPadDown, player) || D8TriggerLAxes(player) == 2)
                return 2;
            return 0;
        }

        #endregion
    }
}
