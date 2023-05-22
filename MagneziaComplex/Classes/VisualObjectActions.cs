using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MagneziaComplex.Classes
{
    internal class VisualObjectActions
    {
        public void ButtonBorderRecolor(object sender)
        {
            var button = sender as Button;
            if (button == null)
            {
                return;
            }
            var bc = new BrushConverter();
            button.BorderBrush = (Brush)bc.ConvertFrom("#3a5fff");
        }
        public void ButtonBorderReset(object sender)
        {
            var button = sender as Button;
            if (button == null)
            {
                return;
            }
            button.BorderBrush = Brushes.Transparent;
        }

        public void ButtonBorderClick(object sender)
        {
            var button = sender as Button;
            if (button == null)
            {
                return;
            }
            var bc = new BrushConverter();
            button.BorderBrush = (Brush)bc.ConvertFrom("#aa2cfc");
        }
    }
}
