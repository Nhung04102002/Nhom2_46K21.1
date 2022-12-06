using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QLCH_Nhom2.Pages.Hang;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.NhapHang
{
    public class CreateModel : PageModel
    {
        public NKInfo NK = new NKInfo();

        public ctnkInfo CTNK = new ctnkInfo();
        public List<ctnkInfo> list = new List<ctnkInfo>();
        public List<ctnkInfo> listCTNK = new List<ctnkInfo>();
        public List<HangInfo> listHang = new List<HangInfo>();
        public HangInfo searchInfo = new HangInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            searchInfo.Search = Request.Query["Search"];
            string MaH = Request.Query["MaHang"];
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var search = new List<string>() { searchInfo.Search };
                    String sql = "select max(MaNK) from PhieuNK";
                    String sql1 = "select MaHang, TenHang, SoLuong, GiaNhap from HANG where (MaHang like '%" + search[0] + "%' or TenHang like '%" + search[0] + "%') and SoLuong>=0";
                    String sql4 = "select * from PhieuNK where MaNK=@MaNK";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                CTNK.MaNK = reader.GetString(0);
                                
                            }
                        }
                    }
                    String sql3 = "select MaNK, HANG.MaHang, TenHang, CT_PhieuNK.SoLuong, GiaNhap, ThanhTien from CT_PhieuNK join HANG on CT_PhieuNK.MaHang = HANG.MaHang where MaNK=@MaNK";
                    using (SqlCommand command = new SqlCommand(sql3, connection))
                    {
                        command.Parameters.AddWithValue("@MaNK", CTNK.MaNK);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ctnkInfo ctnk = new ctnkInfo();
                                ctnk.MaNK = reader.GetString(0);
                                ctnk.MaHang = reader.GetString(1);
                                ctnk.TenHang = reader.GetString(2);
                                ctnk.SoLuong = reader.GetInt32(3);
                                ctnk.GiaNhap = reader.GetDecimal(4);
                                ctnk.ThanhTien = reader.GetDecimal(5);

                                listCTNK.Add(ctnk);
                            }
                        }
                    }
                    using (SqlCommand command = new SqlCommand(sql4, connection))
                    {
                        command.Parameters.AddWithValue("@MaNK", CTNK.MaNK);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                NK.MaNK = reader.GetString(0);
                                NK.MaNCC = reader.GetString(1);
                                NK.NgayNhap = reader.GetDateTime(2);
                                NK.TongTien = reader.GetDecimal(3);
                                NK.ChietKhau = reader.GetDouble(4);
                                NK.ThanhToan= reader.GetDecimal(5);
                            }
                        }
                    }
                    
                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        command.Parameters.AddWithValue("@Search", searchInfo.Search);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                HangInfo Hang = new HangInfo();
                                Hang.MaHang = reader.GetString(0);
                                Hang.TenHang = reader.GetString(1);
                                Hang.SoLuong = reader.GetInt32(2);
                                Hang.GiaNhap = reader.GetDecimal(3);

                                listHang.Add(Hang);
                            }
                        }
                    }
                    

                }

            }
            catch (Exception)
            {
            }
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql2 = "select MaHang, TenHang, GiaNhap from HANG where MaHang=@MaHang";
                    
                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        command.Parameters.AddWithValue("@MaHang", MaH);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CTNK.MaHang = reader.GetString(0);
                                CTNK.TenHang = reader.GetString(1);
                                CTNK.SoLuong = 1;
                                CTNK.GiaNhap = reader.GetDecimal(2);
                                CTNK.ThanhTien = CTNK.GiaNhap * CTNK.SoLuong;
                                list.Add(CTNK);
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {

            }
            
        }
        public void OnPost() 
        {
            CTNK.MaNK = Request.Form["MaNK"];
            CTNK.MaHang = Request.Form["MaHang"];
            CTNK.TenHang = Request.Form["TenHang"];
            CTNK.GiaNhap = Convert.ToDecimal(Request.Form["GiaNhap"]);
            CTNK.SoLuong = Convert.ToInt32(Request.Form["SoLuong"]);
            CTNK.ThanhTien = Convert.ToInt32(Request.Form["SoLuong"]) * Convert.ToDecimal(Request.Form["GiaNhap"]);

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql1 = "insert into CT_PhieuNK values (@MaNK, @MaHang, @SoLuong, @ThanhTien)";

                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        command.Parameters.AddWithValue("@MaNK", CTNK.MaNK);
                        command.Parameters.AddWithValue("@MaHang", CTNK.MaHang);
                        command.Parameters.AddWithValue("@SoLuong", CTNK.SoLuong);
                        command.Parameters.AddWithValue("@ThanhTien", CTNK.ThanhTien);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            Response.Redirect("/NhapHang/Create");
        }
    }
    public class ctnkInfo
    {
        public string MaNK;
        public string MaHang;
        public string TenHang;
        public int SoLuong;
        public decimal GiaNhap;
        public decimal ThanhTien;

    }
}
