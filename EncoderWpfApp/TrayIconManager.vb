Imports System.Windows
Imports System.Windows.Forms
Imports System.Reflection
Imports System.Windows.Threading
Imports System.Drawing

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-12
'  Author:  ChatGPT
'  Description: Manages system tray icon and key press indication.
'------------------------------------------------------------------------------
Public Class TrayIconManager
    Implements IDisposable

    Private Const DefaultIconResource As String = "EncoderWpfApp.HomeCockpit_Icon_DEFAULT.ico"
    Private Const ActiveIconResource As String = "EncoderWpfApp.HomeCockpit_Icon_ACTIVE.ico"

    Private ReadOnly notify As NotifyIcon
    Private ReadOnly owner As Window
    Private ReadOnly defaultIcon As Icon
    Private ReadOnly activeIcon As Icon
    Private ReadOnly revertTimer As DispatcherTimer

    Public Sub New(owner As Window)
        Me.owner = owner
        notify = New NotifyIcon()
        defaultIcon = LoadIcon(DefaultIconResource)
        activeIcon = LoadIcon(ActiveIconResource)
        notify.Icon = defaultIcon
        notify.Text = "Encoder Input"
        notify.Visible = False
        Dim menu = New ContextMenuStrip()
        menu.Items.Add("Show", Nothing, Sub() ShowWindow())
        menu.Items.Add("Exit", Nothing, Sub() Application.Current.Shutdown())
        notify.ContextMenuStrip = menu
        AddHandler notify.DoubleClick, Sub() ShowWindow()
        revertTimer = New DispatcherTimer() With {.Interval = TimeSpan.FromMilliseconds(200)}
        AddHandler revertTimer.Tick, Sub()
                                             notify.Icon = defaultIcon
                                             revertTimer.Stop()
                                         End Sub
    End Sub

    Private Shared Function LoadIcon(resourceName As String) As Icon
        Dim asm = Assembly.GetExecutingAssembly()
        Using stream = asm.GetManifestResourceStream(resourceName)
            Return New Icon(stream)
        End Using
    End Function

    Public Sub Show()
        notify.Visible = True
    End Sub

    Public Sub IndicateKeyPress()
        notify.Icon = activeIcon
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
            Return defaultIcon
        End Get
    End Property

    Public ReadOnly Property ActiveIcon As Icon
        Get
            Return activeIcon
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
        defaultIcon.Dispose()
        activeIcon.Dispose()
        revertTimer.Stop()
    End Sub
End Class

