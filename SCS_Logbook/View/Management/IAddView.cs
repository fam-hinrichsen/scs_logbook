using System.Windows.Forms;
using System;
namespace SCS_Logbook.View.Management
{
    public interface IAddView<T>
    {
        void SetParent(Form form);
    }
}
