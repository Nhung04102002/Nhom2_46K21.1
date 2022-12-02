using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.Hang
{
    public class CreateModel : PageModel
    {
        public HangInfo hangInfo = new HangInfo();
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
                    String sql = "select dbo.fNewMaHang()";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                hangInfo.MaHang = reader.GetString(0);
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
            hangInfo.MaHang = Request.Form["MaHang"];
            hangInfo.TenHang = Request.Form["TenHang"];
            hangInfo.SoLuong = Convert.ToInt32(Request.Form["SoLuong"]);
            hangInfo.GiaNhap = Convert.ToDecimal(Request.Form["GiaNhap"]);
            hangInfo.GiaBan = Convert.ToDecimal(Request.Form["GiaBan"]);

            if (hangInfo.TenHang.Length == 0 || hangInfo.SoLuong == 0 ||
                hangInfo.GiaNhap == 0 || hangInfo.GiaBan == 0)
            {
                errorMessage = "Thông tin không được để trống";
                return;
            }

            if (hangInfo.SoLuong < 0 || hangInfo.GiaNhap > hangInfo.GiaBan ||
                hangInfo.GiaNhap < 0 || hangInfo.GiaBan < 0)
            {
                errorMessage = "Thông tin không hợp lệ";
                return;
            }    

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    String sql1 = "insert into HANG values (@MaHang, @TenHang, @SoLuong,@GiaNhap, @GiaBan)";
                    
                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        command.Parameters.AddWithValue("@MaHang", hangInfo.MaHang);
                        command.Parameters.AddWithValue("@TenHang", hangInfo.TenHang);
                        command.Parameters.AddWithValue("@SoLuong", hangInfo.SoLuong);
                        command.Parameters.AddWithValue("@GiaNhap", hangInfo.GiaNhap);
                        command.Parameters.AddWithValue("@GiaBan", hangInfo.GiaBan);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            hangInfo.TenHang = ""; hangInfo.SoLuong = 0; hangInfo.GiaNhap = 0; hangInfo.GiaBan = 0;
            successMessage = "Thêm mới hàng hoá thành công!";

            Response.Redirect("/Hang/Hang");
        }
    }
}
