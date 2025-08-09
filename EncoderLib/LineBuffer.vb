Imports System.Text

'------------------------------------------------------------------------------
'  Created: 2025-08-10
'  Edited:  2025-08-10
'  Author:  ChatGPT
'  Description: Accumulates incoming text fragments and extracts complete lines.
'------------------------------------------------------------------------------
Public Class LineBuffer
    Private ReadOnly builder As New StringBuilder()

    Public Function ExtractLines(fragment As String) As List(Of String)
        Dim lines As New List(Of String)()
        builder.Append(fragment)
        Dim idx As Integer = IndexOfLineEnd()
        While idx >= 0
            lines.Add(builder.ToString(0, idx))
            builder.Remove(0, idx + 1)
            While builder.Length > 0 AndAlso (builder(0) = ChrW(10) OrElse builder(0) = ChrW(13))
                builder.Remove(0, 1)
            End While
            idx = IndexOfLineEnd()
        End While
        Return lines
    End Function

    Private Function IndexOfLineEnd() As Integer
        For i = 0 To builder.Length - 1
            Dim ch = builder(i)
            If ch = ChrW(10) OrElse ch = ChrW(13) Then
                Return i
            End If
        Next
        Return -1
    End Function
End Class
