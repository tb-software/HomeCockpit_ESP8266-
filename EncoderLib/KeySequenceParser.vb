Imports System
Imports System.Collections.Generic

'------------------------------------------------------------------------------
'  Created: 2025-08-31
'  Edited:  2025-08-31
'  Author:  ChatGPT
'  Description: Parses textual key sequences into lists of key combinations.
'------------------------------------------------------------------------------
Public Class KeySequenceParser
    Public Shared Function ParseSequence(text As String) As IReadOnlyList(Of IReadOnlyList(Of WindowsKey))
        Dim result As New List(Of IReadOnlyList(Of WindowsKey))()
        If String.IsNullOrWhiteSpace(text) Then Return result
        Dim parts = text.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
        If parts.Length = 1 Then
            Dim combo = KeyCombinationParser.Parse(parts(0))
            If combo.Count > 0 Then
                result.Add(combo)
                Return result
            End If
            For Each ch In text.ToCharArray()
                result.Add(KeyCombinationParser.Parse(ch.ToString()))
            Next
        Else
            For Each part In parts
                result.Add(KeyCombinationParser.Parse(part))
            Next
        End If
        Return result
    End Function
End Class
