Imports System

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Parses messages from the encoder protocol.
'------------------------------------------------------------------------------
Public Class ProtocolParser
    Public Function TryParse(line As String, ByRef message As HardwareMessage) As Boolean
        If String.IsNullOrWhiteSpace(line) Then Return False
        Dim parts = line.Trim().Split(";"c)
        Select Case parts(0)
            Case "ENC"
                If parts.Length = 3 Then
                    Dim pos As Integer
                    If Integer.TryParse(parts(1), pos) Then
                        Dim dir As RotationDirection = If(parts(2).ToUpperInvariant() = "CW", RotationDirection.Clockwise, RotationDirection.CounterClockwise)
                        message = New EncoderMessage(pos, dir)
                        Return True
                    End If
                End If
            Case "BTN"
                If parts.Length = 2 AndAlso parts(1).ToUpperInvariant() = "PRESS" Then
                    message = New ButtonMessage()
                    Return True
                End If
        End Select
        Return False
    End Function
End Class
