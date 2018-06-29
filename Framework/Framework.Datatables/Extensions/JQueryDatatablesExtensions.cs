namespace Framework.Datatables.Extensions
{
	public static class JQueryDatatablesExtensions
	{
		public static string SortColumnName(this JQueryDatatablesParam datatablesParam)
		{
			var columnNames = datatablesParam.sColumns.Split(',');
			return columnNames[datatablesParam.iSortCol_0];
		}
	}
}
