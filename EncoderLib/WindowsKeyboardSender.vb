Imports System.Runtime.InteropServices
Imports System.Collections.Generic

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-12
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
        Dim input As New INPUT()
        input.type = 1
        input.ki = New KEYBDINPUT()
        input.ki.wVk = CShort(key)
        input.ki.dwFlags = If(keyUp, &H2UI, 0UI)
        Return input
    End Function
End Class
