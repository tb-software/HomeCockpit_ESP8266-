Imports System
Imports System.Runtime.InteropServices
Imports System.Collections.Generic

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-14
'  Author:  ChatGPT
'  Description: Sends keyboard input via Win32 SendInput.
'------------------------------------------------------------------------------
Public Class WindowsKeyboardSender
    Implements IKeyboardSender

    <StructLayout(LayoutKind.Sequential)>
    Private Structure INPUT
        Public type As Integer
        Public ki As KEYBDINPUT
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure KEYBDINPUT
        Public wVk As UShort
        Public wScan As UShort
        Public dwFlags As UInteger
        Public time As UInteger
        Public dwExtraInfo As IntPtr
    End Structure

    Private Const INPUT_KEYBOARD As Integer = 1
    Private Const KEYEVENTF_KEYUP As UInteger = &H2UI
    Private Const KEYEVENTF_SCANCODE As UInteger = &H8UI
    Private Const KEYEVENTF_EXTENDEDKEY As UInteger = &H1UI
    Private Const MAPVK_VK_TO_VSC_EX As UInteger = &H4UI

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function SendInput(nInputs As UInteger, inputs() As INPUT, cbSize As Integer) As UInteger
    End Function

    <DllImport("user32.dll")>
    Private Shared Function MapVirtualKeyEx(uCode As UInteger, uMapType As UInteger, dwhkl As IntPtr) As UInteger
    End Function

    <DllImport("user32.dll")>
    Private Shared Function GetKeyboardLayout(idThread As UInteger) As IntPtr
    End Function

    Public Sub SendKeys(keys As IReadOnlyList(Of WindowsKey)) Implements IKeyboardSender.SendKeys
        If keys Is Nothing OrElse keys.Count = 0 Then Return
        Dim list As New List(Of INPUT)()
        For Each key In keys
            list.Add(CreateInput(key, False))
        Next
        For i = keys.Count - 1 To 0 Step -1
            list.Add(CreateInput(keys(i), True))
        Next
        SendInput(CUInt(list.Count), list.ToArray(), Marshal.SizeOf(GetType(INPUT)))
    End Sub

    Private Function CreateInput(key As WindowsKey, keyUp As Boolean) As INPUT
        Dim scan = MapVirtualKeyEx(CUInt(key), MAPVK_VK_TO_VSC_EX, GetKeyboardLayout(0))
        Dim flags As UInteger = KEYEVENTF_SCANCODE
        If keyUp Then flags = flags Or KEYEVENTF_KEYUP
        If (scan And &H100UI) <> 0UI Then flags = flags Or KEYEVENTF_EXTENDEDKEY
        Dim input As New INPUT()
        input.type = INPUT_KEYBOARD
        input.ki = New KEYBDINPUT()
        input.ki.wVk = 0
        input.ki.wScan = CUShort(scan And &HFFUI)
        input.ki.dwFlags = flags
        Return input
    End Function
End Class
