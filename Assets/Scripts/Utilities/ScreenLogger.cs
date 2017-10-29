using UnityEngine;
using System.Collections.Generic;

namespace AClockworkBerry
{
    public class ScreenLogger : MonoBehaviour
    {
        private const string Space = " ";
        private const string ScrollToTop = "▬";
        private const string ScrollToBottom = "▬";
        private const string ScrollUp = "▲";
        private const string ScrollDown = "▼";
        private const string ClearView = "CLEAR";

        public static bool IsPersistent = true;

        private static ScreenLogger instance;
        private static bool instantiated = false;

        private Vector2 scrollPosition = Vector2.zero;
        private bool manuallyScroll = false;

        private GUILayoutOption buttonWidth;
        private GUILayoutOption buttonHeight;

        class LogMessage
        {
            public string Message;
            public LogType Type;

            public LogMessage(string msg, LogType type)
            {
                Message = msg;
                Type = type;
            }
        }

        public enum LogAnchor
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        public bool ShowLog = true;
        public bool ShowInEditor = true;
        public bool AdvancedButtons = false;

        [Tooltip("Height of the log area as a percentage of the screen height")]
        [Range(0.3f, 1.0f)]
        public float Height = 0.5f;

        [Tooltip("Width of the log area as a percentage of the screen width")]
        [Range(0.3f, 1.0f)]
        public float Width = 0.5f;

        public int Margin = 20;

        public LogAnchor AnchorPosition = LogAnchor.BottomLeft;

        public int FontSize = 14;

        public float ScrollSpeed = 10f;

        [Range(0f, 01f)]
        public float BackgroundOpacity = 0.5f;
        public Color BackgroundColor = Color.black;

        public bool LogMessages = true;
        public bool LogWarnings = true;
        public bool LogErrors = true;

        public Color MessageColor = Color.white;
        public Color WarningColor = Color.yellow;
        public Color ErrorColor = new Color(1, 0.5f, 0.5f);

        public bool StackTraceMessages = false;
        public bool StackTraceWarnings = false;
        public bool StackTraceErrors = true;

        static Queue<LogMessage> queue = new Queue<LogMessage>();

        GUIStyle styleContainer, styleText;
        int padding = 5;

        private bool destroying = false;

        public static ScreenLogger Instance
        {
            get
            {
                if (instantiated) return instance;

                instance = FindObjectOfType<ScreenLogger>();

                // Object not found, we create a new one
                if (instance == null)
                {
                    // Try to load the default prefab
                    try
                    {
                        instance = Instantiate(Resources.Load<ScreenLogger>("ScreenLoggerPrefab"));
                    }
                    catch
                    {
                        Debug.Log("Failed to load default Screen Logger prefab...");
                        instance = new GameObject("ScreenLogger", typeof(ScreenLogger)).GetComponent<ScreenLogger>();
                    }

                    // Problem during the creation, this should not happen
                    if (instance == null)
                    {
                        Debug.LogError("Problem during the creation of ScreenLogger");
                    }
                    else instantiated = true;
                }
                else
                {
                    instantiated = true;
                }

                return instance;
            }
        }

        public void Awake()
        {
            ScreenLogger[] obj = FindObjectsOfType<ScreenLogger>();

            if (obj.Length > 1)
            {
                Debug.Log("Destroying ScreenLogger, already exists...");

                destroying = true;

                Destroy(gameObject);
                return;
            }

            InitStyles();

            if (IsPersistent)
                DontDestroyOnLoad(this);
        }

        private void InitStyles()
        {
            Texture2D back = new Texture2D(1, 1);
            BackgroundColor.a = BackgroundOpacity;
            back.SetPixel(0, 0, BackgroundColor);
            back.Apply();

            styleContainer = new GUIStyle();
            styleContainer.normal.background = back;
            styleContainer.wordWrap = false;
            styleContainer.padding = new RectOffset(padding, padding, padding, padding);

            styleText = new GUIStyle();
            styleText.fontSize = FontSize;
            styleText.wordWrap = true;
        }

        void OnEnable()
        {
            if (!ShowInEditor && Application.isEditor) return;

            queue = new Queue<LogMessage>();

#if UNITY_4_5 || UNITY_4_6 || UNITY_4_7
            Application.RegisterLogCallback(HandleLog);
#else
            Application.logMessageReceived += HandleLog;
#endif
        }

        void OnDisable()
        {
            // If destroyed because already exists, don't need to de-register callback
            if (destroying) return;

#if UNITY_4_5 || UNITY_4_6 || UNITY_4_7
            Application.RegisterLogCallback(null);
#else
            Application.logMessageReceived -= HandleLog;
#endif
        }

