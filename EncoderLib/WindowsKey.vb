'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-09
'  Author:  ChatGPT
'  Description: Windows virtual key codes used by the application.
'------------------------------------------------------------------------------
Public Enum WindowsKey As UShort
    ' Control keys
    Backspace = &H8
    Tab = &H9
    Enter = &HD
    Shift = &H10
    Control = &H11
    Alt = &H12
    Pause = &H13
    CapsLock = &H14
    Escape = &H1B
    Space = &H20
    PageUp = &H21
    PageDown = &H22
    [End] = &H23
    Home = &H24
    Left = &H25
    Up = &H26
    Right = &H27
    Down = &H28
    PrintScreen = &H2C
    Insert = &H2D
    Delete = &H2E
    Help = &H2F

    ' Number keys
    D0 = &H30
    D1 = &H31
    D2 = &H32
    D3 = &H33
    D4 = &H34
    D5 = &H35
    D6 = &H36
    D7 = &H37
    D8 = &H38
    D9 = &H39

    ' Letters
    A = &H41
    B = &H42
    C = &H43
    D = &H44
    E = &H45
    F = &H46
    G = &H47
    H = &H48
    I = &H49
    J = &H4A
    K = &H4B
    L = &H4C
    M = &H4D
    N = &H4E
    O = &H4F
    P = &H50
    Q = &H51
    R = &H52
    S = &H53
    T = &H54
    U = &H55
    V = &H56
    W = &H57
    X = &H58
    Y = &H59
    Z = &H5A

    ' Windows and system keys
    LWin = &H5B
    RWin = &H5C
    Apps = &H5D
    Sleep = &H5F

    ' Numpad
    NumPad0 = &H60
    NumPad1 = &H61
    NumPad2 = &H62
    NumPad3 = &H63
    NumPad4 = &H64
    NumPad5 = &H65
    NumPad6 = &H66
    NumPad7 = &H67
    NumPad8 = &H68
    NumPad9 = &H69
    Multiply = &H6A
    Add = &H6B
    Separator = &H6C
    Subtract = &H6D
    [Decimal] = &H6E
    Divide = &H6F

    ' Function keys
    F1 = &H70
    F2 = &H71
    F3 = &H72
    F4 = &H73
    F5 = &H74
    F6 = &H75
    F7 = &H76
    F8 = &H77
    F9 = &H78
    F10 = &H79
    F11 = &H7A
    F12 = &H7B
    F13 = &H7C
    F14 = &H7D
    F15 = &H7E
    F16 = &H7F
    F17 = &H80
    F18 = &H81
    F19 = &H82
    F20 = &H83
    F21 = &H84
    F22 = &H85
    F23 = &H86
    F24 = &H87

    NumLock = &H90
    ScrollLock = &H91

    VolumeMute = &HAD
    VolumeDown = &HAE
    VolumeUp = &HAF
    MediaNextTrack = &HB0
    MediaPrevTrack = &HB1
    MediaStop = &HB2
    MediaPlayPause = &HB3
    LaunchMail = &HB4
    LaunchMediaSelect = &HB5
    LaunchApp1 = &HB6
    LaunchApp2 = &HB7

    ' OEM specific keys
    OemSemicolon = &HBA
    OemPlus = &HBB
    OemComma = &HBC
    OemMinus = &HBD
    OemPeriod = &HBE
    OemQuestion = &HBF
    OemTilde = &HC0
    OemOpenBrackets = &HDB
    OemPipe = &HDC
    OemCloseBrackets = &HDD
    OemQuotes = &HDE
    Oem8 = &HDF
    OemBackslash = &HE2
End Enum
