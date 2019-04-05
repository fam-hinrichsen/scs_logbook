using System.Windows.Forms;
using System;
namespace SCS_Lookbock.View.Management
{
    public interface IAddView<T>
    {
        void SetParent(Form form);
    }
}
