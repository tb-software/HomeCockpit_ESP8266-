Imports EncoderLib
Imports Microsoft.VisualStudio.TestTools.UnitTesting

'------------------------------------------------------------------------------
'  Created: 2025-08-12
'  Edited:  2025-08-12
'  Author:  ChatGPT
'  Description: Tests for UsageTracker.
'------------------------------------------------------------------------------
<TestClass>
Public Class UsageTrackerTests
    <TestMethod>
    Public Sub RecordsEvents()
        Dim tracker = New UsageTracker()
        tracker.Record("Start")
        tracker.Record("Start")
        tracker.Record("Stop")
        Dim snap = tracker.Snapshot()
        Assert.AreEqual(2, snap("Start"))
        Assert.AreEqual(1, snap("Stop"))
    End Sub
End Class
