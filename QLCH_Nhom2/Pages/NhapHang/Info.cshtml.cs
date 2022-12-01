using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.NhapHang
{
    public class InfoModel : PageModel
    {
        public CTNKInfo ctnkInfo = new CTNKInfo();
        public List<CtnkInfo> listCTnk = new List<CtnkInfo>();
        public void OnGet()
        {
            string MaNK = Request.Query["MaNK"];
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select MaNK, NgayNhap, NCC.MaNCC, TenNCC, TongTien, ChietKhau, ThanhToan from PhieuNK join NCC on PhieuNK.MaNCC = NCC.MaNCC where MaNK=@MaNK";
                    String sql1 = "select MaNK, HANG.MaHang, TenHang, CT_PhieuNK.SoLuong, GiaNhap, ThanhTien from CT_PhieuNK join HANG on CT_PhieuNK.MaHang = HANG.MaHang where MaNK=@MaNK";
                    String sql2 = "select count(MaNK), sum(SoLuong) from CT_PhieuNK where MaNK=@MaNK";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaNK", MaNK);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ctnkInfo.MaNK = reader.GetString(0);
                                ctnkInfo.NgayNhap = reader.GetDateTime(1);
                                ctnkInfo.MaNCC = reader.GetString(2);
                                ctnkInfo.TenNCC = reader.GetString(3);
                                ctnkInfo.TongTien = reader.GetDecimal(4);
                                ctnkInfo.ChietKhau = reader.GetDouble(5);
                                ctnkInfo.ThanhToan = reader.GetDecimal(6);
                            }
                        }
                    }
                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        command.Parameters.AddWithValue("@MaNK", MaNK);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CtnkInfo CTnk = new CtnkInfo();
                                CTnk.MaHang = reader.GetString(1);
                                CTnk.TenHang = reader.GetString(2);
                                CTnk.SoLuong = reader.GetInt32(3);
                                CTnk.GiaNhap = reader.GetDecimal(4);
                                CTnk.ThanhTien = reader.GetDecimal(5);

                                listCTnk.Add(CTnk);
                            }

                        }

                    }
                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        command.Parameters.AddWithValue("@MaNK", MaNK);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ctnkInfo.TongHang = reader.GetInt32(0);
                                ctnkInfo.TongSL = reader.GetInt32(1);
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

    public class CTNKInfo
    {
        public string? MaNK;
        public DateTime NgayNhap;
        public string? MaNCC;
        public string? TenNCC;
        public decimal TongTien;
        public double ChietKhau;
        public decimal ThanhToan;
        public int TongSL;
        public int TongHang;
    }

    public class CtnkInfo
    {
        public string? MaHang;
        public string? TenHang;
        public int SoLuong;
        public decimal GiaNhap;
        public decimal ThanhTien;
    }
}