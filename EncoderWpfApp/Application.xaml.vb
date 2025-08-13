
Imports System.Windows
Imports System.Windows.Threading
Imports EncoderLib

'------------------------------------------------------------------------------
'  Created: 2025-08-12
'  Edited:  2025-08-12
'  Author:  ChatGPT
'  Description: Application entry point with usage tracking.
'------------------------------------------------------------------------------
Partial Public Class App
    Inherits Application

    Public Shared ReadOnly Tracker As New UsageTracker()

    Public Sub New()
        ' Unhandled-Exception-Handler registrieren
    End Sub

    Protected Overrides Sub OnStartup(e As StartupEventArgs)
        MyBase.OnStartup(e)
        Tracker.Record("AppStarted")
    End Sub

    Private Sub Application_DispatcherUnhandledException(sender As Object, e As DispatcherUnhandledExceptionEventArgs) Handles Me.DispatcherUnhandledException

        ' Uncaught Exception loggen
        MessageBox.Show($"Ein unerwarteter Fehler ist aufgetreten:{Environment.NewLine}{e.Exception.Message}",
                        "Fehler", MessageBoxButton.OK, MessageBoxImage.Error)
        e.Handled = True
    End Sub

    Private Sub Application_Exit(sender As Object, e As ExitEventArgs) Handles Me.Exit

        Tracker.Record("AppExited")
    End Sub

End Class
