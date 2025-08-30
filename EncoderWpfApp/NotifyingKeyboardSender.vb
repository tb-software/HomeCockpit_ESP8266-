Imports EncoderLib
Imports System
Imports System.Collections.Generic

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-30
'  Author:  ChatGPT
'  Description: Wraps keyboard sender and exposes sent keys.
'------------------------------------------------------------------------------
Public Class NotifyingKeyboardSender
    Implements IKeyboardSender

    Private ReadOnly wrappedSender As IKeyboardSender
    Public Event KeysSent(keys As IReadOnlyList(Of WindowsKey))

    Public Sub New(wrappedSender As IKeyboardSender)
        If wrappedSender Is Nothing Then
            Throw New ArgumentNullException(NameOf(wrappedSender))
        End If
        Me.wrappedSender = wrappedSender
    End Sub

    Public Sub SendKeys(keys As IReadOnlyList(Of WindowsKey)) Implements IKeyboardSender.SendKeys
        wrappedSender.SendKeys(keys)
        RaiseEvent KeysSent(keys)
    End Sub
End Class

