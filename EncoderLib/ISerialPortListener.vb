Imports System

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Interface for serial port listeners that signal disconnections.
'------------------------------------------------------------------------------
Public Interface ISerialPortListener
    Inherits IDisposable
    Event Disconnected()
End Interface
