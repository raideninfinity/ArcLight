using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ArcLight
{
    public static class Cache
    {
        private static SegmentContentManager content;

        public static void Initialize(IServiceProvider serviceProvider, string rootDirectory, int segments)
        {
            content = new SegmentContentManager(serviceProvider, rootDirectory, segments);
        }

        //Basic Functions
        #region Basic

        public static T Load<T>(string name, int segment = 1)
        {
            return content.Load<T>(name, segment);
        }

        public static void Unload(string name = "")
        {
            content.Unload(name);
        }

        public static void Unload(object value)
        {
            content.Unload(value);
        }

        public static void UnloadSegment(int segment = 0)
        {
            content.UnloadSegment(segment);
        }

        public static void UnloadAll()
        {
            content.UnloadAll();
        }

        public static void UnloadAllSegment()
        {
            content.UnloadAllSegment();
        }

        public static void UnloadEverything()
        {
            content.UnloadEverything();
        }

        public static void UnloadPerma()
        {
            content.UnloadPerma();
        }

        public static void SwapSegment(int s1, int s2)
        {
            content.SwapSegment(s1, s2);
        }

        public static void DumpSegment(int src, int dest)
        {
            content.DumpSegment(src, dest);
        }

        public static void SwitchSegment(object o, int dest)
        {
            content.SwitchSegment(o, dest);
        }

        #endregion

        //Cache Functions
        public static Texture2D Texture(string name, int segment = 1)
        {
            return Load<Texture2D>(name, segment);
        }

        public static SpriteFont Font(string name, int segment = 1)
        {
            return Load<SpriteFont>(name, segment);
        }

        public static Song BGM(string name, int segment = 1)
        {
            return Load<Song>(name, segment);
        }

        public static SoundEffect SE(string name, int segment = 1)
        {
            return Load<SoundEffect>(name, segment);
        }

    }

    //----------------------------------------------------------------------------------------------------------------

    public partial class SegmentContentManager : ContentManager
    {
        #region Constructors

        public SegmentContentManager(IServiceProvider serviceProvider)
            : base(serviceProvider) { CreateSegments(); }
        public SegmentContentManager(IServiceProvider serviceProvider, string rootDirectory, int num)
            : base(serviceProvider, rootDirectory) { segments = num; CreateSegments(); }

        private void CreateSegments()
        {
            segmentAssets = new List<Dictionary<string, object>>();
            segmentKeys = new List<Dictionary<object, string>>();
            for (int i = 0; i <= segments; i++)
            {
                segmentAssets.Add(new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase));
                segmentKeys.Add(new Dictionary<object, string>());
            }
        }

        #endregion

        #region Variables

        private int segments = 1;
        private List<Dictionary<string, object>> segmentAssets;
        private List<Dictionary<object, string>> segmentKeys;

        #endregion

        #region Loading
        //Load to Segment
        public override T Load<T>(string assetName)
        {
            return LoadEX<T>(assetName, 1);
        }

        public T Load<T>(string assetName, int segment = 1)
        {
            return LoadEX<T>(assetName, segment);
        }

        public T LoadEX<T>(string assetName, int segment)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new ArgumentNullException("assetName");
            }
            T result = default(T);
            var key = assetName.Replace('\\', '/');
            //Cache
            object asset = null;
            for (int i = 0; i < segments; i++)
            {
                if (segmentAssets[i].TryGetValue(key, out asset))
                {
                    if (asset is T)
                    {
                        return (T)asset;
                    }
                }
            }
            //Load the asset
            result = ReadAsset<T>(assetName, null);
            segmentAssets[segment][key] = result;
            segmentKeys[segment][result] = key;
            //Final
            return result;
        }

        #endregion

        #region Unloading

        public override void Unload()
        {
            Unload("");
        }

        public void Unload(string assetName = "")
        {
            if (string.IsNullOrEmpty(assetName))
            {
                UnloadSegment(1);
                return;
            }
            var key = assetName.Replace('\\', '/');
            for (int i = 0; i <= segments; i++)
            {
                if (segmentAssets[i].ContainsKey(key))
                {
                    var o = segmentAssets[i][key];
                    if (o is IDisposable a) { a.Dispose(); }
                    segmentAssets[i].Remove(key);
                    segmentKeys[i].Remove(o);
                    return;
                }
            }
        }

        public void Unload(object value)
        {
            var o = (object)value;
            for (int i = 0; i <= segments; i++)
            {
                if (segmentKeys[i].ContainsKey(o))
                {
                    string key = segmentKeys[i][o];
                    if (o is IDisposable a) { a.Dispose(); }
                    segmentAssets[i].Remove(key);
                    segmentKeys[i].Remove(o);
                    return;
                }
            }
        }

        public void UnloadAll()
        {
            UnloadSegment(0);
        }

        public void UnloadEverything()
        {
            UnloadSegment(0);
            UnloadPerma();
        }

        public void UnloadAllSegment()
        {
            UnloadSegment(0);
        }

        public void UnloadSegment(int segment = 1)
        {
            if (segment == 0)
            {
                for (int i = 1; i <= segments; i++)
                {
                    foreach (object o in segmentAssets[i])
                    {
                        if (o is IDisposable a) { a.Dispose(); }
                    }
                    segmentAssets[i].Clear();
                    segmentKeys[i].Clear();
                }
            }
            else
            {
                foreach (object o in segmentAssets[segment])
                {
                    if (o is IDisposable a) { a.Dispose(); }
                }
                segmentAssets[segment].Clear();
                segmentKeys[segment].Clear();
            }
        }

        public void UnloadPerma()
        {
            foreach (object o in segmentAssets[0])
            {
                if (o is IDisposable a) { a.Dispose(); }
            }
            segmentAssets[0].Clear();
            segmentKeys[0].Clear();
        }

        #endregion

        #region Checking

        public bool CheckCached(string s)
        {
            return CheckSegment(s) == 6;
        }

        public bool CheckCached(object o)
        {
            return CheckSegment(o) == 6;
        }

        public int CheckSegment(string s)
        {
            for (int i = 0; i <= segments; i++)
            {
                if (segmentAssets[i].ContainsKey(s))
                {
                    return i;
                }
            }
            return -1;
        }

        public int CheckSegment(object o)
        {
            for (int i = 0; i <= segments; i++)
            {
                if (segmentKeys[i].ContainsKey(o))
                {
                    return i;
                }
            }
            return -1;
        }

        #endregion

        #region Segment Operations

        public void SwapSegment(int s1, int s2)
        {
            var sv = segmentAssets[s2];
            var sk = segmentKeys[s2];
            segmentAssets[s2] = segmentAssets[s1];
            segmentKeys[s2] = segmentKeys[s1];
            segmentAssets[s1] = sv;
            segmentKeys[s1] = sk;
        }

        public void DumpSegment(int src, int dest)
        {
            var srcv = segmentAssets[src];
            var srck = segmentKeys[src];
            var destv = segmentAssets[dest];
            var destk = segmentKeys[dest];
            foreach (KeyValuePair<string, object> t in srcv)
            {
                destv[t.Key] = t.Value;
            }
            srcv.Clear();
            foreach (KeyValuePair<object, string> t in srck)
            {
                destk[t.Key] = t.Value;
            }
            srck.Clear();
        }

        public void SwitchSegment(object o, int dest)
        {
            string key = null;
            int src = -1;
            for (int i = 0; i <= segments; i++)
            {
                if (segmentKeys[i].ContainsKey(o))
                {
                    key = segmentKeys[i][o];
                    src = i;
                }
            }
            if (src == -1)
            {
                throw new ArgumentException("Asset to swap not found!");
            }
            if (src == dest) { return; }
            segmentAssets[dest][key] = o;
            segmentKeys[dest][o] = key;
            segmentAssets[src].Remove(key);
            segmentKeys[src].Remove(o);
        }

        #endregion

    }
}