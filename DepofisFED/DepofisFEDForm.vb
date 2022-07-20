Imports System.Net

Public Class DepofisFEDForm
    'args
    Private args() As String = Environment.GetCommandLineArgs()
    Private _timeFormat As String = "yyyyMMdd_HHmmss"
    Private _feUri As String
    Private _fePath As String
    Private _feNum As String
    Private _feName As String
    Private _myDate As String
    'msg
    Public Shared Msg, Style, Title, Response
    'properties
    Public Property feUri As String
        Get
            Return _feUri
        End Get
        Set(ByVal valor As String)
            _feUri = valor
        End Set
    End Property
    Public Property fePath As String
        Get
            Return _fePath
        End Get
        Set(ByVal valor As String)
            _fePath = valor
        End Set
    End Property
    Public Property feNum As String
        Get
            Return _feNum
        End Get
        Set(ByVal valor As String)
            _feNum = valor
        End Set
    End Property
    Public Property feName As String
        Get
            Return _feName
        End Get
        Set(ByVal valor As String)
            _feName = valor
        End Set
    End Property
    Public Property myDate As String
        Get
            Return _myDate
        End Get
        Set(ByVal valor As String)
            _myDate = valor
        End Set
    End Property
    Public Property timeFormat As String
        Get
            Return _timeFormat
        End Get
        Set(ByVal valor As String)
            _timeFormat = valor
        End Set
    End Property
    ''' <summary>
    ''' .
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            'hide buttons
            Me.Button1.Visible = False
            Me.Button2.Visible = False
            'args checker
            If args.Length = 1 Then
                Throw New System.Exception("Error al recibir uno o mas parametros.")
            End If
            'setters
            Me.myDate = DateTime.Now.ToString(Me.timeFormat)
            Me.feUri = args(1).Trim
            Me.fePath = args(2).Trim
            Me.feNum = args(3).Trim
            Me.feName = Me.feNum & "_" & Me.myDate & ".PDF"
            Me.Label1.Text = "Descargando comprobante en linea, por favor espere..."
            'set download config

            Dim feDownload As New WebClient

            ServicePointManager.Expect100Continue = True
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            'download file
            AddHandler feDownload.DownloadProgressChanged, AddressOf DownloadProgressChanged
            AddHandler feDownload.DownloadFileCompleted, AddressOf DownloadFileCompleted

            feDownload.DownloadFileAsync(New System.Uri(Me.feUri), Me.fePath & "\" & feName)

        Catch exToLoadArgs As Exception
            Msg = exToLoadArgs.Message
            Style = vbCritical
            Title = "Error DepofisFED"
            Response = MsgBox(Msg, Style, Title)
            'exit on catch with errors
            Me.Label1.Text = exToLoadArgs.Message
            Me.Button2.Text = "Salir"
            Me.Button2.Visible = True
        End Try
    End Sub
    ''' <summary>
    ''' .
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'yes
        'Process.Start("explorer.exe", Me.fePath)
        Process.Start(Me.fePath & "\" & Me.feName)
        Me.Close()
    End Sub
    ''' <summary>
    ''' .
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'no
        'Process.Start("explorer.exe", Me.fePath)
        Me.Close()
    End Sub
    ''' <summary>
    ''' .
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs)
        Me.ProgressBar1.Value = e.ProgressPercentage
        Me.Label1.Text = e.BytesReceived & "/" & e.TotalBytesToReceive
    End Sub

    ''' <summary>
    ''' .
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub DownloadFileCompleted(sender As Object, e As EventArgs)
        Me.Label1.Text = "Se descargo un comprobante electrónico. ¿Desea abrirlo?"
        Me.Button1.Visible = True
        Me.Button2.Visible = True
    End Sub

End Class
