Imports System.IO.Ports
Imports System.Threading
Imports System.Threading.Tasks

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Reads lines from a serial port and exposes them via callback.
'------------------------------------------------------------------------------
Public Class SerialPortListener
    Implements ISerialPortListener

    Private ReadOnly port As SerialPort
    Private ReadOnly lineHandler As Action(Of String)
    Private ReadOnly cancel As CancellationTokenSource

    Public Sub New(portName As String, lineHandler As Action(Of String))
        Me.lineHandler = lineHandler
        port = New SerialPort(portName, 115200, Parity.None, 8, StopBits.One) With {
            .NewLine = "\n",
            .DtrEnable = True,
            .RtsEnable = True
        }
        port.Open()
        cancel = New CancellationTokenSource()
        Task.Run(Sub() ReadLoop(), cancel.Token)
    End Sub

    Private Sub ReadLoop()
        Try
            While Not cancel.IsCancellationRequested
                Dim line = port.ReadLine()
                lineHandler.Invoke(line.Trim())
            End While
        Catch
            RaiseEvent Disconnected()
        End Try
    End Sub

    Public Event Disconnected() Implements ISerialPortListener.Disconnected

    Public Sub Dispose() Implements IDisposable.Dispose
        cancel.Cancel()
        If port.IsOpen Then
            port.Close()
        End If
        port.Dispose()
    End Sub
End Class

