using System;
using System.Runtime.InteropServices;

using PInvoke;

namespace Logic
{
    class Control
    {
        public static unsafe void Press()
        {
            User32.KEYBDINPUT ki = new User32.KEYBDINPUT();
            ki.wVk = User32.VirtualKey.VK_A;
            ki.wScan = User32.ScanCode.KEY_A;
            // ki.dwFlags = User32.KEYEVENTF;
            // ki.time = uint;
            // ki.dwExtraInfo = void*;
            // ki.dwExtraInfo_IntPtr = IntPtr;

            User32.INPUT[] inputs = new User32.INPUT[1];
            inputs[0].type = User32.InputType.INPUT_KEYBOARD;
            inputs[0].Inputs.ki = ki;
            User32.SendInput(1, inputs, Marshal.SizeOf<User32.INPUT>());
        }

        public static unsafe void Release()
        {
            User32.KEYBDINPUT ki = new User32.KEYBDINPUT();
            ki.wVk = User32.VirtualKey.VK_A;
            ki.wScan = User32.ScanCode.KEY_A;
            ki.dwFlags = User32.KEYEVENTF.KEYEVENTF_KEYUP;
            // ki.time = uint;
            // ki.dwExtraInfo = void*;
            // ki.dwExtraInfo_IntPtr = IntPtr;

            User32.INPUT[] inputs = new User32.INPUT[1];
            inputs[0].type = User32.InputType.INPUT_KEYBOARD;
            inputs[0].Inputs.ki = ki;
            User32.SendInput(1, inputs, Marshal.SizeOf<User32.INPUT>());
        }
    }
}