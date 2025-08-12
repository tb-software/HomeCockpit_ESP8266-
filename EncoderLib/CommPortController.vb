Imports System.IO.Ports
Imports System.Threading

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Manages serial port selection and connection with error handling.
'------------------------------------------------------------------------------
Public Class CommPortController
    Private ReadOnly lineHandler As Action(Of String)
    Private ReadOnly listPorts As Func(Of String())
    Private ReadOnly createListener As Func(Of String, Action(Of String), ISerialPortListener)
    Private listener As ISerialPortListener

    Public Sub New(lineHandler As Action(Of String), Optional listPorts As Func(Of String()) = Nothing, Optional createListener As Func(Of String, Action(Of String), ISerialPortListener) = Nothing)
        Me.lineHandler = lineHandler
        Me.listPorts = If(listPorts, Function() SerialPort.GetPortNames())
        Me.createListener = If(createListener, Function(name, handler) CType(New SerialPortListener(name, handler), ISerialPortListener))
    End Sub

    Public Property CurrentPort As String
        Get
            Return _currentPort
        End Get
        Private Set(value As String)
            _currentPort = value
        End Set
    End Property
    Private _currentPort As String

    Public ReadOnly Property LastError As String
        Get
            Return _lastError
        End Get
    End Property
    Private _lastError As String

    Public Event Disconnected()

    Public Function Connect(portSetting As String) As Boolean
        Disconnect()
        _lastError = Nothing
        Dim port = ResolvePort(portSetting)
        If port Is Nothing Then
            _lastError = "No available ports"
            CurrentPort = Nothing
            Return False
        End If
        Try
            listener = createListener(port, lineHandler)
            AddHandler listener.Disconnected, AddressOf OnListenerDisconnected
            CurrentPort = port
            Return True
        Catch ex As Exception
            _lastError = ex.Message
            CurrentPort = Nothing
            Return False
        End Try
    End Function

    Public Sub Disconnect()
        If listener IsNot Nothing Then
            RemoveHandler listener.Disconnected, AddressOf OnListenerDisconnected
            listener.Dispose()
            listener = Nothing
        End If
        CurrentPort = Nothing
    End Sub

    Private Sub OnListenerDisconnected()
        Disconnect()
        RaiseEvent Disconnected()
    End Sub

    Private Function ResolvePort(setting As String) As String
        If String.IsNullOrWhiteSpace(setting) OrElse setting.Equals("Auto", StringComparison.OrdinalIgnoreCase) Then
            Return listPorts().FirstOrDefault()
        End If
        Return setting
    End Function
End Class

