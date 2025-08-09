Imports EncoderLib
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Tests for CommPortController.
'------------------------------------------------------------------------------
<TestClass>
Public Class CommPortControllerTests

    Private Class DummyListener
        Implements IDisposable
        Public Sub Dispose() Implements IDisposable.Dispose
        End Sub
    End Class

    Private Sub IgnoreLine(s As String)
    End Sub

    <TestMethod>
    Public Sub AutoSelectsFirstAvailablePort()
        Dim controller = New CommPortController(AddressOf IgnoreLine, Function() New String() {"COM1", "COM2"}, Function(_a, _b) New DummyListener())
        Assert.IsTrue(controller.Connect("Auto"))
        Assert.AreEqual("COM1", controller.CurrentPort)
    End Sub

    <TestMethod>
    Public Sub ConnectReturnsFalseWhenNoPorts()
        Dim controller = New CommPortController(AddressOf IgnoreLine, Function() Array.Empty(Of String)(), Function(_a, _b) New DummyListener())
        Assert.IsFalse(controller.Connect("Auto"))
    End Sub

    <TestMethod>
    Public Sub ConnectHandlesListenerErrors()
        Dim controller = New CommPortController(AddressOf IgnoreLine, Function() New String() {"COM1"}, _
                                                Function(_a, _b)
                                                    Throw New InvalidOperationException()
                                                End Function)
        Assert.IsFalse(controller.Connect("COM1"))
    End Sub
End Class

