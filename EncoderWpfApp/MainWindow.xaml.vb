'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-12
'  Author:  ChatGPT
'  Description: Main window showing connection info and autostart.
'------------------------------------------------------------------------------
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Threading
Imports EncoderLib
Imports System.Collections.Generic

Namespace EncoderWpfApp

    Partial Class MainWindow
        Inherits Window

        Private controller As CommPortController
        Private ReadOnly parser As New ProtocolParser()
        Private processor As EncoderInputProcessor
        Private keyboard As NotifyingKeyboardSender
        Private settings As AppSettings
        Private autoStart As AutoStartManager
        Private tray As TrayIconManager

        Public Sub New()
            InitializeComponent()
            Dim appVersion = My.Application.Info.Version.ToString()
            Me.Title = $"HomeCockpit  - Jonas Lang / Timo Boehme - V{appVersion}"
            VersionText.Text = $"Version: {appVersion}"
            settings = AppSettings.Load()
            autoStart = New AutoStartManager("EncoderWpfApp", settings)
            AutostartMenuItem.IsChecked = autoStart.IsEnabled()
            keyboard = New NotifyingKeyboardSender(New WindowsKeyboardSender())
            AddHandler keyboard.KeysSent, AddressOf OnKeysSent
            processor = New EncoderInputProcessor(keyboard)
            processor.Mapper = settings.KeyMapping
            controller = New CommPortController(AddressOf HandleLine)
            AddHandler controller.Disconnected, AddressOf OnControllerDisconnected
            PopulateComPorts()
            ConnectPort()
            tray = New TrayIconManager(Me)
            tray.Show()
            AddHandler Me.StateChanged, AddressOf OnStateChanged
            AddHandler Me.Closed, Sub() tray.Dispose()
            If Environment.GetCommandLineArgs().Contains("--autostart") Then
                Me.Hide()
            End If
        End Sub

        Private Sub HandleLine(line As String)
            Dim msg As HardwareMessage = Nothing
            Dim isVersion = line.StartsWith("Version:", StringComparison.OrdinalIgnoreCase)
            Dim parsed = parser.TryParse(line, msg)
            Dispatcher.Invoke(Sub()
                                   LastMessageText.Text = line
                                   If isVersion Then
                                       VersionText.Text = line.Trim()
                                   ElseIf parsed Then
                                       processor.Process(msg, Date.Now)
                                   End If
                               End Sub)
        End Sub

        Private Sub OnKeysSent(keys As IReadOnlyList(Of WindowsKey))
            KeyText.Text = $"Last key: {String.Join(" + ", keys)}"
            tray.IndicateKeyPress()
        End Sub

        Private Sub AutostartMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles AutostartMenuItem.Click
            If AutostartMenuItem.IsChecked Then
                autoStart.Enable()
            Else
                autoStart.Disable()
            End If
        End Sub

        Private Sub PopulateComPorts()
            ComPortMenuItem.Items.Clear()
            Dim autoItem = New MenuItem() With {.Header = "_Auto", .IsCheckable = True, .IsChecked = settings.ComPort = "Auto"}
            AddHandler autoItem.Click, Sub() SetComPort("Auto")
            ComPortMenuItem.Items.Add(autoItem)
            For Each port In System.IO.Ports.SerialPort.GetPortNames()
                Dim item = New MenuItem() With {.Header = port, .IsCheckable = True, .IsChecked = settings.ComPort = port}
                AddHandler item.Click, Sub() SetComPort(port)
                ComPortMenuItem.Items.Add(item)
            Next
        End Sub

        Private Sub SetComPort(port As String)
            settings.ComPort = port
            settings.Save()
            PopulateComPorts()
            ConnectPort()
        End Sub

        Private reconnectTimer As DispatcherTimer

        Private Sub ConnectPort()
            If controller.Connect(settings.ComPort) Then
                ConnectionText.Text = $"Connected to {controller.CurrentPort}"
                StopReconnect()
            Else
                If String.IsNullOrWhiteSpace(controller.LastError) Then
                    ConnectionText.Text = "Disconnected"
                Else
                    ConnectionText.Text = $"Error: {controller.LastError}"
                End If
                StartReconnect()
            End If
        End Sub

        Private Sub OnControllerDisconnected()
            Dispatcher.Invoke(Sub()
                                   ConnectionText.Text = "Disconnected"
                                   StartReconnect()
                               End Sub)
        End Sub

        Private Sub StartReconnect()
            If reconnectTimer Is Nothing Then
                reconnectTimer = New DispatcherTimer()
                reconnectTimer.Interval = TimeSpan.FromSeconds(1)
                AddHandler reconnectTimer.Tick, Sub() ConnectPort()
                reconnectTimer.Start()
            End If
        End Sub

        Private Sub StopReconnect()
            If reconnectTimer IsNot Nothing Then
                reconnectTimer.Stop()
                reconnectTimer = Nothing
            End If
        End Sub

        Private Sub KeyMappingMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles KeyMappingMenuItem.Click
            Dim dlg = New KeyMappingWindow(settings.KeyMapping)
            If dlg.ShowDialog() Then
                settings.Save()
                processor.Mapper = settings.KeyMapping
            End If
        End Sub

        Private Sub SimulatorMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles SimulatorMenuItem.Click
            Dim sim As New HardwareSimulatorWindow(processor)
            sim.Show()
        End Sub

        Private Sub OnStateChanged(sender As Object, e As EventArgs)
            If WindowState = WindowState.Minimized Then
                Me.Hide()
            End If
        End Sub
    End Class
End Namespace
