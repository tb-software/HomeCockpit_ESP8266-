Imports System.Threading.Tasks
Imports System.Windows
Imports EncoderLib

'------------------------------------------------------------------------------
'  Created: 2025-08-12
'  Edited:  2025-08-12
'  Author:  ChatGPT
'  Description: Simulates hardware events via buttons.
'------------------------------------------------------------------------------
Namespace EncoderWpfApp
    Partial Class HardwareSimulatorWindow
        Inherits Window

        Private ReadOnly processor As EncoderInputProcessor
        Private position As Integer

        Public Sub New(proc As EncoderInputProcessor)
            InitializeComponent()
            processor = proc
            position = 0
        End Sub

        Private Sub RotateUp_Click(sender As Object, e As RoutedEventArgs)
            position += 1
            processor.Process(New EncoderMessage(position, RotationDirection.Clockwise), DateTime.UtcNow)
        End Sub

        Private Sub RotateDown_Click(sender As Object, e As RoutedEventArgs)
            position -= 1
            processor.Process(New EncoderMessage(position, RotationDirection.CounterClockwise), DateTime.UtcNow)
        End Sub

        Private Sub RotateUpBtn_Click(sender As Object, e As RoutedEventArgs)
            processor.Process(New ButtonMessage(), DateTime.UtcNow)
            RotateUp_Click(sender, e)
        End Sub

        Private Sub RotateDownBtn_Click(sender As Object, e As RoutedEventArgs)
            processor.Process(New ButtonMessage(), DateTime.UtcNow)
            RotateDown_Click(sender, e)
        End Sub

        Private Sub ButtonPress_Click(sender As Object, e As RoutedEventArgs)
            processor.Process(New ButtonMessage(), DateTime.UtcNow)
        End Sub

        Private Async Sub ButtonLongPress_Click(sender As Object, e As RoutedEventArgs)
            processor.Process(New ButtonMessage(), DateTime.UtcNow)
            For i = 1 To 6
                Await Task.Delay(150)
                processor.Process(New ButtonMessage(), DateTime.UtcNow)
            Next
        End Sub
    End Class
End Namespace
