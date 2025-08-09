Imports System.Runtime.InteropServices

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
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

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function SendInput(nInputs As UInteger, inputs() As INPUT, cbSize As Integer) As UInteger
    End Function

    Public Sub SendKey(key As WindowsKey) Implements IKeyboardSender.SendKey
        Dim inputs(1) As INPUT
        inputs(0) = CreateInput(key, False)
        inputs(1) = CreateInput(key, True)
        SendInput(2UI, inputs, Marshal.SizeOf(GetType(INPUT)))
    End Sub

    Private Function CreateInput(key As WindowsKey, keyUp As Boolean) As INPUT
        Dim input As New INPUT()
        input.type = 1
        input.ki = New KEYBDINPUT()
        input.ki.wVk = CShort(key)
        input.ki.dwFlags = If(keyUp, &H2UI, 0UI)
        Return input
    End Function
End Class
