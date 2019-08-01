using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using System.IO;

namespace ArcLight
{
    public static class Data
    {
        public static void Initialize()
        {
            LoadSettings();
        }

        private static string settingsPath = "Config/Settings.xml";

        public static Settings settings;

        public static void NewSettings()
        {
            settings = new Settings
            {
                isFullScreen = 0,
                windowSize = 0,
                orientation = 0,
                controlMode = new int[] { 0, 0 },
                p1KeyboardDPad = new Keys[] { Keys.Up, Keys.Left, Keys.Down, Keys.Right },
                p2KeyboardDPad = new Keys[] { Keys.W, Keys.A, Keys.S, Keys.D },
                p1KeyboardControls = new Keys[] { Keys.Enter, Keys.Z, Keys.X, Keys.LeftShift, Keys.C, Keys.LeftControl },
                p2KeyboardControls = new Keys[] { Keys.Escape, Keys.OemPeriod, Keys.OemQuestion, Keys.OemComma, Keys.RightShift, Keys.RightControl },
                p1GamepadControls = new Buttons[] { Buttons.Start, Buttons.B, Buttons.A, Buttons.RightTrigger, Buttons.X, Buttons.Y },
                p2GamepadControls = new Buttons[] { Buttons.Start, Buttons.B, Buttons.A, Buttons.RightTrigger, Buttons.X, Buttons.Y },
                bgmVolume = 1.0f,
                seVolume = 1.0f
            };
        }

        public static void LoadSettings()
        {
            if (!File.Exists(settingsPath))
            {
                NewSettings();
                SaveSettings();
            }
            FileStream stream = File.Open(settingsPath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                settings = (Settings)serializer.Deserialize(stream);
            }
            catch (Exception)
            {
                stream.Close();
                NewSettings();
                SaveSettings();
            }
            finally
            {
                stream.Close();
            }
        }

        public static void SaveSettings()
        {
            if (!Directory.Exists(Path.GetDirectoryName(settingsPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(settingsPath));
            }
            if (File.Exists(settingsPath))
            {
                File.Delete(settingsPath);
            }
            FileStream stream = File.Open(settingsPath, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                serializer.Serialize(stream, settings);
            }
            finally
            {
                stream.Close();
            }
        }

        public static Vector2 GetWindowSizeOriginal()
        {
            int index = settings.windowSize;
            switch (index)
            {
                case 0: return new Vector2(480, 640);
                case 1: return new Vector2(600, 800);
                case 2: return new Vector2(720, 960);
                case 3: return new Vector2(768, 1024);
                case 4: return new Vector2(864, 1152);
                case 5: return new Vector2(960, 1280);
            }
            return new Vector2(480, 640);
        }

        public static Vector2 GetWindowSize()
        {
            int orientation = settings.orientation;
            Vector2 size = GetWindowSizeOriginal();
            if (orientation == 1 || orientation == 3)
            {
                return new Vector2(size.Y, size.X);
            }
            return size;
        }

        public static int ControlMode(int player)
        {
            return settings.controlMode[player];
        }

        public static bool FullScreen()
        {
            return Data.settings.isFullScreen != 0;
        }

        public static void Update()
        {
            if (Input.KeyTrigger(Keys.F5))
            {
                settings.isFullScreen = FullScreen() ? 0 : 1;
                Graphics.ToggleFullScreen();
                SaveSettings();
                Graphics.ShowMessage("Full Screen " + (FullScreen() ? "On" : "Off"), 300);
            }
            else if (Input.KeyTrigger(Keys.F6))
            {
                settings.orientation += 1;
                if (settings.orientation > 3)
                    settings.orientation = 0;
                Graphics.FixWindowSize();
                Graphics.ShowMessage("Orientation changed to " + (settings.orientation * 90).ToString() + " degrees", 120);
                SaveSettings();
            }
            else if (Input.KeyTrigger(Keys.F7))
            {
                settings.controlMode[0] += 1;
                if (settings.controlMode[0] > 2)
                    settings.controlMode[0] = 0;
                Graphics.ShowMessage("Player 1's controls changed to Mode " + (settings.controlMode[0] + 1).ToString(), 120);
                SaveSettings();
            }
            else if (Input.KeyTrigger(Keys.F8))
            {
                settings.controlMode[1] += 1;
                if (settings.controlMode[1] > 2)
                    settings.controlMode[1] = 0;
                Graphics.ShowMessage("Player 2's controls changed to Mode " + (settings.controlMode[1] + 1).ToString(), 120);
                SaveSettings();
            }
            else if (Input.KeyTrigger(Keys.F9) && settings.bgmVolume > 0)
            {
                if (Input.KeyDown(Keys.LeftControl) || Input.KeyDown(Keys.RightControl))
                    settings.bgmVolume -= 0.05f;
                else
                    settings.bgmVolume -= 0.01f;
                settings.bgmVolume.Clamp(0, 1.0f);
                Graphics.ShowMessage("BGM Volume decreased. (" + ((int)(settings.bgmVolume * 100)).ToString() + "%)", 120);
                Audio.UpdateBGMVolume();
                SaveSettings();
            }
            else if (Input.KeyTrigger(Keys.F10) && settings.bgmVolume < 1.0f)
            {
                if (Input.KeyDown(Keys.LeftControl) || Input.KeyDown(Keys.RightControl))
                    settings.bgmVolume += 0.05f;
                else
                    settings.bgmVolume += 0.01f;
                settings.bgmVolume.Clamp(0, 1.0f);
                Graphics.ShowMessage("BGM Volume increased. (" + ((int)(settings.bgmVolume * 100)).ToString() + "%)", 120);
                Audio.UpdateBGMVolume();
                SaveSettings();
            }
            else if (Input.KeyTrigger(Keys.F11) && settings.seVolume > 0)
            {
                if (Input.KeyDown(Keys.LeftControl) || Input.KeyDown(Keys.RightControl))
                    settings.seVolume -= 0.05f;
                else
                    settings.seVolume -= 0.01f;
                settings.seVolume.Clamp(0, 1.0f);
                Graphics.ShowMessage("SFX Volume decreased. (" + ((int)(settings.seVolume * 100)).ToString() + "%)", 120);
                SaveSettings();
            }
            else if (Input.KeyTrigger(Keys.F12) && settings.seVolume < 1.0f)
            {
                if (Input.KeyDown(Keys.LeftControl) || Input.KeyDown(Keys.RightControl))
                    settings.seVolume += 0.05f;
                else
                    settings.seVolume += 0.01f;
                settings.seVolume.Clamp(0, 1.0f);
                Graphics.ShowMessage("SFX Volume decreased. (" + ((int)(settings.seVolume * 100)).ToString() + "%)", 120);
                SaveSettings();
            }
        }
    }

    public struct Settings
    {
        public int isFullScreen;
        public int windowSize;
        public int[] controlMode;
        public int orientation;
        public Keys[] p1KeyboardDPad;
        public Keys[] p2KeyboardDPad;
        public Keys[] p1KeyboardControls;
        public Keys[] p2KeyboardControls;
        public Buttons[] p1GamepadControls;
        public Buttons[] p2GamepadControls;
        public float bgmVolume;
        public float seVolume;
    }
}