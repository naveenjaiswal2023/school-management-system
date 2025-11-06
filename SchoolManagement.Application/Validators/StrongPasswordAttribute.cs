using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Validators
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is not string password)
                return false;

            // At least 8 characters, one uppercase, one lowercase, one digit, one special character
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            return regex.IsMatch(password);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";
        }
    }
}
