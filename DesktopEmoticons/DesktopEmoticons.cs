using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DesktopEmoticons
{
    public class DesktopEmoticons: ApplicationContext
    {
        public static readonly Dictionary<string, string[]> EMOTICONS = new Dictionary<string, string[]>()
        {
            { "Animals", new string[] {
                @"ʕ •ᴥ•ʔ",
                @"▼・ᴥ・▼",
            } },

            { "CONCERN", new string[] {
                @"(；ꏿ︿ꏿ)",
                @"ヾ(｡ꏿ﹏ꏿ)ﾉﾞ",
                @"(′°︿°)",
                @"ԅ[ •́ ﹏ •̀ ]و",
            } },

            { "Disbelief", new string[] {
                @"( ͠° ͟ʖ ͡°)",
            } },

            { "FAILURE", new string[] {
                @"ಥ╭╮ಥ",
                @"(-﹏-。)",
                @"︵‿︵‿︵‿︵‿︵‿︵‿︵‿︵‿︵‿ヽ(゜□゜ )ノ︵‿︵‿︵‿︵‿︵‿",
            } },

            { "Flirtatious", new string[] {
                @"(◡‿◡✿)",
                @"( ˘ ³˘)♥",
                @"\( ͡° ͜/// ͡°)/",
                @"ԅ|.͡° ڡ ͡°.|ᕤ",
            } },

            { "Food time!", new string[] {
                @"( ˘▽˘)っ♨",
                @"(っ˘ڡ˘)っ─∈",
            } },

            { "Happy ^^", new string[] {
                @"≧◠◡◠≦",
                @"=^_^=",
                @"(*ﾟｰﾟ)ゞ",
                @"*:ﾟ*｡⋆ฺ(*´◡`)",
                @"(✿ꈍ。 ꈍ✿)",
            } },

            { "I dunno!", new string[] {
                @"¯\_(°⊱,°)_/¯",
                @"◴_◶",
                @"(⊙_☉)",
                @"ɿ(｡･ɜ･)ɾⓌⓗⓐⓣ？",
            } },

            { "Pissed!", new string[] {
                @"(╯°□°）╯︵ ┻━┻",
                @"ಠ_ಠ",
                @"┏|￣＾￣* |┛",
            } },

            { "Success!", new string[] {
                @"(ง ͡° ͜ʖ ͡°)ง",
                @"ヾ（＾ヮ＾)ﾉ",
                @"[̲̅$̲̅(̲̅ ͡° ͜ʖ ͡°̲̅)̲̅$̲̅]",
                @"＼（＠￣∇￣＠）／",
            } },

            { "Misc.", new string[] {
                @"┗|・o・|┛",
                @"（￣へ￣）",
                @"( ・_・)ノ",
                @"(ᵕ≀ ̠ᵕ )",
                @"♬♫♪◖(●。●)◗♪♫♬",
            } }
        };

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        private NotifyIcon _notifyIcon;
        private GlobalHotKeyListener _hotKeyListener;
        
        private static IntPtr _previousWindow = IntPtr.Zero;

        public DesktopEmoticons()
        {
            _notifyIcon = new NotifyIcon()
            {
                Icon = new Icon(Properties.Resources.Emoticon, SystemInformation.SmallIconSize),
                Visible = true,

                ContextMenu = BuildContextMenu(),
            };

            MethodInfo notifyIconShowContextMenu = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);

            _notifyIcon.Click += (s, e) =>
            {
                notifyIconShowContextMenu.Invoke(_notifyIcon, null);
            };

            _notifyIcon.ContextMenu.Collapse += (s, e) =>
            {
                _previousWindow = IntPtr.Zero;
            };

            _hotKeyListener = new GlobalHotKeyListener(GlobalHotKey.WIN | GlobalHotKey.SHIFT, Keys.OemSemicolon);

            _hotKeyListener.HotKey += (s, e) =>
            {
                _previousWindow = GetForegroundWindow();
                notifyIconShowContextMenu.Invoke(_notifyIcon, null);
            };
        }

        private ContextMenu BuildContextMenu()
        {
            List<MenuItem> items = new List<MenuItem>();

            foreach (string category in EMOTICONS.Keys)
            {
                List<MenuItem> subItems = new List<MenuItem>();

                foreach (string emoticon in EMOTICONS[category])
                {
                    MenuItem item = new MenuItem(emoticon);

                    item.Click += (s, e) =>
                    {
                        Clipboard.SetText(emoticon);

                        if (_previousWindow != IntPtr.Zero)
                        {
                            //SetForegroundWindow(_previousWindow);
                        }
                    };

                    subItems.Add(item);
                }

                items.Add(new MenuItem(category, subItems.ToArray()));
            }

            items.Add(new MenuItem("Exit", (s, e) => {
                _notifyIcon.Visible = false;
                Application.Exit();
            }));

            return new ContextMenu(items.ToArray());
        }
    }
}
