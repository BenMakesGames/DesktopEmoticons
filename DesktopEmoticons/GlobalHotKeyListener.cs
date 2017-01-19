using System;
using System.Windows.Forms;

namespace DesktopEmoticons
{
    class GlobalHotKeyListener: Form
    {
        public EventHandler HotKey { get; set; }

        private GlobalHotKey _globalHotKey;

        public GlobalHotKeyListener(int modifier, Keys key)
        {
            _globalHotKey = new GlobalHotKey(modifier, key, Handle);
            _globalHotKey.Register();
        }

        protected override void Dispose(bool disposing)
        {
            _globalHotKey.Unregiser();
            base.Dispose(disposing);
        }

        protected override void WndProc(ref Message m)
        {
            if(m.Msg == GlobalHotKey.WM_HOTKEY_MSG_ID)
                HotKey(this, EventArgs.Empty);

            base.WndProc(ref m);
        }
    }
}
