namespace SCS_Lookbock.View.Management
{
    public interface IEditView<T> : IAddView<T>
    {
        void SetEdit(T toEdit);
    }
}
