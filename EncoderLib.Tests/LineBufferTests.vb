Imports EncoderLib
Imports Microsoft.VisualStudio.TestTools.UnitTesting

'------------------------------------------------------------------------------
'  Created: 2025-08-10
'  Edited:  2025-08-10
'  Author:  ChatGPT
'  Description: Tests for LineBuffer.
'------------------------------------------------------------------------------
<TestClass>
Public Class LineBufferTests
    <TestMethod>
    Public Sub ExtractsLinesWithMixedTerminators()
        Dim buf = New LineBuffer()
        Dim lines = buf.ExtractLines("a" & vbCrLf & "b" & vbLf & "c" & vbCr & "d")
        Assert.AreEqual(3, lines.Count)
        Assert.AreEqual("a", lines(0))
        Assert.AreEqual("b", lines(1))
        Assert.AreEqual("c", lines(2))
        lines = buf.ExtractLines(vbLf)
        Assert.AreEqual(1, lines.Count)
        Assert.AreEqual("d", lines(0))
    End Sub
End Class
