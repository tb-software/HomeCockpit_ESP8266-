Imports System
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Collections.Generic

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-30
'  Author:  ChatGPT
'  Description: Sends keyboard input via Win32 SendInput.
'------------------------------------------------------------------------------
Public Class WindowsKeyboardSender
    Implements IKeyboardSender

    <StructLayout(LayoutKind.Sequential)>
    Private Structure INPUT
        Public type As Integer
        Public U As InputUnion
    End Structure

    <StructLayout(LayoutKind.Explicit)>
    Private Structure InputUnion
        <FieldOffset(0)> Public ki As KEYBDINPUT
        <FieldOffset(0)> Public mi As MOUSEINPUT
        <FieldOffset(0)> Public hi As HARDWAREINPUT
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure KEYBDINPUT
        Public wVk As UShort
        Public wScan As UShort
        Public dwFlags As UInteger
        Public time As UInteger
        Public dwExtraInfo As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure MOUSEINPUT
        Public dx As Integer
        Public dy As Integer
        Public mouseData As UInteger
        Public dwFlags As UInteger
        Public time As UInteger
        Public dwExtraInfo As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure HARDWAREINPUT
        Public uMsg As UInteger
        Public wParamL As UShort
        Public wParamH As UShort
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
        Dim inputs As New List(Of INPUT)()
        For Each virtualKey In keys
            inputs.Add(CreateKeyInput(virtualKey, False))
        Next
        For i = keys.Count - 1 To 0 Step -1
            inputs.Add(CreateKeyInput(keys(i), True))
        Next
        Dim sent = SendInput(CUInt(inputs.Count), inputs.ToArray(), Marshal.SizeOf(GetType(INPUT)))
        If sent <> inputs.Count Then
            Throw New Win32Exception(Marshal.GetLastWin32Error())
        End If
    End Sub

    Private Function CreateKeyInput(virtualKey As WindowsKey, keyUp As Boolean) As INPUT
        Dim scan = MapVirtualKeyEx(CUInt(virtualKey), MAPVK_VK_TO_VSC_EX, GetKeyboardLayout(0))

        Dim flags As UInteger = 0UI
        Dim wVk As UShort = CUShort(virtualKey)
        Dim wScan As UShort = 0US

        If scan <> 0UI Then
            flags = KEYEVENTF_SCANCODE
            wVk = 0US
            wScan = CUShort(scan And &HFFUI)
            If (scan And &H100UI) <> 0UI Then flags = flags Or KEYEVENTF_EXTENDEDKEY
        End If

        If keyUp Then flags = flags Or KEYEVENTF_KEYUP

        Dim input As New INPUT()
        input.type = INPUT_KEYBOARD
        input.U.ki = New KEYBDINPUT()
        input.U.ki.wVk = wVk
        input.U.ki.wScan = wScan
        input.U.ki.dwFlags = flags
        Return input
    End Function
End Class
