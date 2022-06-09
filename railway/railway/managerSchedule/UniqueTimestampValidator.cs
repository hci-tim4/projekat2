﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace railway.managerSchedule
{
    public class UniqueTimestampValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
       {
            var nums = value.ToString().Split(":");
            try
            {
                if (Int32.Parse(nums[0]) > 24 || Int32.Parse(nums[0]) < 0)
                {
                    return new ValidationResult(false, "Neodgovara format");
                }
                if (Int32.Parse(nums[1]) > 60 || Int32.Parse(nums[1]) < 0)
                {
                    return new ValidationResult(false, "Neodgovara format");
                }
                if (Int32.Parse(nums[2]) > 60 || Int32.Parse(nums[2]) < 0)
                {
                    return new ValidationResult(false, "Neodgovara format");
                }
            }
            catch (Exception e) { 
                return new ValidationResult(false, "Neodgovara format");
            }
            return new ValidationResult(true, null);
        }
    }
}
