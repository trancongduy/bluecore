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
        Logout = 2,
        View = 4,
        Create = 8,
        Update = 16,
        Delete = 32,
        Import = 64,
        Export = 128
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
