using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Newtonsoft.Json;


namespace FSAPortal.Controllers
{
    public class Gantt : Controller
    {
        string connectionString = "server=185.132.198.192;" +
                "Port=3306;user id=Ivanov;" +
                "Password=5827757;" +
                "persistsecurityinfo=True;" +
                "database=newschema;" +
                "CharSet=utf8;" +
                "SslMode=none";

        [HttpGet]
        public IActionResult GanttGetData()
        {
            List<Dictionary<string, object>> ganttData = new List<Dictionary<string, object>>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("SELECT * FROM gantt", connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> task = new Dictionary<string, object>();
                            task.Add("id", reader.GetString("idgantt"));
                            task.Add("text", reader.GetString("text"));
                            task.Add("number_proc", reader.GetString("number_proc"));
                            task.Add("oplata_plo", reader.GetString("oplata_plo"));
                            task.Add("nmck", reader.GetString("nmck"));
                            task.Add("date_pret", reader.GetString("date_pret"));
                            task.Add("result", reader.GetString("result"));
                            task.Add("price", reader.GetString("price"));
                            task.Add("itog", reader.GetString("itog"));
                            task.Add("start_date", reader.GetString("start_date"));
                            task.Add("end_date", reader.GetString("end_date"));
                            task.Add("progress", reader.GetString("progress"));
                            task.Add("open", reader.GetString("open"));
                            task.Add("name_concurs", reader.GetString("name_concurs"));
                            task.Add("link", reader.GetString("link"));
                            task.Add("comment", reader.GetString("comment"));
                            task.Add("parent", reader.GetString("parent"));
                            task.Add("endtip", reader.GetString("endtip"));

                            ganttData.Add(task);
                        }
                    }
                }
            }

            string json = JsonConvert.SerializeObject(ganttData);
            return Json(json);
        }

        [HttpPost]
        public IActionResult SaveGanttData(string id, string text, string number_proc, string oplata_plo, string nmck, string date_pret, string result, string price, string itog, string start_date, string end_date, string progress, string open, string name_concurs, string link, string comment, string parent, string endtip)
        {
            start_date = $"{start_date.Substring(8, 2)}-{start_date.Substring(4, 3)}-{start_date.Substring(11, 4)} {start_date.Substring(16, 8)}";
            string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            foreach (string month in months)
            {
                start_date = start_date.Replace(month, (Array.IndexOf(months, month) + 1).ToString("D2"));
            }

            end_date = $"{end_date.Substring(8, 2)}-{end_date.Substring(4, 3)}-{end_date.Substring(11, 4)} {end_date.Substring(16, 8)}";
            foreach (string month in months)
            {
                end_date = end_date.Replace(month, (Array.IndexOf(months, month) + 1).ToString("D2"));
            }
            
            string query = string.Format("SELECT id FROM gantt WHERE idgantt = {0}", id);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    object existingId = command.ExecuteScalar();
                    if (existingId!= null)
                    {
                        // Если id уже существует, обновляем запись
                        command.CommandText = "UPDATE gantt SET text = '" + text + "', number_proc = '" + number_proc + "', oplata_plo = '" + oplata_plo + "', nmck = '" + nmck + "', date_pret = '" + date_pret + "', result = '" + result + "', price = '" + price + "', itog = '" + itog + "', start_date = '" + start_date + "', end_date = '" + end_date + "', progress = '" + progress + "', open = '" + open + "', name_concurs = '" + name_concurs + "', link = '" + link + "', comment = '" + comment + "', parent = '" + parent + "', endtip = '" + endtip + "' WHERE idgantt = '" + id + "'";
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        // Если id не существует, добавляем новую запись
                        command.CommandText = "INSERT INTO gantt (idgantt, text, number_proc, oplata_plo, nmck, date_pret, result, price, itog, start_date, end_date, progress, open, name_concurs, link, comment, parent, endtip) VALUES ('" + id + "', '" + text + "', '" + number_proc + "', '" + oplata_plo + "', '" + nmck + "', '" + date_pret + "', '" + result + "', '" + price + "', '" + itog + "', '" + start_date + "', '" + end_date + "', '" + progress + "', '" + open + "', '" + name_concurs + "', '" + link + "', '" + comment + "', '" + parent + "', '" + endtip + "')";
                        command.ExecuteNonQuery();
                    }

                }
            }
            var json = text;
            return Json(json);
        }

        [HttpGet]
        public IActionResult GanttGeleteData(string id)
        {
            List<Dictionary<string, object>> ganttData = new List<Dictionary<string, object>>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("DELETE FROM gantt WHERE idgantt = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }

            }
            return Json(new { success = true });
        }

    }
}