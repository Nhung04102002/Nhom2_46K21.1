using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QLCH_Nhom2.Pages.NhapHang;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.NCC
{
    public class NCCModel : PageModel
    {
        public List<NCCInfo> listNCC = new List<NCCInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM NCC";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                NCCInfo NCC = new NCCInfo();
                                NCC.MaNCC = reader.GetString(0);
                                NCC.TenNCC = reader.GetString(1);
                                NCC.DiaChi = reader.GetString(2);
                                NCC.SDT = reader.GetString(3);

                                listNCC.Add(NCC);
                            }

                        }
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class NCCInfo
    {
        public string? MaNCC;
        public string? TenNCC;
        public string DiaChi;
        public string? SDT;
    }
}
