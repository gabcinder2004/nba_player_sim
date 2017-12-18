using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Excel;

namespace BasketballPlayerSimulator
{
    public static class ScheduleImporter
    {
        private static Application appExcel;
        private static Workbook newWorkbook;
        private static _Worksheet objsheet;
        private static NBA NBA;

        public static Schedule ImportSchedule(NBA nba)
        {
            NBA = nba;
            var row = 2;
            var listOfGames = new List<Game>();
            OpenExcel("C:\\BasketballSimulator\\Schedule.xlsx");
            while (!GetValue(string.Format("A{0}", row)).Equals(string.Empty))
            {
                var date = GetValue(string.Format("A{0}", row));
                var awayTeam = DetermineTeamName(GetValue(string.Format("B{0}", row)));
                var homeTeam = DetermineTeamName(GetValue(string.Format("C{0}", row)));
                var realDate = ParseDate(date);
                row++;
                listOfGames.Add(new Game(homeTeam, awayTeam, realDate));
            }
            excel_close();
            return new Schedule(listOfGames);
        }
            
        private static DateTime ParseDate(string date)
        {
            var parts = date.Split('.')[1].Trim().Split(' ');
            var month = parts[0];
            var day = Convert.ToInt32(parts[1].Replace(',', ' ').Trim());
            var year = Convert.ToInt32(parts[2]);
            int intMonth;

            switch (month)
            {
                case "October":
                    intMonth = 10;
                    break;
                case "November":
                    intMonth = 11;
                    break;
                case "December":
                    intMonth = 12;
                    break;
                case "January":
                    intMonth = 1;
                    break;
                case "February":
                    intMonth = 2;
                    break;
                case "March":
                    intMonth = 3;
                    break;
                case "April":
                    intMonth = 4;
                    break;
                default:
                    intMonth = 5;
                    break;
            }
            return new DateTime(year, intMonth, day);
        }

        private static void OpenExcel(string path)
        {
            appExcel = new Application();

            if (File.Exists(path))
            {
                // then go and load this into excel
                newWorkbook = appExcel.Workbooks.Open(path, true, true);
                objsheet = (_Worksheet)appExcel.ActiveWorkbook.ActiveSheet;
            }
            else
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(appExcel);
                appExcel = null;
                throw new Exception("Unable to open file!");
            }
        }

        private static string GetValue(string cellname)
        {
            string value;
            try
            {
                value = objsheet.Range[cellname].get_Value().ToString();
            }
            catch
            {
                value = "";
            }

            return value;
        }

        private static void excel_close()
        {
            if (appExcel == null) return;

            try
            {
                newWorkbook.Close();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(appExcel);
                appExcel = null;
                objsheet = null;
            }
            catch (Exception ex)
            {
                appExcel = null;
                throw new Exception("Unable to release the Object " + ex);
            }
            finally
            {
                GC.Collect();
            }
        }

        private static Team DetermineTeamName(string team)
        {
            team = team.Replace("*", "");
            var listOfTeamNames = Enum.GetValues(typeof (TeamName)).OfType<TeamName>().ToList();
            var teamName =  listOfTeamNames.Single(x => x.ToString().Contains(team));
            return NBA.Teams.Single(t => t.Name == teamName);
        }
    }
}
