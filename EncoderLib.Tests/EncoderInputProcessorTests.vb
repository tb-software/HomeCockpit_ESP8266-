Imports EncoderLib
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports System.Collections.Generic
Imports System.Linq

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Tests for EncoderInputProcessor.
'------------------------------------------------------------------------------
<TestClass>
Public Class EncoderInputProcessorTests

    Private Class KeyboardMock
        Implements IKeyboardSender
        Public ReadOnly Sent As New List(Of WindowsKey)
        Public Sub SendKey(key As WindowsKey) Implements IKeyboardSender.SendKey
            Sent.Add(key)
        End Sub
    End Class

    <TestMethod>
    Public Sub RotationWithoutButtonSendsUp()
        Dim keyboard = New KeyboardMock()
        Dim processor = New EncoderInputProcessor(keyboard, New KeyMapper())
        processor.Process(New EncoderMessage(0, RotationDirection.Clockwise), DateTime.UtcNow)
        processor.Process(New EncoderMessage(1, RotationDirection.Clockwise), DateTime.UtcNow)
        Assert.AreEqual(1, keyboard.Sent.Count)
        Assert.AreEqual(WindowsKey.Up, keyboard.Sent(0))
    End Sub

    <TestMethod>
    Public Sub RotationWithButtonSendsPageUp()
        Dim keyboard = New KeyboardMock()
        Dim processor = New EncoderInputProcessor(keyboard, New KeyMapper())
        Dim now = DateTime.UtcNow
        processor.Process(New ButtonMessage(), now)
        processor.Process(New EncoderMessage(0, RotationDirection.Clockwise), now.AddMilliseconds(10))
        processor.Process(New EncoderMessage(1, RotationDirection.Clockwise), now.AddMilliseconds(20))
        Assert.AreEqual(WindowsKey.PageUp, keyboard.Sent.Last())
    End Sub

    <TestMethod>
    Public Sub ShortPressSendsEnter()
        Dim keyboard = New KeyboardMock()
        Dim processor = New EncoderInputProcessor(keyboard, New KeyMapper())
        Dim now = DateTime.UtcNow
        processor.Process(New ButtonMessage(), now)
        processor.Process(Nothing, now.AddMilliseconds(500))
        Assert.AreEqual(WindowsKey.Enter, keyboard.Sent.Single())
    End Sub

    <TestMethod>
    Public Sub LongPressSendsEscape()
        Dim keyboard = New KeyboardMock()
        Dim processor = New EncoderInputProcessor(keyboard, New KeyMapper())
        Dim now = DateTime.UtcNow
        processor.Process(New ButtonMessage(), now)
        processor.Process(New ButtonMessage(), now.AddMilliseconds(300))
        processor.Process(Nothing, now.AddMilliseconds(1200))
        Assert.AreEqual(WindowsKey.Escape, keyboard.Sent.Single())
    End Sub
End Class
