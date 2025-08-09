Imports EncoderLib
Imports Microsoft.VisualStudio.TestTools.UnitTesting

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Tests for ProtocolParser covering firmware message formats.
'------------------------------------------------------------------------------
<TestClass>
Public Class ProtocolParserTests

    <TestMethod>
    Public Sub ParsesEncoderLine()
        Dim parser As New ProtocolParser()
        Dim msg As HardwareMessage = Nothing
        Dim ok = parser.TryParse("Encoder: 5", msg)
        Assert.IsTrue(ok)
        Dim enc = TryCast(msg, EncoderMessage)
        Assert.IsNotNull(enc)
        Assert.AreEqual(5, enc.Position)
    End Sub

    <TestMethod>
    Public Sub ParsesButtonLine()
        Dim parser As New ProtocolParser()
        Dim msg As HardwareMessage = Nothing
        Dim ok = parser.TryParse("Button: PRESS", msg)
        Assert.IsTrue(ok)
        Assert.IsInstanceOfType(msg, GetType(ButtonMessage))
    End Sub

    <TestMethod>
    Public Sub IgnoresInvalidLine()
        Dim parser As New ProtocolParser()
        Dim msg As HardwareMessage = Nothing
        Dim ok = parser.TryParse("alive", msg)
        Assert.IsFalse(ok)
        Assert.IsNull(msg)
    End Sub
End Class
