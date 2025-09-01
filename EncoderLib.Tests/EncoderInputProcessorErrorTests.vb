Imports EncoderLib
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel

'------------------------------------------------------------------------------
'  Created: 2025-09-03
'  Edited:  2025-09-03
'  Author:  ChatGPT
'  Description: Tests error handling in EncoderInputProcessor.
'------------------------------------------------------------------------------
<TestClass>
Public Class EncoderInputProcessorErrorTests

    Private Class ThrowingKeyboard
        Implements IKeyboardSender
        Public CallCount As Integer
        Public Sub SendKeys(keys As IReadOnlyList(Of WindowsKey)) Implements IKeyboardSender.SendKeys
            CallCount += 1
            Throw New Win32Exception(5)
        End Sub
    End Class

    <TestMethod>
    Public Sub RotationFailureDoesNotRepeat()
        Dim kb = New ThrowingKeyboard()
        Dim processor = New EncoderInputProcessor(kb)
        processor.Mapper = New KeyMapper() With {.RotateUp = "A"}
        processor.Process(New EncoderMessage(0, RotationDirection.Clockwise), DateTime.UtcNow)
        Try
            processor.Process(New EncoderMessage(1, RotationDirection.Clockwise), DateTime.UtcNow)
        Catch ex As Win32Exception
        End Try
        Try
            processor.Process(New EncoderMessage(1, RotationDirection.Clockwise), DateTime.UtcNow)
        Catch ex As Win32Exception
        End Try
        Assert.AreEqual(1, kb.CallCount)
    End Sub

    <TestMethod>
    Public Sub ReleaseFailureResetsState()
        Dim kb = New ThrowingKeyboard()
        Dim processor = New EncoderInputProcessor(kb)
        processor.Mapper = New KeyMapper() With {.ButtonPress = "A"}
        Dim now = DateTime.UtcNow
        processor.Process(New ButtonMessage(), now)
        Try
            processor.Process(New EncoderMessage(0, RotationDirection.Clockwise), now.AddMilliseconds(1000))
        Catch ex As Win32Exception
        End Try
        Try
            processor.Process(New EncoderMessage(0, RotationDirection.Clockwise), now.AddMilliseconds(1200))
        Catch ex As Win32Exception
        End Try
        Assert.AreEqual(1, kb.CallCount)
    End Sub
End Class
