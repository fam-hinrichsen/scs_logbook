using SCS_Lookbock.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCS_Lookbock.View.Management.Jobmanagement
{
    public partial class EditJobView : Form, IEditView<Job>
    {
        public EditJobView()
        {
            InitializeComponent();
        }

        public void SetEdit(Job toEdit)
        {
            throw new NotImplementedException();
        }

        public void SetParent(Form form)
        {
            throw new NotImplementedException();
        }
    }
}
