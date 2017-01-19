using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DesktopEmoticons
{
    public class GlobalHotKey
    {
        //modifiers
        public const int NOMOD = 0x0000;
        public const int ALT = 0x0001;
        public const int CTRL = 0x0002;
        public const int SHIFT = 0x0004;
        public const int WIN = 0x0008;

        //windows message id for hotkey
        public const int WM_HOTKEY_MSG_ID = 0x0312;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private int _modifier;
        private int _key;
        private IntPtr _hWnd;
        private int _id;

        public GlobalHotKey(int modifier, Keys key, IntPtr handle)
        {
            _modifier = modifier;
            _key = (int)key;
            _hWnd = handle;
            _id = this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _modifier ^ _key ^ _hWnd.ToInt32();
        }

        public bool Register()
        {
            return RegisterHotKey(_hWnd, _id, _modifier, _key);
        }

        public bool Unregiser()
        {
            return UnregisterHotKey(_hWnd, _id);
        }
    }
}
