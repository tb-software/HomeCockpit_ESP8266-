Imports System.ComponentModel
Imports Microsoft.VisualStudio.TestTools.UnitTesting

'------------------------------------------------------------------------------
'  Created: 2025-09-01
'  Edited:  2025-09-01
'  Author:  ChatGPT
'  Description: Tests for Win32ErrorHelper message generation.
'------------------------------------------------------------------------------
<TestClass>
Public Class Win32ErrorHelperTests
    <TestMethod>
    Public Sub AccessDeniedMentionsAdministrator()
        Dim ex As New Win32Exception(5)
        Dim msg = Win32ErrorHelper.ToMessage(ex)
        StringAssert.Contains(msg, "Administrator")
    End Sub

    <TestMethod>
    Public Sub OtherErrorsIncludeCode()
        Dim ex As New Win32Exception(2)
        Dim msg = Win32ErrorHelper.ToMessage(ex)
        StringAssert.Contains(msg, "Fehler 2")
        StringAssert.Contains(msg, ex.Message)
    End Sub
End Class
