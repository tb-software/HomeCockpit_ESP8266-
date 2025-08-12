Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System.Threading
Imports System.Windows
Imports EncoderWpfApp

'------------------------------------------------------------------------------
'  Created: 2025-08-12
'  Edited:  2025-08-13
'  Author:  ChatGPT
'  Description: Tests TrayIconManager key press indication.
'------------------------------------------------------------------------------
Namespace EncoderWpfApp.Tests
    <TestClass>
    Public Class TrayIconManagerTests
        <TestMethod>
        Public Sub IndicateKeyPressTogglesIcon()
            Dim ex As Exception = Nothing
            Dim t = New Thread(Sub()
                                   Try
                                       Using mgr As New TrayIconManager(New Window())
                                           mgr.Show()
                                           Assert.IsTrue(mgr.CurrentIcon.Equals(mgr.DefaultIcon))
                                           mgr.IndicateKeyPress()
                                           Assert.IsTrue(mgr.CurrentIcon.Equals(mgr.ActiveIcon))
                                           Thread.Sleep(250)
                                           Assert.IsTrue(mgr.CurrentIcon.Equals(mgr.DefaultIcon))
                                       End Using
                                   Catch e As Exception
                                       ex = e
                                   End Try
                               End Sub)
            t.SetApartmentState(ApartmentState.STA)
            t.Start()
            t.Join()
            If ex IsNot Nothing Then
                Throw ex
            End If
        End Sub
    End Class
End Namespace
