using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketGeneratorApp.Models
{
    public class Employee
    {

        public int ID { get; set; }

        public string TicketNo
        {
            get => ID.ToString().Length < 2 ? "C  D  0  0  0  " + GetTicketValue(ID) :
                   ID.ToString().Length < 3 ? "C  D  0  0  " + GetTicketValue(ID) :
                   ID.ToString().Length < 4 ? "C  D  0  " : "";
        }

        public string Name { get => FirstName + " " + LastName; }
        public string EmpCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NoOfAttendiees { get; set; }

        private string GetTicketValue(int data)
        {
            return System.Text.RegularExpressions.Regex.Replace(Convert.ToString(data), @"(?<=.)(?!$)", " ");
        }
    }
}
