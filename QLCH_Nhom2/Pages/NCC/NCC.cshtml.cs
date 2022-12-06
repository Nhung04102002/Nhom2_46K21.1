using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
                    string sql = "SELECT * FROM NCC where DiaChi <>'1'";
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
        public NCCInfo searchInfo = new NCCInfo();
        public void OnPost()
        {
            searchInfo.Search = Request.Form["Search"];
            if (searchInfo.Search.Length == 0)
            {
                Response.Redirect("/NCC/NCC");
            }
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var search = new List<string>() { searchInfo.Search };
                    String sql1 = "select * from NCC where (TenNCC like '%" + search[0] + "%' or MaNCC like '%" + search[0] + "%') and DiaChi <>'1'";

                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        command.Parameters.AddWithValue("@Search", searchInfo.Search);

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

            }
            catch (Exception)
            {
            }
        }
    }

    public class NCCInfo
    {
        public string Search;
        public string? MaNCC;
        public string? TenNCC;
        public string DiaChi;
        public string? SDT;
    }
}
