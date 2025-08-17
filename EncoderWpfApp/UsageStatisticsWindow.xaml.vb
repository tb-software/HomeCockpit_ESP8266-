Imports System.Windows
Imports System.Linq
Imports EncoderLib

'------------------------------------------------------------------------------
'  Created: 2025-08-12
'  Edited:  2025-08-12
'  Author:  ChatGPT
'  Description: Displays logged usage statistics.
'------------------------------------------------------------------------------
Partial Public Class UsageStatisticsWindow
    Inherits Window

    Private Class StatItem
        Public Property Name As String
        Public Property Count As Integer
        Public Property Width As Double
    End Class

    Public Sub New()
        InitializeComponent()
        Dim stats = App.Tracker.Snapshot()
        Dim max = If(stats.Count > 0, stats.Values.Max(), 1)
        Dim items = stats.Select(Function(kv) New StatItem With {
                                     .Name = kv.Key,
                                     .Count = kv.Value,
                                     .Width = (kv.Value / max) * 200
                                 }).ToList()
        DataContext = items
    End Sub
End Class
