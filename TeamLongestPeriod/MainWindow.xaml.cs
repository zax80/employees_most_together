using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace AssignmentTeamLongestPeriod
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var format = System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                List<DocumentRecord> recordsList = ReadDocument(openFileDialog.FileName); //Get all records

                //Check where have overlapping
                List<DocumentRecord[]> overlappingEvents =
                (
                    from e1 in recordsList
                    from e2 in recordsList
                    where e1 != e2
                    where e1.ProjectID == e2.ProjectID
                    where e1.DateFrom <= e2.DateTo
                    where e1.DateTo >= e2.DateFrom
                    select new[] { e1, e2 }
                ).Distinct().ToList();
                //

                List<OverlapCase> overlapCases = ProcessCases(overlappingEvents); //Get overlap cases ordered by days worked together
                gridEmployeesOverlap.ItemsSource = overlapCases; //Populate grid
                OverlapCase result = overlapCases[0];

                //Display messagebox with the result
                MessageBox.Show("Employee1ID:" + result.Employee1ID + " | Employee2ID:" + result.Employee2ID + " | ProjectID:" + result.ProjectID + " | Days on:" + result.DaysOn, "Result");
            }
        }

        private List<OverlapCase> ProcessCases(List<DocumentRecord[]> overlappingEvents)
        {
            List<OverlapCase> res = new List<OverlapCase>();
            foreach (DocumentRecord[] li in overlappingEvents)
            {
                OverlapCase loopItem = new OverlapCase(li[0], li[1]);
                res.Add(loopItem);
            }
            res = res.OrderByDescending(a => a.DaysOn).ToList();
            return res;
        }

        private List<DocumentRecord> ReadDocument(string fileName)
        {
            var format = System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat;
            List<DocumentRecord> arrayResult = new List<DocumentRecord>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                while (!sr.EndOfStream)
                {
                    string fullText = sr.ReadToEnd().ToString(); //read full file text
                    string[] rows = fullText.Split('\n'); //split full file text into rows
                    int size = rows.Count() - 1;

                    for (int i = 0; i < size; i++)
                    {
                        string[] rowValues = rows[i].Split(','); //split each row with comma to get individual values
                        {
                            if (i > 0)
                            {
                                int employID = Int32.Parse(rowValues[0]);
                                int projectID = Int32.Parse(rowValues[1]);
                                DateTime dtFrom = (rowValues[2].Trim() == "NULL") ? DateTime.Now : DateTime.Parse(rowValues[2].Trim(), format);
                                DateTime dtTo = (rowValues[3].Trim() == "NULL") ? DateTime.Now : DateTime.Parse(rowValues[3].Trim(), format);
                                DocumentRecord docRecord = new DocumentRecord(employID, projectID, dtFrom, dtTo);
                                arrayResult.Add(docRecord);
                            }
                        }
                    }
                }
            }
            return arrayResult;
        }
    }
}