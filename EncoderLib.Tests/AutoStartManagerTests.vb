Imports EncoderLib
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System.IO

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Tests for AutoStartManager.
'------------------------------------------------------------------------------
<TestClass>
Public Class AutoStartManagerTests

    <TestMethod>
    Public Sub EnableAndDisableChangeSetting()
        Dim dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())
        Dim settings = AppSettings.Load(dir)
        Dim manager = New AutoStartManager("TestApp", settings)
        manager.Enable()
        Assert.IsTrue(settings.Autostart)
        manager.Disable()
        Assert.IsFalse(settings.Autostart)
    End Sub

End Class

