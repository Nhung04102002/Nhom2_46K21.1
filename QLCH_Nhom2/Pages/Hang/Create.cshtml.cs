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
        }

        public void OnPost()
        {
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

            //lưu ncc mới vào database
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var Hang = new List<string>() { hangInfo.TenHang, ""+hangInfo.SoLuong, ""+hangInfo.GiaNhap, ""+hangInfo.GiaBan };
                    String sql = "exec pInsHang N'" + Hang[0] + "', '" + Hang[1] + "', '" + Hang[2] + "', '" + Hang[3] + "'";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
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
