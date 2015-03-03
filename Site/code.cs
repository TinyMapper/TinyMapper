    /// <summary>
    /// Viewmodel for person to see.
    /// </summary>
public sealed class PersonViewModel<T> : ViewModel
{
    private string firstName;

    /// <summary>
    /// Persons first name.
    /// </summary>
    public string FirstName
    {
        get { return firstName; }
        set
        {
            firstName = value;
            OnPropertyChanged(() => FirstName);
        }
    }
}
