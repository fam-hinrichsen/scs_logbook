namespace SCS_Logbook.View.Management
{
    public interface IEditView<T> : IAddView<T>
    {
        void SetEdit(T toEdit);
    }
}
