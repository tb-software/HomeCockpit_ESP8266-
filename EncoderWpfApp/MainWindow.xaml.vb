'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Main window handling serial input.
'------------------------------------------------------------------------------
Class MainWindow
    Inherits Window

    Private ReadOnly listener As SerialPortListener
    Private ReadOnly parser As New ProtocolParser()
    Private ReadOnly processor As EncoderInputProcessor

    Public Sub New()
        InitializeComponent()
        processor = New EncoderInputProcessor(New WindowsKeyboardSender())
        listener = New SerialPortListener("COM4", AddressOf HandleLine)
    End Sub

    Private Sub HandleLine(line As String)
        Dim msg As HardwareMessage = Nothing
        If parser.TryParse(line, msg) Then
            Dispatcher.Invoke(Sub() processor.Process(msg, Date.Now))
        End If
    End Sub
End Class
