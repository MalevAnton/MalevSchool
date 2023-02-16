using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MalevSchool
{
    public partial class Service
    {
        public string CostAndTime
        {
            get
            {
                if (Discount != null)
                {
                    return Convert.ToDouble(Convert.ToDouble(Cost) - (Convert.ToDouble(Cost) * Convert.ToDouble(Discount))) + " рублей за " + sentomin + " минут";
                }

                else
                {
                    return Convert.ToDouble(Cost) + " рублей за " + sentomin + " минут";
                }

            }
        }

        public string price
        {
            get
            {
                if (Discount == null)
                {
                    return "";
                }

                else
                {
                    double a = Convert.ToDouble(Cost);
                    return a + " ";
                }

            }
        }

        public SolidColorBrush TextBrush
        {
            get
            {
                var brushConverter = new BrushConverter();

                if (Discount == null)
                {
                    return (SolidColorBrush)(Brush)brushConverter.ConvertFrom("#FFFFFF");
                }

                else
                {
                    return (SolidColorBrush)(Brush)brushConverter.ConvertFrom("#e7fabf");

                }

            }
        }

        public int sentomin
        {
            get
            {
                return (DurationInSeconds / 60);
            }
        }

        public string discount
        {
            get
            {
                if (Discount != null)
                {
                    return "* скидка " + (Discount * 100) + "% ";
                }
                else
                {
                    return "";
                }

            }
        }
    }
}
