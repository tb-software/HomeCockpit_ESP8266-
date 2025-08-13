Imports System.Collections.Concurrent

'------------------------------------------------------------------------------
'  Created: 2025-08-12
'  Edited:  2025-08-12
'  Author:  ChatGPT
'  Description: Records counts of named usage events.
'------------------------------------------------------------------------------
Public Class UsageTracker
    Private ReadOnly counts As ConcurrentDictionary(Of String, Integer)

    Public Sub New()
        counts = New ConcurrentDictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
    End Sub

    Public Sub Record(eventName As String)
        counts.AddOrUpdate(eventName, 1, Function(k, c) c + 1)
    End Sub

    Public Function Snapshot() As IReadOnlyDictionary(Of String, Integer)
        Return New Dictionary(Of String, Integer)(counts)
    End Function
End Class
