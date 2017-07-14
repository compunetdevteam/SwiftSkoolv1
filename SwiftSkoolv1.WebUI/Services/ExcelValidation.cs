using OfficeOpenXml;
using System;
using System.Collections;

namespace HopeAcademySMS.Services
{
    public class ExcelValidation
    {
        public string ValidateExcel(int noOfRow, ExcelWorksheet workSheet, int noOfRequired)
        {
            int colum = 0;
            for (int row = 2; row <= noOfRow; row++)
            {
                ArrayList myList = new ArrayList();
                try
                {
                    for (int i = 1; i <= noOfRequired; i++)
                    {
                        myList.Add(workSheet.Cells[row, i].Value.ToString().Trim());
                        colum = i + 1;
                    }
                    //string salutation = workSheet.Cells[row, 1].Value.ToString().Trim();
                    //string firstName = workSheet.Cells[row, 2].Value.ToString().Trim();
                    //string middleName = workSheet.Cells[row, 3].Value.ToString().Trim();
                    //string lastName = workSheet.Cells[row, 4].Value.ToString().Trim();
                    //string phoneNumber = workSheet.Cells[row, 5].Value.ToString().Trim();
                    //string email = workSheet.Cells[row, 6].Value.ToString().Trim();
                    //string gender = workSheet.Cells[row, 7].Value.ToString().Trim();
                    //string address = workSheet.Cells[row, 8].Value.ToString().Trim();
                    //string stateOffOrigin = workSheet.Cells[row, 9].Value.ToString().Trim();
                    //string designation = workSheet.Cells[row, 10].Value.ToString().Trim();
                    //DateTime dateofBirth = DateTime.Parse(workSheet.Cells[row, 11].Value.ToString().Trim());
                    //string maritalStatus = workSheet.Cells[row, 12].Value.ToString().Trim();
                    //string qualification = workSheet.Cells[row, 13].Value.ToString().Trim();
                    //string password = workSheet.Cells[row, 14].Value.ToString().Trim();
                    //string username = firstName.Trim() + " " + lastName.Trim();

                }
                catch (Exception e)
                {
                    //Validation = row.ToString();
                    return row.ToString() + " " + colum.ToString();
                }
            }
            return "Success";
        }
    }
}