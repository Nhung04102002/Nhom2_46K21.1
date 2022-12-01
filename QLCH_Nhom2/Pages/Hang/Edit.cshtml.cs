using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.Hang
{
    public class EditModel : PageModel
    {
        public HangInfo hangInfo = new HangInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            string MaHang = Request.Query["MaHang"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select * from HANG where MaHang=@MaHang";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaHang", MaHang);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                hangInfo.MaHang = reader.GetString(0);
                                hangInfo.TenHang = reader.GetString(1);
                                hangInfo.SoLuong = reader.GetInt32(2);
                                hangInfo.GiaNhap = reader.GetDecimal(3);
                                hangInfo.GiaBan = reader.GetDecimal(4);

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
                hangInfo.GiaNhap == 0 || hangInfo.GiaBan == 0 || hangInfo.MaHang == "")
            {
                errorMessage = "Thông tin không được để trống";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "update HANG " + "set TenHang=@TenHang, SoLuong=@SoLuong, GiaNhap=@GiaNhap, GiaBan=@GiaBan where MaHang=@MaHang";

                    using (SqlCommand command = new SqlCommand(sql, connection))
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

            Response.Redirect("/Hang/Hang");
        }


    }
}
