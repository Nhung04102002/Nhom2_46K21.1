using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QLCH_Nhom2.Pages.Hang;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.HoaDon
{
    public class CreateModel : PageModel
    {
        public HDInfo hdInfo = new HDInfo();
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
                    String sql = "select dbo.fNewMaHD()";
                    String sql1 = "select GetDate()";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                hdInfo.MaHD = reader.GetString(0);
                            }
                        }
                    }
                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                hdInfo.NgayBan = reader.GetDateTime(0);
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
            hdInfo.MaHD = Request.Form["MaHD"];
            hdInfo.MaKH = Request.Form["MaKH"];
            hdInfo.NgayBan = Convert.ToDateTime(Request.Form["NgayBan"]);
            hdInfo.TongTien = Convert.ToInt32(Request.Form["TongTien"]);
            hdInfo.ChietKhau = Convert.ToDouble(Request.Form["ChietKhau"]);
            hdInfo.ThanhToan = Convert.ToDecimal(Request.Form["ThanhToan"]);
            if (hdInfo.MaKH.Length == 0)
            {
                hdInfo.MaKH = "KH000000";
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql1 = "insert into HOADON values (@MaHD, @MaKH, @NgayBan, @TongTien, @ChietKhau, @ThanhToan)";

                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        command.Parameters.AddWithValue("@MaHD", hdInfo.MaHD);
                        command.Parameters.AddWithValue("@MaKH", hdInfo.MaKH);
                        command.Parameters.AddWithValue("@NgayBan", hdInfo.NgayBan);
                        command.Parameters.AddWithValue("@TongTien", hdInfo.TongTien);
                        command.Parameters.AddWithValue("@ChietKhau", hdInfo.ChietKhau);
                        command.Parameters.AddWithValue("@ThanhToan", hdInfo.ThanhToan);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            Response.Redirect("/HoaDon/CTHD");
            hdInfo.MaHD = ""; hdInfo.MaKH = ""; hdInfo.ChietKhau = 0;
            successMessage = "Thêm mới hoá đơn thành công!";
        }
    }
}