        void Update()
        {
            if (!ShowInEditor && Application.isEditor) return;

            float InnerHeight = (Screen.height - 2 * Margin) * Height - 2 * padding;
            int TotalRows = (int) (InnerHeight / styleText.lineHeight);

            // Remove overflowing rows
            while (queue.Count > TotalRows)
                queue.Dequeue();
        }

        void OnGUI()
        {
            if (!ShowLog) return;
            if (!ShowInEditor && Application.isEditor) return;

            if (AdvancedButtons && (buttonWidth == null || buttonHeight == null))
            {
                buttonWidth = GUILayout.Width(25f);
                buttonHeight = GUILayout.Height(25f);
            }

            float w = (Screen.width - 2 * Margin) * Width;
            float h = (Screen.height - 2 * Margin) * Height;
            float x = 1, y = 1;

            switch (AnchorPosition)
            {
                case LogAnchor.BottomLeft:
                    x = Margin;
                    y = Margin + (Screen.height - 2 * Margin) * (1 - Height);
                    break;

                case LogAnchor.BottomRight:
                    x = Margin + (Screen.width - 2 * Margin) * (1 - Width);
                    y = Margin + (Screen.height - 2 * Margin) * (1 - Height);
                    break;

                case LogAnchor.TopLeft:
                    x = Margin;
                    y = Margin;
                    break;

                case LogAnchor.TopRight:
                    x = Margin + (Screen.width - 2 * Margin) * (1 - Width);
                    y = Margin;
                    break;
            }

            GUILayout.BeginArea(new Rect((int) x, (int) y, w, h), styleContainer);

            if (AdvancedButtons)
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(buttonWidth);

                if (GUILayout.Button(ScrollToTop, buttonHeight))
                {
                    manuallyScroll = true;
                    scrollPosition.y = 0;
                }

                GUILayout.Space(10f);

                if (GUILayout.RepeatButton(ScrollUp, buttonHeight))
                {
                    manuallyScroll = true;
                    scrollPosition.y -= ScrollSpeed;
                }

                GUILayout.FlexibleSpace();

                if (GUILayout.RepeatButton(ScrollDown, buttonHeight))
                {
                    manuallyScroll = true;
                    scrollPosition.y += ScrollSpeed;
                }

                GUILayout.Space(10f);

                if (GUILayout.Button(ScrollToBottom, buttonHeight))
                {
                    manuallyScroll = false;
                }

                if (!manuallyScroll)
                {
                    scrollPosition.y = int.MaxValue;
                }

                GUILayout.EndVertical();
                GUILayout.BeginVertical();
            }

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);

            var started = false;

            foreach (LogMessage m in queue)
            {
                switch (m.Type)
                {
                    case LogType.Warning:
                        styleText.normal.textColor = WarningColor;
                        break;

                    case LogType.Log:
                        styleText.normal.textColor = MessageColor;
                        break;

                    case LogType.Assert:
                    case LogType.Exception:
                    case LogType.Error:
                        styleText.normal.textColor = ErrorColor;
                        break;

                    default:
                        styleText.normal.textColor = MessageColor;
                        break;
                }

                if (!m.Message.StartsWith(Space) && started)
                    GUILayout.Space(FontSize);

                GUILayout.Label(m.Message, styleText);
                started = true;
            }

            GUILayout.EndScrollView();

            if (AdvancedButtons)
            {
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                if (GUILayout.Button(ClearView, buttonHeight))
                {
                    queue.Clear();
                }
            }

            GUILayout.EndArea();
        }

        void HandleLog(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Assert && !LogErrors) return;
            if (type == LogType.Error && !LogErrors) return;
            if (type == LogType.Exception && !LogErrors) return;
            if (type == LogType.Log && !LogMessages) return;
            if (type == LogType.Warning && !LogWarnings) return;

            string[] lines = message.Split(new char[] { '\n' });

            foreach (string l in lines)
                queue.Enqueue(new LogMessage(l, type));

            if (type == LogType.Assert && !StackTraceErrors) return;
            if (type == LogType.Error && !StackTraceErrors) return;
            if (type == LogType.Exception && !StackTraceErrors) return;
            if (type == LogType.Log && !StackTraceMessages) return;
            if (type == LogType.Warning && !StackTraceWarnings) return;

            string[] trace = stackTrace.Split(new char[] { '\n' });

            foreach (string t in trace)
                if (t.Length != 0) queue.Enqueue(new LogMessage("    " + t, type));
        }

        public void InspectorGUIUpdated()
        {
            InitStyles();
        }
    }
}

/*
The MIT License

Copyright © 2016 Screen Logger - Giuseppe Portelli <giuseppe@aclockworkberry.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
