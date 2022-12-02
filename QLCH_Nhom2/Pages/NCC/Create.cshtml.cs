using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.NCC
{
    public class CreateModel : PageModel
    {
        public NCCInfo nccInfo = new NCCInfo();
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
                    String sql = "select dbo.fNewMaNCC()";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                nccInfo.MaNCC = reader.GetString(0);
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
            nccInfo.TenNCC = Request.Form["MaNCC"];
            nccInfo.TenNCC = Request.Form["TenNCC"];
            nccInfo.DiaChi = Request.Form["DiaChi"];
            nccInfo.SDT = Request.Form["SDT"];

            if (nccInfo.TenNCC.Length == 0 || nccInfo.DiaChi.Length == 0 ||
                nccInfo.SDT.Length == 0)
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
                    String sql1 = "insert into NCC values(@MaNCC, @TenNCC, @SDT, @DiaChi)";

                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        command.Parameters.AddWithValue("@MaNCC", nccInfo.MaNCC);
                        command.Parameters.AddWithValue("@TenNCC", nccInfo.TenNCC);
                        command.Parameters.AddWithValue("@DiaChi", nccInfo.DiaChi);
                        command.Parameters.AddWithValue("@SDT", nccInfo.SDT);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) 
            {
                errorMessage= ex.Message;
                return;
            }

            nccInfo.TenNCC = ""; nccInfo.DiaChi = ""; nccInfo.SDT = "";
            successMessage = "Thêm mới nhà cung cấp thành công!";

            Response.Redirect("/NCC/NCC");
        }
    }
}
