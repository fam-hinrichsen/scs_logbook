using System.Windows.Forms;

namespace SCS_Lookbock.View.Management
{
    public interface IEditView<T>
    {
        void SetEdit(T toEdit);
        void SetParent(Form form);
    }
}
