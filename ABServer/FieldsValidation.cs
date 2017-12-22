using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ABServer
{
    class HouseValidation : ValidationRule
    {
        private int maxValue = int.MaxValue;
        private int minValue = 1;

        public int Max
        {
            get { return maxValue; }
            set { maxValue = value; }
        }
        public int Min
        {
            get { return minValue; }
            set { minValue = value; }
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int Value = 0;

            try
            {
                Value = Int32.Parse((string)value);
            }
            catch
            {
                return new ValidationResult(false, "Недопустимые символы");
            }

            if (Value < Min || Value > Max)
            {
                return new ValidationResult(false, "Значение выходит за допустимые пределы");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
