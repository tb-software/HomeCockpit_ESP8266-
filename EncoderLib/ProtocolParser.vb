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
        Dim trimmed = line.Trim()

        ' New format: "Encoder: 12" / "Button: PRESS"
        If trimmed.StartsWith("Encoder:", StringComparison.OrdinalIgnoreCase) Then
            Dim value = trimmed.Substring("Encoder:".Length).Trim()
            Dim pos As Integer
            If Integer.TryParse(value, pos) Then
                message = New EncoderMessage(pos, RotationDirection.Clockwise)
                Return True
            End If
            Return False
        ElseIf trimmed.StartsWith("Button:", StringComparison.OrdinalIgnoreCase) Then
            If trimmed.Substring("Button:".Length).Trim().Equals("PRESS", StringComparison.OrdinalIgnoreCase) Then
                message = New ButtonMessage()
                Return True
            End If
            Return False
        End If

        ' Legacy format: "ENC;pos;CW" / "BTN;PRESS"
        Dim parts = trimmed.Split(";"c)
        Select Case parts(0)
            Case "ENC"
                If parts.Length = 3 Then
                    Dim pos As Integer
                    If Integer.TryParse(parts(1), pos) Then
                        Dim dir As RotationDirection = If(parts(2).Equals("CW", StringComparison.OrdinalIgnoreCase), RotationDirection.Clockwise, RotationDirection.CounterClockwise)
                        message = New EncoderMessage(pos, dir)
                        Return True
                    End If
                End If
            Case "BTN"
                If parts.Length = 2 AndAlso parts(1).Equals("PRESS", StringComparison.OrdinalIgnoreCase) Then
                    message = New ButtonMessage()
                    Return True
                End If
        End Select
        Return False
    End Function
End Class
