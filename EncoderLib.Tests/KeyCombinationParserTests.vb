Imports EncoderLib
Imports Microsoft.VisualStudio.TestTools.UnitTesting

'------------------------------------------------------------------------------
'  Created: 2025-08-12
'  Edited:  2025-08-12
'  Author:  ChatGPT
'  Description: Tests for KeyCombinationParser.
'------------------------------------------------------------------------------
<TestClass>
Public Class KeyCombinationParserTests
    <TestMethod>
    Public Sub ParsesCtrlA()
        Dim keys = KeyCombinationParser.Parse("Ctrl+A")
        Assert.AreEqual(2, keys.Count)
        Assert.AreEqual(WindowsKey.Control, keys(0))
        Assert.AreEqual(WindowsKey.A, keys(1))
    End Sub
End Class
