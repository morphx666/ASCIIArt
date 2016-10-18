Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

Public Class ASCIIArtCtrl
    Public Enum ColorModes
        GrayScale
        FullGrayScale
        Color
    End Enum

    Public Enum ScanModes
        Fast
        Accurate
    End Enum

    Public Enum Charsets
        Standard = 0
        Advanced = 1
    End Enum

    Public Enum GrayscaleModes
        Average
        Accuarte
    End Enum

    Public Structure ASCIIChar
        Public Property Character As Char
        Public Property Color As Color

        Public Sub New(character As Char, color As Color)
            Me.Character = character
            Me.Color = color
        End Sub
    End Structure

    Private mBitmap As DirectBitmap
    Private mSurface As Bitmap

    Private mCanvasSize As Size = New Size(80, 25)
    Private mCanvas()() As ASCIIChar
    Private mColorMode As ColorModes = ColorModes.GrayScale
    Private mScanMode As ScanModes = ScanModes.Fast
    Private mCharset As Charsets = Charsets.Standard
    Private mGrayScaleMode As GrayscaleModes = GrayscaleModes.Average

    Private lastCanvasSize As Size = New Size(-1, -1)

    Private charsetsChars() As String = {" ·:+x#W@", " ░░▒▒▓▓█"}
    Private activeChars As String = charsetsChars(0)

    Private charSize As Size

    Private sw As New Stopwatch()

    Public Event ImageProcessed(sender As Object, e As EventArgs)

    Private Sub ASCIIArt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.Selectable, True)
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        Me.SetStyle(ControlStyles.SupportsTransparentBackColor, True)

        Me.BackColor = Color.Black

        SetCharSize()
        AddHandler Me.FontChanged, Sub() SetCharSize()
    End Sub

    Public Property CanvasSize As Size
        Get
            Return mCanvasSize
        End Get
        Set(value As Size)
            Dim sizeChange = mCanvasSize <> value
            mCanvasSize = value
            ProcessImage()
        End Set
    End Property

    Public Property Bitmap As Bitmap
        Get
            Return mBitmap
        End Get
        Set(value As Bitmap)
            mBitmap = value
            ProcessImage()
        End Set
    End Property

    Public ReadOnly Property Surface As Bitmap
        Get
            Return mSurface
        End Get
    End Property

    Public Property GrayScaleMode As GrayscaleModes
        Get
            Return mGrayScaleMode
        End Get
        Set(value As GrayscaleModes)
            mGrayScaleMode = value
            ProcessImage()
        End Set
    End Property

    Public Property Charset As Charsets
        Get
            Return mCharset
        End Get
        Set(value As Charsets)
            mCharset = value
            activeChars = charsetsChars(mCharset)
            ProcessImage()
        End Set
    End Property

    Public Property ColorMode As ColorModes
        Get
            Return mColorMode
        End Get
        Set(value As ColorModes)
            mColorMode = value
            ProcessImage()
        End Set
    End Property

    Public Property ScanMode As ScanModes
        Get
            Return mScanMode
        End Get
        Set(value As ScanModes)
            mScanMode = value
            ProcessImage()
        End Set
    End Property

    Public ReadOnly Property ProcessingTime As TimeSpan
        Get
            Return sw.Elapsed
        End Get
    End Property

    Public ReadOnly Property Canvas As ASCIIChar()()
        Get
            Return mCanvas
        End Get
    End Property

    Private Sub SetCharSize()
        charSize = TextRenderer.MeasureText("X", Me.Font)
        charSize.Width -= 8
        charSize.Height -= 1
    End Sub

    Private Sub ProcessImage()
        If mBitmap Is Nothing Then Exit Sub

        sw.Start()

        Dim sx As Integer
        Dim sy As Integer

        Dim sizeChanged As Boolean = (lastCanvasSize <> mCanvasSize)

        If sizeChanged Then
            If mSurface IsNot Nothing Then mSurface.Dispose()
            mSurface = New DirectBitmap(mCanvasSize.Width * charSize.Width, mCanvasSize.Height * charSize.Height)
        End If

        Using surfaceGraphics As Graphics = Graphics.FromImage(mSurface)
            surfaceGraphics.Clear(Me.BackColor)

            Dim scanStep As Size = New Size(Math.Ceiling(mBitmap.Width / mCanvasSize.Width), Math.Ceiling(mBitmap.Height / mCanvasSize.Height))
            scanStep.Width += mCanvasSize.Width Mod scanStep.Width
            scanStep.Height += mCanvasSize.Height Mod scanStep.Height
            Dim scanStepSize = scanStep.Width * scanStep.Height

            If sizeChanged Then ReDim mCanvas(mCanvasSize.Width - 1)

            For x = 0 To mCanvasSize.Width - 1
                If sizeChanged Then ReDim mCanvas(x)(mCanvasSize.Height - 1)
                For y = 0 To mCanvasSize.Height - 1
                    mCanvas(x)(y) = New ASCIIChar(" ", Me.BackColor)
                Next
            Next

            Dim r As Integer
            Dim g As Integer
            Dim b As Integer
            'Dim a As Integer
            Dim gray As Integer

            Dim offset As Integer

            For y As Integer = 0 To mBitmap.Height - scanStep.Height - 1 Step scanStep.Height
                For x As Integer = 0 To mBitmap.Width - scanStep.Width - 1 Step scanStep.Width
                    If mScanMode = ScanModes.Fast Then
                        offset = (x + y * mBitmap.Width) * 4
                        r = mBitmap.Bits(offset + 2)
                        g = mBitmap.Bits(offset + 1)
                        b = mBitmap.Bits(offset + 0)
                    Else
                        r = 0
                        g = 0
                        b = 0

                        For y1 = y To y + scanStep.Height - 1
                            For x1 = x To x + scanStep.Width - 1
                                offset = (x1 + y1 * mBitmap.Width) * 4

                                r += mBitmap.Bits(offset + 2)
                                g += mBitmap.Bits(offset + 1)
                                b += mBitmap.Bits(offset + 0)
                            Next
                        Next

                        r /= scanStepSize
                        g /= scanStepSize
                        b /= scanStepSize
                    End If

                    sx = x / scanStep.Width
                    sy = y / scanStep.Height

                    Select Case mColorMode
                        Case ColorModes.GrayScale
                            mCanvas(sx)(sy) = New ASCIIChar(ColorToASCII(r, g, b), Color.White)
                        Case ColorModes.FullGrayScale
                            gray = ToGrayScale(r, g, b)
                            mCanvas(sx)(sy) = New ASCIIChar(ColorToASCII(r, g, b), Color.FromArgb(gray, gray, gray))
                        Case ColorModes.Color
                            mCanvas(sx)(sy) = New ASCIIChar(ColorToASCII(r, g, b), Color.FromArgb(r, g, b))
                    End Select

                    Using sb As New SolidBrush(mCanvas(sx)(sy).Color)
                        surfaceGraphics.DrawString(mCanvas(sx)(sy).Character, Me.Font, sb, sx * charSize.Width, sy * charSize.Height)
                    End Using
                Next
            Next
        End Using

        sw.Stop()
        Debug.WriteLine(sw.ElapsedMilliseconds)
        sw.Reset()
        RaiseEvent ImageProcessed(Me, New EventArgs())

        lastCanvasSize = mCanvasSize

        Me.Invalidate()
    End Sub

    Private Function ColorToASCII(color As Color) As Char
        Return ColorToASCII(color.R, color.G, color.B)
    End Function

    Private Function ToGrayScale(r As Integer, g As Integer, b As Integer) As Double
        Select Case mGrayScaleMode
            Case GrayscaleModes.Accuarte
                Return r * 0.2126 + g * 0.7152 + b * 0.0722
            Case GrayscaleModes.Average
                Return r * 1 / 3 + g * 1 / 3 + b * 1 / 3
            Case Else
                Return 0
        End Select
    End Function

    Private Function ColorToASCII(r As Integer, g As Integer, b As Integer) As Char
        Return activeChars(Math.Floor(ToGrayScale(r, g, b) / (256 / activeChars.Length)))
    End Function

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        MyBase.OnPaintBackground(e)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If mBitmap Is Nothing OrElse mSurface Is Nothing Then Exit Sub

        Dim g As Graphics = e.Graphics

        g.DrawImageUnscaled(mSurface, 0, 0)
    End Sub
End Class
