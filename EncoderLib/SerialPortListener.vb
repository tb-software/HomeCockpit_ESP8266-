Imports System.IO.Ports

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Reads lines from a serial port and exposes them via callback.
'------------------------------------------------------------------------------
Public Class SerialPortListener
    Implements IDisposable

    Private ReadOnly port As SerialPort
    Private ReadOnly lineHandler As Action(Of String)

    Public Sub New(portName As String, lineHandler As Action(Of String))
        Me.lineHandler = lineHandler
        port = New SerialPort(portName, 115200, Parity.None, 8, StopBits.One) With {
            .NewLine = "\n"
        }
        AddHandler port.DataReceived, AddressOf OnDataReceived
        port.Open()
    End Sub

    Private Sub OnDataReceived(sender As Object, e As SerialDataReceivedEventArgs)
        Try
            Dim line = port.ReadLine()
            lineHandler.Invoke(line.Trim())
        Catch
            ' Ignore read errors to avoid crashing the application
        End Try
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        RemoveHandler port.DataReceived, AddressOf OnDataReceived
        If port.IsOpen Then
            port.Close()
        End If
        port.Dispose()
    End Sub
End Class

