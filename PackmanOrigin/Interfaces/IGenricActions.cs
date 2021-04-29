using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace PackmanOrigin.Interfaces
{
    public interface IGenricActions
    {
         void AddObjectToCanvas(Canvas can);
         void RemoveObjectFromCanvas(Canvas can);
       
    }
}
