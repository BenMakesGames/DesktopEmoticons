using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace DesktopEmoticons
{
    static class Program
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

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (NotifyIcon icon = new NotifyIcon())
            {
                icon.Icon = new Icon(Properties.Resources.Emoticon, SystemInformation.SmallIconSize);
                icon.Visible = true;

                icon.ContextMenu = BuildContextMenu();

                MethodInfo notifyIconShowContextMenu = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);

                icon.Click += (s, e) =>
                {
                    notifyIconShowContextMenu.Invoke(icon, null);
                };

                Application.Run();
            }
        }

        private static ContextMenu BuildContextMenu()
        {
            List<MenuItem> items = new List<MenuItem>();

            foreach (string category in EMOTICONS.Keys)
            {
                List<MenuItem> subItems = new List<MenuItem>();

                foreach(string e in EMOTICONS[category])
                {
                    subItems.Add(new EmoticonMenuItem(e));
                }

                items.Add(new MenuItem(category, subItems.ToArray()));
            }

            items.Add(new MenuItem("Exit", (s, e) => { Application.Exit(); }));

            return new ContextMenu(items.ToArray());
        }
    }

    public class EmoticonMenuItem: MenuItem
    {
        public EmoticonMenuItem(string text): base(text)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            Clipboard.SetText(Text);
            base.OnClick(e);
        }
    }
}
