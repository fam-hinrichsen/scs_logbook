using SCS_Lookbock.Objects;
using System;
using System.Windows.Forms;

namespace SCS_Lookbock.View.Management.Jobmanagement
{
    public partial class NewJobView : Form, IAddView<Job>
    {
        public NewJobView()
        {
            InitializeComponent();
        }

        public void SetParent(Form form)
        {
            throw new NotImplementedException();
        }
    }
}
