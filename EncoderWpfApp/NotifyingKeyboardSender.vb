Imports EncoderLib
Imports System.Collections.Generic

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-12
'  Author:  ChatGPT
'  Description: Wraps keyboard sender and exposes sent keys.
'------------------------------------------------------------------------------
Public Class NotifyingKeyboardSender
    Implements IKeyboardSender

    Private ReadOnly inner As IKeyboardSender
    Public Event KeysSent(keys As IReadOnlyList(Of WindowsKey))

    Public Sub New(inner As IKeyboardSender)
        Me.inner = inner
    End Sub

    Public Sub SendKeys(keys As IReadOnlyList(Of WindowsKey)) Implements IKeyboardSender.SendKeys
        inner.SendKeys(keys)
        RaiseEvent KeysSent(keys)
    End Sub
End Class

