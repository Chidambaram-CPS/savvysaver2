Imports System.Data.SqlClient
Imports Microsoft.Data.SqlClient

Public Class Vendor

    Dim con As New SqlConnection("Data Source=CHIDAMBARAM-LAP\SQLEXPRESS01;Initial Catalog=Comparison;Integrated Security=True;TrustServerCertificate=True")

    ' Save New Vendor (Button 1)
    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Try
            If con.State = ConnectionState.Closed Then con.Open()

            Dim query As String = "INSERT INTO Vendor (Vendor_Name, Contact_Number, Email, Address) VALUES (@Name, @Contact, @Email, @Address)"
            Dim cmd As New SqlCommand(query, con)

            cmd.Parameters.AddWithValue("@Name", Guna2TextBox3.Text)
            cmd.Parameters.AddWithValue("@Contact", Guna2TextBox1.Text)
            cmd.Parameters.AddWithValue("@Email", Guna2TextBox4.Text)
            cmd.Parameters.AddWithValue("@Address", Guna2TextBox2.Text)

            Dim result As Integer = cmd.ExecuteNonQuery()

            If result > 0 Then
                MessageBox.Show("Vendor Added Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadVendorData()
            Else
                MessageBox.Show("Insertion failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            con.Close()
        End Try
    End Sub

    ' Load Vendor Data (Button 2)
    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        LoadVendorData()
    End Sub

    Private Sub LoadVendorData()
        Try
            If con.State = ConnectionState.Closed Then con.Open()

            Dim adapter As New SqlDataAdapter("SELECT * FROM Vendor", con)
            Dim dt As New DataTable()
            adapter.Fill(dt)
            Guna2DataGridView1.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            con.Close()
        End Try
    End Sub

    ' Save Changes Made in DataGridView (Button 3)
    Private Sub Guna2GradientButton3_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton3.Click
        Try
            If con.State = ConnectionState.Closed Then con.Open()

            Dim adapter As New SqlDataAdapter("SELECT * FROM Vendor", con)
            Dim builder As New SqlCommandBuilder(adapter)

            Dim dt As DataTable = CType(Guna2DataGridView1.DataSource, DataTable)

            adapter.Update(dt)

            MessageBox.Show("Changes saved to database successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error saving changes: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            con.Close()
        End Try
    End Sub
End Class
