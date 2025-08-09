Imports System.Runtime.InteropServices
Imports Microsoft.Win32

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Toggles application autostart on Windows.
'------------------------------------------------------------------------------
Public Class AutoStartManager
    Private Const RunKey As String = "Software\Microsoft\Windows\CurrentVersion\Run"
    Private ReadOnly appName As String
    Private ReadOnly settings As AppSettings

    Public Sub New(appName As String, settings As AppSettings)
        Me.appName = appName
        Me.settings = settings
    End Sub

    Public Sub Enable()
        settings.Autostart = True
        settings.Save()
        If RuntimeInformation.IsOSPlatform(OSPlatform.Windows) Then
            Using key = Registry.CurrentUser.OpenSubKey(RunKey, True)
                key.SetValue(appName, $"""{Process.GetCurrentProcess().MainModule.FileName}"" --autostart")
            End Using
        End If
    End Sub

    Public Sub Disable()
        settings.Autostart = False
        settings.Save()
        If RuntimeInformation.IsOSPlatform(OSPlatform.Windows) Then
            Using key = Registry.CurrentUser.OpenSubKey(RunKey, True)
                key.DeleteValue(appName, False)
            End Using
        End If
    End Sub

    Public Function IsEnabled() As Boolean
        Return settings.Autostart
    End Function
End Class

