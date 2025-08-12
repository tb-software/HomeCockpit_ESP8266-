Imports System
Imports System.Collections.Generic
Imports System.Timers

'------------------------------------------------------------------------------
'  Created: 2025-08-09
'  Edited:  2025-08-11
'  Author:  ChatGPT
'  Description: Processes hardware messages and triggers keyboard actions.
'------------------------------------------------------------------------------
Public Class EncoderInputProcessor

    Private ReadOnly keyboard As IKeyboardSender
    Public Property Mapper As KeyMapper
    Private lastPosition As Integer?
    Private buttonPressed As Boolean
    Private buttonPressStart As DateTime
    Private lastButtonSignal As DateTime
    Private ReadOnly releaseThreshold As TimeSpan = TimeSpan.FromMilliseconds(200)
    Private ReadOnly longPressThreshold As TimeSpan = TimeSpan.FromMilliseconds(800)
    Private ReadOnly releaseTimer As Timer
    Private ReadOnly stateLock As New Object()

    Public Sub New(keyboard As IKeyboardSender)
        Me.keyboard = keyboard
        Me.Mapper = New KeyMapper()
        releaseTimer = CreateReleaseTimer()
    End Sub

    Public Sub New(keyboard As IKeyboardSender, mapper As KeyMapper)
        Me.keyboard = keyboard
        Me.Mapper = mapper
        releaseTimer = CreateReleaseTimer()
    End Sub

    Public Sub Process(message As HardwareMessage, timestamp As DateTime)
        SyncLock stateLock
            If TypeOf message Is ButtonMessage Then
                ProcessButtonPress(timestamp)
            ElseIf TypeOf message Is EncoderMessage Then
                ProcessEncoder(CType(message, EncoderMessage))
            End If
            CheckButtonRelease(timestamp)
        End SyncLock
    End Sub

    Private Sub ProcessButtonPress(timestamp As DateTime)
        If Not buttonPressed Then
            buttonPressed = True
            buttonPressStart = timestamp
        End If
        lastButtonSignal = timestamp
    End Sub

    Private Sub ProcessEncoder(msg As EncoderMessage)
        If lastPosition.HasValue Then
            Dim stepCount = msg.Position - lastPosition.Value
            If stepCount <> 0 Then
                Dim combo As IReadOnlyList(Of WindowsKey)
                If buttonPressed Then
                    combo = KeyCombinationParser.Parse(If(stepCount > 0, Mapper.RotateUpWithButton, Mapper.RotateDownWithButton))
                Else
                    combo = KeyCombinationParser.Parse(If(stepCount > 0, Mapper.RotateUp, Mapper.RotateDown))
                End If
                For i = 1 To Math.Abs(stepCount)
                    keyboard.SendKeys(combo)
                Next
            End If
        End If
        lastPosition = msg.Position
    End Sub

    Private Sub CheckButtonRelease(timestamp As DateTime)
        If buttonPressed AndAlso timestamp - lastButtonSignal > releaseThreshold Then
            Dim duration = timestamp - buttonPressStart
            If duration >= longPressThreshold Then
                keyboard.SendKeys(KeyCombinationParser.Parse(Mapper.ButtonLongPress))
            Else
                keyboard.SendKeys(KeyCombinationParser.Parse(Mapper.ButtonPress))
            End If
            buttonPressed = False
        End If
    End Sub

    Private Function CreateReleaseTimer() As Timer
        Dim t As New Timer(50)
        AddHandler t.Elapsed, AddressOf OnReleaseTimer
        t.AutoReset = True
        t.Start()
        Return t
    End Function

    Private Sub OnReleaseTimer(sender As Object, e As ElapsedEventArgs)
        SyncLock stateLock
            CheckButtonRelease(DateTime.UtcNow)
        End SyncLock
    End Sub
End Class
