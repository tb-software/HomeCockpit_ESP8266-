Imports System
Imports System.Collections.Generic

'------------------------------------------------------------------------------
'  Created: 2025-08-12
'  Edited:  2025-08-12
'  Author:  ChatGPT
'  Description: Parses textual key combinations into WindowsKey lists.
'------------------------------------------------------------------------------
Public Class KeyCombinationParser
    Public Shared Function Parse(text As String) As IReadOnlyList(Of WindowsKey)
        Dim keys As New List(Of WindowsKey)()
        If String.IsNullOrWhiteSpace(text) Then Return keys
        For Each part In text.Split("+"c)
            Dim token = part.Trim()
            Dim key As WindowsKey
            If [Enum].TryParse(token, True, key) Then
                keys.Add(key)
            Else
                Select Case token.ToLowerInvariant()
                    Case "ctrl", "control"
                        keys.Add(WindowsKey.Control)
                    Case "shift"
                        keys.Add(WindowsKey.Shift)
                    Case "alt"
                        keys.Add(WindowsKey.Alt)
                    Case "win", "windows"
                        keys.Add(WindowsKey.LWin)
                    Case "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
                        Dim numKey As WindowsKey = CType([Enum].Parse(GetType(WindowsKey), "D" & token), WindowsKey)
                        keys.Add(numKey)
                End Select
            End If
        Next
        Return keys
    End Function
End Class
