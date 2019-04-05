using SCS_Lookbock.Objects;
using System;

namespace SCS_Lookbock.View.Management.Jobmanagement
{
    public partial class EditJobView : EditView<Job>
    {
        public EditJobView()
        {
            InitializeComponent();
        }

        public override void SetEdit(Job toEdit)
        {
            base.SetEdit(toEdit);
            throw new NotImplementedException();
        }
    }
}
