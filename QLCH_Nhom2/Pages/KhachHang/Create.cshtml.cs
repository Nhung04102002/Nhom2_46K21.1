using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        }

        public void OnPost()
        {
            khInfo.TenKH = Request.Form["TenKH"];
            khInfo.SDT = Request.Form["SDT"];
            khInfo.DiaChi = Request.Form["DiaChi"];

            if (khInfo.TenKH.Length == 0 || khInfo.SDT.Length == 0)
            {
                errorMessage = "Tên và số điện thoại không được để trống";
                return;
            }

            //lưu KH mới vào database
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var Kh = new List<string>() { khInfo.TenKH, khInfo.SDT, khInfo.DiaChi };
                    String sql = "exec pInsKH N'" + Kh[0] + "', '" + Kh[1] + "', N'" + Kh[2] + "'";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
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