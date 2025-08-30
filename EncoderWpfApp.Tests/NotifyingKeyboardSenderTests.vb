Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports EncoderLib
Imports EncoderWpfApp
Imports System.Collections.Generic

'------------------------------------------------------------------------------
'  Created: 2025-08-30
'  Edited:  2025-08-30
'  Author:  ChatGPT
'  Description: Tests NotifyingKeyboardSender event forwarding.
'------------------------------------------------------------------------------
Namespace EncoderWpfApp.Tests
    <TestClass>
    Public Class NotifyingKeyboardSenderTests
        Private Class DummySender
            Implements IKeyboardSender
            Public Sent As IReadOnlyList(Of WindowsKey)
            Public Sub SendKeys(keys As IReadOnlyList(Of WindowsKey)) Implements IKeyboardSender.SendKeys
                Sent = keys
            End Sub
        End Class

        <TestMethod>
        Public Sub SendKeys_RaisesEventAndDelegates()
            Dim dummy = New DummySender()
            Dim sender = New NotifyingKeyboardSender(dummy)
            Dim received As IReadOnlyList(Of WindowsKey) = Nothing
            AddHandler sender.KeysSent, Sub(k) received = k
            Dim keys As IReadOnlyList(Of WindowsKey) = New List(Of WindowsKey) From {WindowsKey.A}
            sender.SendKeys(keys)
            Assert.IsNotNull(received)
            Assert.AreSame(keys, dummy.Sent)
            Assert.AreSame(keys, received)
        End Sub
    End Class
End Namespace
