
Imports System.Windows
Imports System.Windows.Threading

Partial Public Class App
    Inherits Application

    Public Sub New()
        ' Unhandled-Exception-Handler registrieren
    End Sub

    Private Sub Application_DispatcherUnhandledException(sender As Object, e As DispatcherUnhandledExceptionEventArgs) Handles Me.DispatcherUnhandledException

        ' Uncaught Exception loggen
        MessageBox.Show($"Ein unerwarteter Fehler ist aufgetreten:{Environment.NewLine}{e.Exception.Message}",
                        "Fehler", MessageBoxButton.OK, MessageBoxImage.Error)
        e.Handled = True
    End Sub

    Private Sub Application_Exit(sender As Object, e As ExitEventArgs) Handles Me.Exit

        ' Programmende loggen
    End Sub



End Class
