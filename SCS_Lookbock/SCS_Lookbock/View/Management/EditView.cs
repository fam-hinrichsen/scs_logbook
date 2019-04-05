﻿namespace SCS_Lookbock.View.Management
{
    public class EditView<T> : AddView<T>, IEditView<T>
    {
        protected T toEdit;

        public virtual void SetEdit(T toEdit)
        {
            this.toEdit = toEdit;
        }
    }
}
