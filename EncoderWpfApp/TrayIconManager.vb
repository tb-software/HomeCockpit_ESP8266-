Imports System.Windows
Imports System.Windows.Forms

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Manages system tray icon for showing the window.
'------------------------------------------------------------------------------
Public Class TrayIconManager
    Private ReadOnly notify As NotifyIcon
    Private ReadOnly owner As Window

    Public Sub New(owner As Window)
        Me.owner = owner
        notify = New NotifyIcon()
        notify.Icon = System.Drawing.SystemIcons.Application
        notify.Text = "Encoder Input"
        notify.Visible = False
        Dim menu = New ContextMenuStrip()
        menu.Items.Add("Show", Nothing, Sub() ShowWindow())
        menu.Items.Add("Exit", Nothing, Sub() Application.Current.Shutdown())
        notify.ContextMenuStrip = menu
        AddHandler notify.DoubleClick, Sub() ShowWindow()
    End Sub

    Public Sub Show()
        notify.Visible = True
    End Sub

    Private Sub ShowWindow()
        owner.Show()
        owner.WindowState = WindowState.Normal
    End Sub
End Class

