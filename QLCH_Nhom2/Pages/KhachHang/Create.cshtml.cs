using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QLCH_Nhom2.Pages.Hang;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.KhachHang
{
    public class CreateModel : PageModel
    {
        public KHInfo khInfo = new KHInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select dbo.fNewMaKH()";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                khInfo.MaKH = reader.GetString(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            khInfo.MaKH = Request.Form["MaKH"];
            khInfo.TenKH = Request.Form["TenKH"];
            khInfo.SDT = Request.Form["SDT"];
            khInfo.DiaChi = Request.Form["DiaChi"];

            if (khInfo.TenKH.Length == 0 || khInfo.SDT.Length == 0)
            {
                errorMessage = "Tên và số điện thoại không được để trống";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql1 = "insert into KHACHHANG values (@MaKH, @TenKH, @SDT, @DiaChi)";

                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        command.Parameters.AddWithValue("@MaKH", khInfo.MaKH);
                        command.Parameters.AddWithValue("@TenKH", khInfo.TenKH);
                        command.Parameters.AddWithValue("@SDT", khInfo.SDT);
                        command.Parameters.AddWithValue("@DiaChi", khInfo.DiaChi);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            khInfo.TenKH = ""; khInfo.SDT = ""; khInfo.DiaChi = "";
            successMessage = "Thêm mới Khách hàng thành công!";

            Response.Redirect("/KhachHang/KhachHang");
        }
    }
}