Imports EncoderLib

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Wraps keyboard sender and exposes sent keys.
'------------------------------------------------------------------------------
Public Class NotifyingKeyboardSender
    Implements IKeyboardSender

    Private ReadOnly inner As IKeyboardSender
    Public Event KeySent(key As WindowsKey)

    Public Sub New(inner As IKeyboardSender)
        Me.inner = inner
    End Sub

    Public Sub SendKey(key As WindowsKey) Implements IKeyboardSender.SendKey
        inner.SendKey(key)
        RaiseEvent KeySent(key)
    End Sub
End Class

