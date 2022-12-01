using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.NCC
{
    public class EditModel : PageModel
    {
        public NCCInfo nccInfo = new NCCInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            string MaNCC = Request.Query["MaNCC"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select * from NCC where MaNCC=@MaNCC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaNCC", MaNCC);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                nccInfo.MaNCC = reader.GetString(0);
                                nccInfo.TenNCC = reader.GetString(1);
                                nccInfo.DiaChi = reader.GetString(2);
                                nccInfo.SDT = reader.GetString(3);

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
            nccInfo.MaNCC = Request.Form["MaNCC"];
            nccInfo.TenNCC = Request.Form["TenNCC"];
            nccInfo.DiaChi = Request.Form["DiaChi"];
            nccInfo.SDT = Request.Form["SDT"];

            if (nccInfo.TenNCC.Length == 0 || nccInfo.DiaChi.Length == 0 ||
                nccInfo.SDT.Length == 0 || nccInfo.MaNCC.Length == 0)
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
                    String sql = "update NCC " + "set TenNCC=@TenNCC, DiaChi=@DiaChi, SDT=@SDT where MaNCC=@MaNCC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
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
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/NCC/NCC");
        }

        
    }
}
