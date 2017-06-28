Public Class ASCIIArtCtrl
    Private mI2A As New Image2Ascii()

    Private Sub ASCIIArt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.Selectable, True)
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        Me.SetStyle(ControlStyles.SupportsTransparentBackColor, True)

        AddHandler Me.FontChanged, Sub() mI2A.Font = Me.Font
        AddHandler Me.BackColorChanged, Sub() mI2A.BackColor = Me.BackColor
        AddHandler mI2A.ImageProcessed, Sub() Me.Invalidate()

        Me.BackColor = Color.Black
    End Sub

    Public ReadOnly Property I2A As Image2Ascii
        Get
            Return mI2A
        End Get
    End Property

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        MyBase.OnPaintBackground(e)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If mI2A.Bitmap Is Nothing OrElse mI2A.Surface Is Nothing Then Exit Sub

        e.Graphics.DrawImageUnscaled(mI2A.Surface, 0, 0)
    End Sub
End Class
