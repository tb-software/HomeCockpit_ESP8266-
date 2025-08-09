Imports System.IO
Imports System.Text.Json

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Persists application settings locally.
'------------------------------------------------------------------------------
Public Class AppSettings
    Public Property Autostart As Boolean
    Public Property ComPort As String = "COM4"

    Public Shared Function Load(Optional basePath As String = Nothing) As AppSettings
        Dim filePath = GetPath(basePath)
        If File.Exists(filePath) Then
            Dim json = File.ReadAllText(filePath)
            Return JsonSerializer.Deserialize(Of AppSettings)(json)
        End If
        Return New AppSettings()
    End Function

    Public Sub Save(Optional basePath As String = Nothing)
        Dim filePath = GetPath(basePath)
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath))
        Dim json = JsonSerializer.Serialize(Me)
        File.WriteAllText(filePath, json)
    End Sub

    Private Shared Function GetPath(basePath As String) As String
        Dim dir = If(basePath, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EncoderWpfApp"))
        Return Path.Combine(dir, "settings.json")
    End Function
End Class

