Imports EncoderLib
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System.IO

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-12
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
            .ComPort = "COM7",
            .KeyMapping = New KeyMapper() With {
                .RotateUp = "PageUp",
                .ButtonPress = "Escape"
            }
        }
        settings.Save(dir)
        Dim loaded = AppSettings.Load(dir)
        Assert.IsTrue(loaded.Autostart)
        Assert.AreEqual("COM7", loaded.ComPort)
        Assert.AreEqual("PageUp", loaded.KeyMapping.RotateUp)
        Assert.AreEqual("Escape", loaded.KeyMapping.ButtonPress)
    End Sub

    <TestMethod>
    Public Sub LoadLegacyNumericKeyMapping()
        Dim dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())
        Directory.CreateDirectory(dir)
        Dim json = "{""Autostart"":false,""ComPort"":""Auto"",""KeyMapping"":{""RotateUp"":38,""RotateDown"":40,""ButtonPress"":13}}"
        File.WriteAllText(Path.Combine(dir, "settings.json"), json)
        Dim loaded = AppSettings.Load(dir)
        Assert.AreEqual("Up", loaded.KeyMapping.RotateUp)
        Assert.AreEqual("Down", loaded.KeyMapping.RotateDown)
        Assert.AreEqual("Enter", loaded.KeyMapping.ButtonPress)
    End Sub

End Class

