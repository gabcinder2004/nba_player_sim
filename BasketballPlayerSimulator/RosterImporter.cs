using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Excel;

namespace BasketballPlayerSimulator
{
    public static class RosterImporter
    {
        private static Application appExcel;
        private static Workbook newWorkbook;
        private static _Worksheet objsheet;

        public static List<Player> ImportPlayers()
        {
            var row = 2;
            var listOfPlayers = new List<Player>();
            OpenExcel("C:\\BasketballSimulator\\Players.xlsx");

            while (!GetValue(string.Format("A{0}", row)).Equals(string.Empty))
            {
                var playerName = GetValue(string.Format("A{0}", row));
                var position = DeterminePosition(GetValue(string.Format("B{0}", row)).Replace(" ", ""));
                var team = DetermineTeamName(GetValue(string.Format("C{0}", row)));
                var playerNumber = GetValue(string.Format("D{0}", row));
                var overallRating = Convert.ToInt32(Convert.ToDouble(GetValue(string.Format("E{0}", row))));
                var offenseRating = Convert.ToInt32(Convert.ToDouble(GetValue(string.Format("F{0}", row))));
                var defenseRating = Convert.ToInt32(Convert.ToDouble(GetValue(string.Format("G{0}", row))));
                var closeRating = Convert.ToInt32(Convert.ToDouble(GetValue(string.Format("H{0}", row))));
                var mediumRating = Convert.ToInt32(Convert.ToDouble(GetValue(string.Format("I{0}", row))));
                var longRating = Convert.ToInt32(Convert.ToDouble(GetValue(string.Format("J{0}", row))));
                var rating = new Rating(overallRating, offenseRating, defenseRating, closeRating, mediumRating, longRating);
                listOfPlayers.Add(new Player(playerName, playerNumber, position, team, rating));
                row++;
            }

            excel_close();
            return listOfPlayers;
        }


        private static void OpenExcel(string path)
        {
            appExcel = new Application();

            if (File.Exists(path))
            {
                try
                {
                    // then go and load this into excel
                    newWorkbook = appExcel.Workbooks.Open(path, true, true);
                    objsheet = (_Worksheet) appExcel.ActiveWorkbook.ActiveSheet;
                }
                catch (Exception e)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(appExcel);
                }
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

        private static TeamName DetermineTeamName(string team)
        {
            var listOfTeamNames = Enum.GetValues(typeof(TeamName)).OfType<TeamName>().ToList();
            return listOfTeamNames.Single(x => x.ToString().Contains(team));
        }

        private static Position DeterminePosition(string position)
        {
            var listOfPositions = Enum.GetValues(typeof(Position)).OfType<Position>().ToList();
            return listOfPositions.Single(x => x.ToString().Contains(position));
        }
    }
}
