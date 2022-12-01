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
        }

        public void OnPost() 
        {
            nccInfo.TenNCC = Request.Form["TenNCC"];
            nccInfo.DiaChi = Request.Form["DiaChi"];
            nccInfo.SDT = Request.Form["SDT"];

            if (nccInfo.TenNCC.Length == 0 || nccInfo.DiaChi.Length == 0 ||
                nccInfo.SDT.Length == 0)
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
                    var Ncc = new List<string>() { nccInfo.TenNCC, nccInfo.DiaChi,nccInfo.SDT};
                    String sql = "exec pInsNCC N'" + Ncc[0] + "', N'" + Ncc[1] + "', '" + Ncc[2] + "'";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
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
