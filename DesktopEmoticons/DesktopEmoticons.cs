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
        
        private IntPtr _previousWindow = IntPtr.Zero;
        private IntPtr _focusWindow = IntPtr.Zero;

        public DesktopEmoticons()
        {
            _notifyIcon = new NotifyIcon()
            {
                Icon = new Icon(Properties.Resources.Emoticon, SystemInformation.SmallIconSize),
                Visible = true,

                ContextMenuStrip = BuildContextMenu(),
            };

            MethodInfo notifyIconShowContextMenu = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);

            _notifyIcon.MouseUp += (s, e) =>
            {
                _previousWindow = IntPtr.Zero;

                if (e.Button == MouseButtons.Left)
                {
                    if (_notifyIcon.ContextMenuStrip.Visible)
                        _notifyIcon.ContextMenuStrip.Hide();
                    else
                    {
                        MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                        mi.Invoke(_notifyIcon, null);
                    }
                }
            };

            _hotKeyListener = new GlobalHotKeyListener(GlobalHotKey.WIN | GlobalHotKey.SHIFT, Keys.OemSemicolon);

            _hotKeyListener.HotKey += (s, e) =>
            {
                _previousWindow = GetForegroundWindow();

                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(_notifyIcon, null);
            };
        }

        private ContextMenuStrip BuildContextMenu()
        {
            ContextMenuStrip menuStrip = new ContextMenuStrip();

            foreach (string category in EMOTICONS.Keys)
            {
                List<ToolStripMenuItem> subItems = new List<ToolStripMenuItem>();

                foreach (string emoticon in EMOTICONS[category])
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(emoticon, null, (s, e) =>
                    {
                        Clipboard.SetText(emoticon);

                        if (_previousWindow != IntPtr.Zero)
                        {
                            SetForegroundWindow(_previousWindow);
                        }
                    });

                    subItems.Add(item);
                }

                menuStrip.Items.Add(new ToolStripMenuItem(category, null, subItems.ToArray()));
            }

            menuStrip.Items.Add(new ToolStripMenuItem("Exit", null, (s, e) => {
                _notifyIcon.Visible = false;
                Application.Exit();
            }));

            return menuStrip;
        }
    }
}
