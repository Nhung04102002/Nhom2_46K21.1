using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QLCH_Nhom2.Pages.Hang;
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
        public NKInfo searchInfo = new NKInfo();
        public void OnPost()
        {
            searchInfo.Search = Request.Form["Search"];
            if (searchInfo.Search.Length == 0)
            {
                Response.Redirect("/NhapHang/NhapHang");
            }
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var search = new List<string>() { searchInfo.Search };
                    String sql1 = "select MaNK, TenNCC, NgayNhap, ThanhToan from PhieuNK join NCC on PhieuNK.MaNCC = NCC.MaNCC where MaNK like '%" + search[0] + "%' or TenNCC like '%" + search[0] + "%'";

                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        command.Parameters.AddWithValue("@Search", searchInfo.Search);

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
            catch (Exception)
            {
            }
        }
    }

    public class NKInfo
    {
        public string Search;
        public string? MaNK;
        public string? TenNCC;
        public DateTime NgayNhap;
        public decimal ThanhToan;
        public string MaNCC;
        public double ChietKhau;
        public decimal TongTien;
        public int result;
    }
}
