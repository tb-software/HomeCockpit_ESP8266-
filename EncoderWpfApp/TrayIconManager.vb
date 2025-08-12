Imports System.Windows
Imports System.Windows.Forms
Imports System.Windows.Threading
Imports System.Drawing

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-13
'  Author:  ChatGPT
'  Description: Manages system tray icon and key press indication.
'------------------------------------------------------------------------------
Public Class TrayIconManager
    Implements IDisposable

    Private Const DefaultIconResource As String = "pack://application:,,,/HomeCockpit_Icon_DEFAULT.ico"
    Private Const ActiveIconResource As String = "pack://application:,,,/HomeCockpit_Icon_ACTIVE.ico"

    Private ReadOnly notify As NotifyIcon
    Private ReadOnly owner As Window
    Private ReadOnly defaultIconImage As Icon
    Private ReadOnly activeIconImage As Icon
    Private ReadOnly revertTimer As DispatcherTimer

    Public Sub New(owner As Window)
        Me.owner = owner
        notify = New NotifyIcon()
        defaultIconImage = LoadIcon(DefaultIconResource)
        activeIconImage = LoadIcon(ActiveIconResource)
        notify.Icon = defaultIconImage
        notify.Text = "Encoder Input"
        notify.Visible = False
        Dim menu = New ContextMenuStrip()
        menu.Items.Add("Show", Nothing, Sub() ShowWindow())
        menu.Items.Add("Exit", Nothing, Sub() System.Windows.Application.Current.Shutdown())
        notify.ContextMenuStrip = menu
        AddHandler notify.DoubleClick, Sub() ShowWindow()
        revertTimer = New DispatcherTimer() With {.Interval = TimeSpan.FromMilliseconds(200)}
        AddHandler revertTimer.Tick, Sub()
                                             notify.Icon = defaultIconImage
                                             revertTimer.Stop()
                                         End Sub
    End Sub

    Private Shared Function LoadIcon(resourceUri As String) As Icon
        Dim uri = New Uri(resourceUri, UriKind.RelativeOrAbsolute)
        Using stream = System.Windows.Application.GetResourceStream(uri).Stream
            Return New Icon(stream)
        End Using
    End Function

    Public Sub Show()
        notify.Visible = True
    End Sub

    Public Sub IndicateKeyPress()
        notify.Icon = activeIconImage
        revertTimer.Stop()
        revertTimer.Start()
    End Sub

    Public ReadOnly Property CurrentIcon As Icon
        Get
            Return notify.Icon
        End Get
    End Property

    Public ReadOnly Property DefaultIcon As Icon
        Get
            Return defaultIconImage
        End Get
    End Property

    Public ReadOnly Property ActiveIcon As Icon
        Get
            Return activeIconImage
        End Get
    End Property

    Private Sub ShowWindow()
        owner.Show()
        owner.WindowState = WindowState.Normal
        owner.Activate()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        notify.Visible = False
        notify.Dispose()
        defaultIconImage.Dispose()
        activeIconImage.Dispose()
        revertTimer.Stop()
    End Sub
End Class

