using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.NhapHang
{
    public class NhapHangModel : PageModel
    {
        public List<NKInfo> listNK = new List<NKInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "select MaNK, TenNCC, NgayNhap, ThanhToan from PhieuNK join NCC on PhieuNK.MaNCC = NCC.MaNCC";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                NKInfo NK = new NKInfo();
                                NK.MaNK = reader.GetString(0);
                                NK.TenNCC = reader.GetString(1);
                                NK.NgayNhap = reader.GetDateTime(2);
                                NK.ThanhToan = reader.GetDecimal(3);

                                listNK.Add(NK);
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class NKInfo
    {
        public string? MaNK;
        public string? TenNCC;
        public DateTime NgayNhap;
        public decimal ThanhToan;
    }
}
