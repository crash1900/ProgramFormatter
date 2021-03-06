﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;

namespace ProgramFormatter
{
    public partial class frmProgramFormatter : Form
    {
        public frmProgramFormatter()
        {
            InitializeComponent();
        }

        #region Events

        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtSelectedFile.Text = dlg.FileName;
                PopulateBlocks(txtSelectedFile.Text);
                chkSaveNewFile.Enabled = txtSelectedFile.Text.Length > 0;
                btnGenerateSheet.Enabled = txtSelectedFile.Text.Length > 0;
            }
        }

        private void btnGenerateSheet_Click(object sender, EventArgs e)
        {

            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(txtSelectedFile.Text)))
            {
                string blockNumber = String.Format("Block {0}", cboBlockNumber.SelectedValue);
                
                if (package.Workbook.Worksheets[blockNumber] != null)
                {
                    if (MessageBox.Show("A sheet for this block already exists. Do you want to overwrite it?", 
                        "Existing Block Found", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
                    {
                        package.Workbook.Worksheets.Delete(blockNumber);
                    }
                    else
                    {
                        return;
                    }
                }

                var newSheet = package.Workbook.Worksheets.Add(blockNumber);
                package.Workbook.Worksheets.MoveBefore(blockNumber, "Training data");
                newSheet.Select();

                var ws = package.Workbook.Worksheets["Training data"];
                bool hasHeader = true;

                var tbl = GetTrainingData(ws, hasHeader);

                int blockNum;
                if (!int.TryParse(cboBlockNumber.SelectedValue.ToString(), out blockNum))
                    throw new Exception("Invalid block number. Not an integer.");

                var tblFiltered = FilterBlockData(tbl, blockNum);

                BuildProgram(tblFiltered, newSheet);

                if (chkSaveNewFile.Checked)
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.InitialDirectory = new FileInfo(txtSelectedFile.Text).Directory.FullName;
                    dlg.Title = "Save to New File";
                    dlg.CheckPathExists = true;
                    dlg.Filter = "Excel Files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    dlg.FilterIndex = 1;

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        package.SaveAs(new FileInfo(dlg.FileName));
                        MessageBox.Show(String.Format("Save to new file. New sheet block {0} created!", blockNum),
                            "Update Successful",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                else
                {
                    package.Save();
                    MessageBox.Show(String.Format("Overwrite existing file. New sheet block {0} created!", blockNum),
                        "Update Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        ///  Read the excel sheet that was provided in the filename and retrieve
        ///  all the blocks in there from the Training data tab. Populate the cbo with
        ///  this list.
        /// </summary>
        private void PopulateBlocks(string fileName)
        {
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(fileName)))
            {
                var sheet = package.Workbook.Worksheets[Constants.trainingDataSheet];

                var range = sheet.Cells[2, 1, sheet.Dimension.End.Row, 1];


                var blockNumberList = range.Select(r => r.Value).Distinct();
                cboBlockNumber.DataSource = blockNumberList.ToList();
            }
        }

        /// <summary>
        /// Main routine to build the program using the data in the datatable and writing
        /// to the provided workshete
        /// </summary>
        /// <param name="tbl">Table with training data</param>
        /// <param name="newSheet">Worksheet to write to</param>
        private void BuildProgram(DataTable tbl, ExcelWorksheet newSheet)
        {
            int colOffset = 8;

            int currentWeek = 0;
            int currentDay = 0;
            int currentRow = 1;

            // Iterate through the datatable and build the sheet row-by-row
            for (int i = 0; i < tbl.Rows.Count - 1; i++)
            {
                int weekNum;
                int dayNum;
                string exercise;
                double? weight = null;
                int? reps = null;
                int? sets = null;
                string notes;
                string rpe;

                weekNum = GetValueFromTable(tbl.Rows[i][Constants.weekCol].ToString());
                dayNum = GetValueFromTable(tbl.Rows[i][Constants.dayCol].ToString());
                exercise = tbl.Rows[i][Constants.exerciseCol].ToString();

                if (!String.IsNullOrEmpty(tbl.Rows[i][Constants.weightCol].ToString()))
                {
                    double tmpWeight;
                    if (double.TryParse(tbl.Rows[i][Constants.weightCol].ToString(), out tmpWeight))
                        weight = tmpWeight;
                    else
                        weight = null;
                }

                if (!String.IsNullOrEmpty(tbl.Rows[i][Constants.repCol].ToString()))
                    reps = GetValueFromTable(tbl.Rows[i][Constants.repCol].ToString());

                if (!String.IsNullOrEmpty(tbl.Rows[i][Constants.setCol].ToString()))
                    sets = GetValueFromTable(tbl.Rows[i][Constants.setCol].ToString());

                notes = tbl.Rows[i][Constants.noteCol].ToString();
                rpe = tbl.Rows[i][Constants.rpeCol].ToString();


                int dayRow = currentRow + 1;
                int col;
                if (weekNum <= 1)
                    col = 1;
                else
                    col = (colOffset * (weekNum - 1)) + 1;

                // Iterate row by row. If we find a new week, reset the row
                if (currentWeek != weekNum)
                {
                    currentRow = 1;
                    newSheet.Cells[currentRow, col].Value = string.Format("Week {0}", weekNum);
                    newSheet.Cells[currentRow, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    newSheet.Cells[currentRow, col].Style.Fill.BackgroundColor.SetColor(Color.Azure);
                    newSheet.Cells[currentRow, col].Style.Font.Bold = true;
                    currentWeek = weekNum;
                    currentRow++;
                }

                // If the day is different from the previous, reposition the row
                if (currentDay != dayNum)
                {
                    currentRow++;
                    newSheet.Cells[currentRow, col].Value = String.Format("Day {0}", dayNum);
                    newSheet.Cells[currentRow, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    newSheet.Cells[currentRow, col].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
                    newSheet.Cells[currentRow, col].Style.Font.Bold = true;
                    currentRow++;
                    currentDay = dayNum;

                    // Print headers in the next row
                    BuildHeaders(newSheet, currentRow, col);
                    currentRow++;
                }

                // Start printing out the lines
                newSheet.Cells[currentRow, col].Value = exercise;

                if (weight != null)
                    newSheet.Cells[currentRow, col + 1].Value = weight;

                if (reps != null)
                    newSheet.Cells[currentRow, col + 2].Value = reps;

                if (sets != null)
                    newSheet.Cells[currentRow, col + 3].Value = sets;

                newSheet.Cells[currentRow, col + 4].Value = notes;
                newSheet.Cells[currentRow, col + 5].Value = rpe;
                currentRow++;
            }
            newSheet.Cells[newSheet.Dimension.Address].AutoFitColumns();
        }

        /// <summary>
        /// Build headers in the worksheet starting in the provided row and column
        /// </summary>
        /// <param name="newSheet">Worksheet header to appear on</param>
        /// <param name="currentRow">Start row position</param>
        /// <param name="col">Start column position</param>
        private void BuildHeaders(ExcelWorksheet newSheet, int currentRow, int col)
        {
            newSheet.Cells[currentRow, col].Value = "Exercise";
            newSheet.Cells[currentRow, col + 1].Value = "Weight";
            newSheet.Cells[currentRow, col + 2].Value = "Reps";
            newSheet.Cells[currentRow, col + 3].Value = "Sets";
            newSheet.Cells[currentRow, col + 4].Value = "Notes";
            newSheet.Cells[currentRow, col + 5].Value = "RPE";

            for (int i = 0; i <= 5; i++)
            {
                newSheet.Cells[currentRow, col + i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                newSheet.Cells[currentRow, col + i].Style.Fill.BackgroundColor.SetColor(Color.CornflowerBlue);
                newSheet.Cells[currentRow, col + i].Style.Font.Bold = true;
            }
        }

        /// <summary>
        /// Transforms string value into int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int GetValueFromTable(string value)
        {
            int retVal;

            if (!int.TryParse(value, out retVal))
                throw new Exception("Cannot parse int");

            return retVal;
        }

        /// <summary>
        /// Get the data from the provided worksheet
        /// </summary>
        /// <param name="ws">Worksheet with training data</param>
        /// <param name="hasHeader">Whether the training data has a header column</param>
        /// <returns></returns>
        private DataTable GetTrainingData(ExcelWorksheet ws, bool hasHeader)
        {
            var tbl = new DataTable();

            foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
            {
                tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
            }
            var startRow = hasHeader ? 2 : 1;
            for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
            {
                var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                DataRow row = tbl.Rows.Add();
                foreach (var cell in wsRow)
                {
                    row[cell.Start.Column - 1] = cell.Text;
                }
            }

            return tbl;
        }

        /// <summary>
        /// Filter the datatable to only show data from the selected block, ordered by week and day
        /// </summary>
        /// <param name="tbl">Datatable to filter</param>
        /// <param name="blockNum">Block number</param>
        /// <returns>Filtered data table containing only data from the selected block</returns>
        private DataTable FilterBlockData(DataTable tbl, int blockNum)
        {
            var tblFiltered = tbl.AsEnumerable()
                .Where(row => row.Field<string>(Constants.blockCol) == blockNum.ToString())
                .OrderBy(row => row.Field<string>("Week"))
                .ThenBy(row => row.Field<string>("Day"))
                .CopyToDataTable();

            return tblFiltered;
        }

        #endregion
    }
}
