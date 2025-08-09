Imports System.IO.Ports

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Reads lines from serial port.
'------------------------------------------------------------------------------
Public Class SerialPortListener
    Private ReadOnly port As SerialPort
    Private ReadOnly lineHandler As Action(Of String)

    Public Sub New(portName As String, lineHandler As Action(Of String))
        Me.lineHandler = lineHandler
        port = New SerialPort(portName, 115200, Parity.None, 8, StopBits.One)
        port.NewLine = "\n"
        AddHandler port.DataReceived, AddressOf OnDataReceived
        port.Open()
    End Sub

    Private Sub OnDataReceived(sender As Object, e As SerialDataReceivedEventArgs)
        Dim line = port.ReadLine()
        lineHandler.Invoke(line.Trim())
    End Sub
End Class
