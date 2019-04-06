using System.Windows.Forms;

namespace SCS_Logbook.View.Management
{
    public class AddView<T> : Form, IAddView<T>
    {
        private Form parent;

        public AddView()
        {
            FormClosing += AAddView_FormClosing;
        }

        private void AAddView_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent.Enabled = true;
        }

        public void SetParent(Form form)
        {
            parent = form;
        }
    }
}
