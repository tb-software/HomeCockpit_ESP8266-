Imports EncoderLib
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System.IO

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Tests for AppSettings persistence.
'------------------------------------------------------------------------------
<TestClass>
Public Class AppSettingsTests

    <TestMethod>
    Public Sub SaveAndLoadRoundtrip()
        Dim dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())
        Dim settings = New AppSettings() With {
            .Autostart = True,
            .ComPort = "COM7"
        }
        settings.Save(dir)
        Dim loaded = AppSettings.Load(dir)
        Assert.IsTrue(loaded.Autostart)
        Assert.AreEqual("COM7", loaded.ComPort)
    End Sub

End Class

