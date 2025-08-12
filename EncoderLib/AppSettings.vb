Imports System.IO
Imports System.Text.Json
Imports System.Text.Json.Nodes
Imports System.Linq

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-12
'  Author:  ChatGPT
'  Description: Persists application settings locally.
'------------------------------------------------------------------------------
Public Class AppSettings
    Public Property Autostart As Boolean
    Public Property ComPort As String = "Auto"
    Public Property KeyMapping As New KeyMapper()

    Public Shared Function Load(Optional basePath As String = Nothing) As AppSettings
        Dim filePath = GetPath(basePath)
        If File.Exists(filePath) Then
            Dim json = File.ReadAllText(filePath)
            json = NormalizeKeyMapping(json)
            Return JsonSerializer.Deserialize(Of AppSettings)(json)
        End If
        Return New AppSettings()
    End Function

    Public Sub Save(Optional basePath As String = Nothing)
        Dim filePath = GetPath(basePath)
        Directory.CreateDirectory(Path.GetDirectoryName(filePath))
        Dim json = JsonSerializer.Serialize(Me)
        File.WriteAllText(filePath, json)
    End Sub

    Private Shared Function GetPath(basePath As String) As String
        Dim dir = If(basePath, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EncoderWpfApp"))
        Return Path.Combine(dir, "settings.json")
    End Function

    Private Shared Function NormalizeKeyMapping(json As String) As String
        Dim node = JsonNode.Parse(json)
        Dim mapping = node?("KeyMapping")?.AsObject()
        If mapping IsNot Nothing Then
            Dim keys = mapping.Select(Function(k) k.Key).ToList()
            For Each key In keys
                Dim valNode = TryCast(mapping(key), JsonValue)
                If valNode IsNot Nothing Then
                    Dim num As Integer
                    If valNode.TryGetValue(Of Integer)(num) Then
                        mapping(key) = CType(num, WindowsKey).ToString()
                    End If
                End If
            Next
        End If
        Return node.ToJsonString()
    End Function
End Class
