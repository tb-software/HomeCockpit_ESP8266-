Imports System
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports System.Windows
Imports System.Windows.Controls
Imports EncoderLib

'------------------------------------------------------------------------------
'  Created: 2025-08-12
'  Edited:  2025-08-31
'  Author:  ChatGPT
'  Description: Simulates hardware events via buttons.
'------------------------------------------------------------------------------
Public Partial Class HardwareSimulatorWindow
    Inherits Window

    Private ReadOnly processor As EncoderInputProcessor
    Private ReadOnly delayProvider As Func(Of TimeSpan, Task)
    Private position As Integer

    Public Sub New(proc As EncoderInputProcessor, Optional delayProvider As Func(Of TimeSpan, Task) = Nothing)
        InitializeComponent()
        processor = proc
        Me.delayProvider = If(delayProvider, Function(ts) Task.Delay(ts))
        position = 0
    End Sub

    Private Sub ProcessSafe(msg As HardwareMessage)
        Try
            processor.Process(msg, DateTime.UtcNow)
        Catch ex As Win32Exception
            MessageBox.Show("Die Tasten konnten nicht gesendet werden. Bitte als Administrator ausführen.", "HomeCockpit", MessageBoxButton.OK, MessageBoxImage.Warning)
        End Try
    End Sub

    Private Sub RotateUp_Click(sender As Object, e As RoutedEventArgs)
        position += 1
        ProcessSafe(New EncoderMessage(position, RotationDirection.Clockwise))
    End Sub

    Private Sub RotateDown_Click(sender As Object, e As RoutedEventArgs)
        position -= 1
        ProcessSafe(New EncoderMessage(position, RotationDirection.CounterClockwise))
    End Sub

    Private Sub RotateUpBtn_Click(sender As Object, e As RoutedEventArgs)
        ProcessSafe(New ButtonMessage())
        RotateUp_Click(sender, e)
    End Sub

    Private Sub RotateDownBtn_Click(sender As Object, e As RoutedEventArgs)
        ProcessSafe(New ButtonMessage())
        RotateDown_Click(sender, e)
    End Sub

    Private Sub ButtonPress_Click(sender As Object, e As RoutedEventArgs)
        ProcessSafe(New ButtonMessage())
    End Sub

    Private Async Function PerformButtonLongPressAsync() As Task
        ProcessSafe(New ButtonMessage())
        For i = 1 To 6
            Await delayProvider(TimeSpan.FromMilliseconds(150))
            ProcessSafe(New ButtonMessage())
        Next
    End Function

    Private Async Sub ButtonLongPress_Click(sender As Object, e As RoutedEventArgs)
        Await PerformButtonLongPressAsync()
    End Sub

    Public Async Function ClickAllButtonsAsync() As Task
        Await delayProvider(TimeSpan.FromSeconds(1))
        RotateUp_Click(Me, New RoutedEventArgs())
        Await delayProvider(TimeSpan.FromSeconds(1))
        RotateDown_Click(Me, New RoutedEventArgs())
        Await delayProvider(TimeSpan.FromSeconds(1))
        RotateUpBtn_Click(Me, New RoutedEventArgs())
        Await delayProvider(TimeSpan.FromSeconds(1))
        RotateDownBtn_Click(Me, New RoutedEventArgs())
        Await delayProvider(TimeSpan.FromSeconds(1))
        ButtonPress_Click(Me, New RoutedEventArgs())
        Await delayProvider(TimeSpan.FromSeconds(1))
        Await PerformButtonLongPressAsync()
    End Function

    Private Async Sub RunAllButtons_Click(sender As Object, e As RoutedEventArgs)
        Dim btn = CType(sender, Button)
        btn.IsEnabled = False
        Try
            Await delayProvider(TimeSpan.FromSeconds(3))
            Await ClickAllButtonsAsync()
        Finally
            btn.IsEnabled = True
        End Try
    End Sub
End Class

