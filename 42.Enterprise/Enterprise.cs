namespace Incapsulation.EnterpriseTask;

public class Enterprise
{
	public const string DefaultName = "Default";
	public const string DefaultInn = "0000000000";
    private readonly Guid _guid;
	private string _inn;
	public Enterprise(Guid guid)
	{
		_guid = guid;
		_inn = DefaultInn;
		Name = DefaultName;
    }

	public Guid Guid { get { return _guid; } }

	public string Inn 
	{ 
		get { return _inn; } 
		set 
		{
            if (value == null) throw new ArgumentNullException();
            if (value.Length != 10 || !value.All(z => char.IsDigit(z)))
                throw new ArgumentException();
            _inn = value; 
		} 
	}

    public string Name { get; set; }

    public DateTime EstablishDate { get; set; }

	public TimeSpan ActiveTimeSpan => DateTime.Now - EstablishDate;

	public double GetTotalTransactionsAmount()
	{
		DataBase.OpenConnection();
		var amount = 0.0;
		foreach (Transaction t in DataBase.Transactions().Where(z => z.EnterpriseGuid == _guid))
			amount += t.Amount;
		return amount;
	}
}