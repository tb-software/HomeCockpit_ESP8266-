Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System.Threading.Tasks
Imports System.Threading
Imports EncoderWpfApp

'------------------------------------------------------------------------------
'  Created: 2025-08-12
'  Edited:  2025-08-12
'  Author:  ChatGPT
'  Description: Tests TrayIconManager key press indication.
'------------------------------------------------------------------------------
Namespace EncoderWpfApp.Tests
    <TestClass>
    Public Class TrayIconManagerTests
        <TestMethod>
        <Apartment(ApartmentState.STA)>
        Public Async Function IndicateKeyPressTogglesIcon() As Task
            Using mgr As New TrayIconManager(New Window())
                mgr.Show()
                Assert.IsTrue(mgr.CurrentIcon.Equals(mgr.DefaultIcon))
                mgr.IndicateKeyPress()
                Assert.IsTrue(mgr.CurrentIcon.Equals(mgr.ActiveIcon))
                Await Task.Delay(250)
                Assert.IsTrue(mgr.CurrentIcon.Equals(mgr.DefaultIcon))
            End Using
        End Function
    End Class
End Namespace
