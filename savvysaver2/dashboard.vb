Public Class dashboard
    Private Sub Guna2GradientButton1_Click_1(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Vendor.Show()
        Hide()

    End Sub

    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        Product.Show()
        Hide()
    End Sub

    Private Sub Guna2GradientButton3_Click_1(sender As Object, e As EventArgs) Handles Guna2GradientButton3.Click
        Report.Show()
        Hide()
    End Sub

    Private Sub Guna2GradientButton4_Click_1(sender As Object, e As EventArgs) Handles Guna2GradientButton4.Click
        Application.Exit()
    End Sub
End Class