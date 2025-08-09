'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Main window showing connection info and autostart.
'------------------------------------------------------------------------------
Imports System.Windows
Imports EncoderLib

Namespace EncoderWpfApp

    Class MainWindow
        Inherits Window

        Private ReadOnly listener As SerialPortListener
        Private ReadOnly parser As New ProtocolParser()
        Private ReadOnly processor As EncoderInputProcessor
        Private ReadOnly keyboard As NotifyingKeyboardSender
        Private ReadOnly settings As AppSettings
        Private ReadOnly autoStart As AutoStartManager
        Private tray As TrayIconManager

        Public Sub New()
            InitializeComponent()
            settings = AppSettings.Load()
            autoStart = New AutoStartManager("EncoderWpfApp", settings)
            AutostartMenuItem.IsChecked = autoStart.IsEnabled()
            keyboard = New NotifyingKeyboardSender(New WindowsKeyboardSender())
            AddHandler keyboard.KeySent, AddressOf OnKeySent
            processor = New EncoderInputProcessor(keyboard)
            listener = New SerialPortListener(settings.ComPort, AddressOf HandleLine)
            ConnectionText.Text = $"Connected to {settings.ComPort}"
            If Environment.GetCommandLineArgs().Contains("--autostart") Then
                tray = New TrayIconManager(Me)
                tray.Show()
                Me.Hide()
            End If
        End Sub

        Private Sub HandleLine(line As String)
            Dim msg As HardwareMessage = Nothing
            If parser.TryParse(line, msg) Then
                Dispatcher.Invoke(Sub()
                                       LastMessageText.Text = line
                                       processor.Process(msg, Date.Now)
                                   End Sub)
            End If
        End Sub

        Private Sub OnKeySent(key As WindowsKey)
            KeyText.Text = $"Last key: {key}"
        End Sub

        Private Sub AutostartMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles AutostartMenuItem.Click
            If AutostartMenuItem.IsChecked Then
                autoStart.Enable()
            Else
                autoStart.Disable()
            End If
        End Sub
    End Class
End Namespace
