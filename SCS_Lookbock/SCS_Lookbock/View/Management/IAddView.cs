using System.Windows.Forms;

namespace SCS_Lookbock.View.Management
{
    public interface IAddView<T>
    {
        void SetParent(Form form);
    }
}
