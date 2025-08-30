Imports System.Reflection
Imports System.Runtime.InteropServices
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass>
Public Class WindowsKeyboardSenderStructTests
    <TestMethod>
    Public Sub InputStructHasExpectedSize()
        Dim t = GetType(WindowsKeyboardSender).GetNestedType("INPUT", BindingFlags.NonPublic)
        Assert.IsNotNull(t)
        Dim size = Marshal.SizeOf(t)
        Assert.IsTrue(size >= 40, $"INPUT size {size} is too small")
    End Sub
End Class
