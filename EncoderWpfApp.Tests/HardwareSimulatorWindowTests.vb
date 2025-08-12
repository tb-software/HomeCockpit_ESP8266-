'------------------------------------------------------------------------------
'  Created: 2025-08-12
'  Edited:  2025-08-12
'  Author:  ChatGPT
'  Description: Tests for hardware simulator run-all button.
'------------------------------------------------------------------------------
Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports EncoderLib
Imports EncoderWpfApp

<TestClass>
Public Class HardwareSimulatorWindowTests
    <TestMethod>
    Public Async Function ClickAllButtonsAsync_SendsExpectedSequences() As Task
        Dim recorder As New RecordingKeyboardSender()
        Dim processor As New EncoderInputProcessor(recorder)
        Dim window As New HardwareSimulatorWindow(processor, Function(ts) Task.CompletedTask)

        Await window.ClickAllButtonsAsync()
        Await Task.Delay(300)

        Dim expected = {"Up", "Down", "PageUp", "PageDown", "Escape"}
        Dim actual = recorder.Keys.ConvertAll(Function(k) String.Join("+", k)).ToArray()
        CollectionAssert.AreEqual(expected, actual)
    End Function

    Private Class RecordingKeyboardSender
        Implements IKeyboardSender

        Public ReadOnly Keys As New List(Of IReadOnlyList(Of WindowsKey))()

        Public Sub SendKeys(keys As IReadOnlyList(Of WindowsKey)) Implements IKeyboardSender.SendKeys
            Keys.Add(keys)
        End Sub
    End Class
End Class
