using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Validators
{
    public class ValidRoleAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is not int roleValue)
                return false;

            return Enum.IsDefined(typeof(SchoolManagement.Domain.Entities.UserRole), roleValue);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be a valid user role.";
        }
    }
}
