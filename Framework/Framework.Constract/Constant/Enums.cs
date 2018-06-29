namespace Framework.Constract.Constant
{
	public enum OrderBy
	{
		Ascending,
		Descending
	}

	public enum Status
	{
		Success,
		Failure,
		NotFound,
		ServiceError
	}

    public enum UserAction
    {
        Login = 1,
        View = 2,
        Create = 3,
        Update = 4,
        Delete = 5,
        Import = 6,
        Export = 7,
        Logout = 8
    }

    public enum RequestState
    {
        Failed = -1,
        NotAuth = 0,
        Success = 1
    }

    public enum AudittingLevel
    {
        None = 0,   
        Basic = 1,  
        Middle = 2, 
        High = 3
    }
}
